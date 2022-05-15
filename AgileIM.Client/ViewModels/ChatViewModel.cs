using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AgileIM.Client.Models;

using System.Windows.Input;
using Agile.Client.Service.Api.Common;
using Agile.Client.Service.Services;
using AgileIM.Client.Controls;
using AgileIM.Client.Views;
using AgileIM.Shared.Models.Users.Dto;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using AgileIM.Client.Properties;
using AgileIM.Shared.Models.ClientModels.ChatUser.Entity;
using AgileIM.Shared.Models.ClientModels.Message.Dto;
using WebSocketSharp;

namespace AgileIM.Client.ViewModels
{
    public class ChatViewModel : ObservableObject, IRecipient<UserInfoDto>, IRecipient<IEnumerable<UserInfoDto>>
    {

        public ChatViewModel(IFriendService friendService, IChatUserService chatUserService, IMessagesService messagesService)
        {
            WeakReferenceMessenger.Default.Register<UserInfoDto, string>(this, "ChatViewModel");
            WeakReferenceMessenger.Default.Register<IEnumerable<UserInfoDto>, string>(this, "SetChatUserList");
            _friendService = friendService;
            _chatUserService = chatUserService;
            _messagesService = messagesService;
            ConnectionServer();
        }

        #region Service
        private readonly IFriendService _friendService;
        private readonly IChatUserService _chatUserService;
        private readonly IMessagesService _messagesService;
        #endregion

        #region Property

        private WebSocketSharp.WebSocket mainWs;
        private string? _userId;
        private ObservableCollection<UserInfoDto> _chatUserList = new();
        private UserInfoDto? _selectedUserInfo;
        private bool _sendTextIsFocus;
        private string _sendText;


        /// <summary>
        /// 好友列表
        /// </summary>
        public ObservableCollection<UserInfoDto> ChatUserList
        {
            get => _chatUserList;
            set => SetProperty(ref _chatUserList, value);
        }
        /// <summary>
        /// 当前选中的user
        /// </summary>
        public UserInfoDto? SelectedUserInfo
        {
            get => _selectedUserInfo;
            set
            {
                SetProperty(ref _selectedUserInfo, value);
                OnSelectedUserInfo();
            }
        }
        /// <summary>
        /// 发送消息文本框焦点
        /// </summary>
        public bool SendTextIsFocus
        {
            get => _sendTextIsFocus;
            set => SetProperty(ref _sendTextIsFocus, value);
        }
        /// <summary>
        /// 消息文本框
        /// </summary>
        public string SendText
        {
            get => _sendText;
            set => SetProperty(ref _sendText, value);
        }
        #endregion

        #region Command
        public ICommand SendMessageCommand => new AsyncRelayCommand(SendMessage);
        public ICommand ResendMessageCommand => new AsyncRelayCommand<MessageDto>(ResendMessage);
        public ICommand CreateChatCommand => new AsyncRelayCommand(CreateChat);
        public ICommand UpdateUserNoteCommand => new AsyncRelayCommand<string?>(UpdateUserNote);


        #endregion

        #region Methods



        private void ConnectionServer()
        {
            try
            {
                mainWs = new WebSocket($"ws://localhost:9659/ws/?Authorization={ApiConfiguration.TokenValue}");
                mainWs.Connect();
                mainWs.OnClose += (sen, args) =>
                {
                    Task.Run(() =>
                    {
                        while (true)
                        {
                            try
                            {
                                Debug.WriteLine("断线重连");
                                Task.Delay(5000);
                                mainWs.Connect();
                            }
                            catch (Exception e)
                            {
                                return;
                            }
                        }
                    });
                };
                mainWs.OnMessage += (sen, args) =>
                {
                    if (args.IsText)
                    {
                        MessageBox.Show(args.Data);
                    }

                };
                mainWs.OnError += (sen, args) =>
                {

                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 修改好友备注
        /// </summary>
        /// <param name="userNote"></param>
        /// <returns></returns>
        private async Task UpdateUserNote(string? userNote)
        {
            if (string.IsNullOrEmpty(userNote?.Trim()))
            {
                return;
            }

            var note = SelectedUserInfo?.UserNote?.Trim();
            var newNote = userNote.Trim();

            if (note?.Equals(newNote) is true)
            {
                return;
            }

            var userId = Settings.Default.LoginUser?.Id;
            var resp = await _friendService.UpdateUserNote(userId, SelectedUserInfo.Id, userNote);
            SelectedUserInfo.UserNote = resp.Code.Equals(200) ? resp.Data : null;
        }
        /// <summary>
        /// 选中用户
        /// </summary>
        private void OnSelectedUserInfo()
        {
            SendTextIsFocus = true;
            SelectedUserInfo.IsUnreadMessage = false;
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        private async Task SendMessage()
        {
            if (SelectedUserInfo is null) return;

            SelectedUserInfo.Messages ??= new ObservableCollection<MessageDto>();
            foreach (var messageDto in SelectedUserInfo.Messages)
                messageDto.IsRead = true;
            if (string.IsNullOrEmpty(SendText)) return;
            var msgDto = new MessageDto { Content = SendText, IsSelf = true, IsSending = true, IsRead = true };
            SelectedUserInfo.Messages.Add(msgDto);
            SendText = string.Empty;

            MessageDto result = null;
            await Task.Delay(3000);
            msgDto.IsSending = false;
            if (result is null)
            {
                //TODO 发送消息失败
                msgDto.IsError = true;
            }

        }
        /// <summary>
        /// 重新发送消息
        /// </summary>
        private async Task ResendMessage(MessageDto msgDto)
        {
            var result = await Send(msgDto);

            if (result is null)
            {
                //TODO 发送消息失败
                msgDto.IsError = true;
            }
        }
        /// <summary>
        /// 创建聊天
        /// </summary>
        /// <returns></returns>
        private Task CreateChat()
        {
            DialogHostHelper.ShowDialog(new CreateChatView());

            return Task.CompletedTask;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgDto"></param>
        /// <returns></returns>
        private async Task<MessageDto?> Send(MessageDto? msgDto)
        {
            if (SelectedUserInfo is null) return null;
            if (msgDto is null) return null;

            msgDto.IsSending = true;
            msgDto.IsError = false;
            _userId ??= Settings.Default.LoginUser.Id;

            var result = await _messagesService.SendMessage(new Shared.Models.ClientModels.Message.Entity.Messages()
            {
                FromId = _userId,
                TargetId = SelectedUserInfo.Id,
                Content = msgDto.Content,
                SendTime = msgDto.SendTime,
                IsRead = msgDto.IsRead,
            });
            msgDto.IsSending = false;
            return result;
        }
        #endregion

        #region Recipient
        public async void Receive(UserInfoDto message)
        {
            var user = ChatUserList.FirstOrDefault(a => a.Id.Equals(message.Id));
            if (user is null)
            {
                var result = await _chatUserService.InsertAsync(Settings.Default.LoginUser.Id, message.Id);
                if (result is not null)
                {
                    ChatUserList.Add(message);
                    SelectedUserInfo = message;
                }
            }
            else
                SelectedUserInfo = user;

            SendTextIsFocus = true;
        }
        public void Receive(IEnumerable<UserInfoDto> message)
        {
            ChatUserList = new ObservableCollection<UserInfoDto>(message);
        }
        #endregion

        ~ChatViewModel()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }
    }
}
