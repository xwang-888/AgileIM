using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AgileIM.Client.Models;

using System.Windows.Input;
using Agile.Client.Service.Api.Common;
using Agile.Client.Service.Services;
using AgileIM.Client.Common;
using AgileIM.Client.Controls;
using AgileIM.Client.Views;
using AgileIM.Shared.Models.Users.Dto;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using AgileIM.Client.Properties;
using AgileIM.IM.Models;
using AgileIM.Shared.Models.ClientModels.ChatUser.Entity;
using AgileIM.Shared.Models.ClientModels.Message.Dto;
using AgileIM.Shared.Models.ClientModels.Message.Entity;
using AgileIM.Shared.Models.Enum;
using Newtonsoft.Json;
using WebSocketSharp;
using System.Windows.Interop;

namespace AgileIM.Client.ViewModels
{
    public class ChatViewModel : ObservableObject, IRecipient<UserInfoDto>, IRecipient<IEnumerable<UserInfoDto>>
    {

        public ChatViewModel(IFriendService friendService, IChatUserService chatUserService, IMessagesService messagesService, IImService imService)
        {
            WeakReferenceMessenger.Default.Register<UserInfoDto, string>(this, "ChatViewModel");
            WeakReferenceMessenger.Default.Register<IEnumerable<UserInfoDto>, string>(this, "SetChatUserList");
            _friendService = friendService;
            _chatUserService = chatUserService;
            _messagesService = messagesService;
            _imService = imService;
            ConnectionServer();
        }

        #region Service
        private readonly IFriendService _friendService;
        private readonly IChatUserService _chatUserService;
        private readonly IMessagesService _messagesService;
        private readonly IImService _imService;
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
            set
            {
                SetProperty(ref _sendTextIsFocus, value);
                SendTextGotFocus();
            }
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
        public ICommand SendTextGotFocusCommand => new AsyncRelayCommand(SendTextGotFocus);


        #endregion

        #region Event

        private async void OnMessage(object? sender, MessageEventArgs args)
        {
            if (!args.IsText) return;

            if (string.IsNullOrEmpty(args.Data)) return;
            var msg = JsonConvert.DeserializeObject<Message>(args.Data);

            if (msg is null) return;
            var msgData = JsonConvert.DeserializeObject<SendMessageModel>(msg.Content);
            if (msgData is null) return;

            switch (msgData.Category)
            {
                case MsgCategory.SendMessage:
                    await ReceivedMessage(msgData.Messages);
                    break;
                case MsgCategory.FriendVerification:
                    ReceivedFriendVerification(msgData.Messages);
                    break;
            }

        }

        private void OnClose(object? sender, CloseEventArgs args)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        Debug.WriteLine("断线重连");
                        Task.Delay(3000);
                        mainWs.Connect();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return;
                    }
                }
            });
        }
        #endregion

        #region Methods
        /// <summary>
        /// 收到消息
        /// </summary>
        /// <param name="msgData"></param>
        /// <returns></returns>
        private async Task ReceivedMessage(Messages msgData)
        {
            var msgDto = new MessageDto()
            {
                Id = Guid.NewGuid().ToString(),
                IsRead = false,
                SendTime = msgData.SendTime,
                Content = msgData.Content
            };
            var user = ChatUserList.FirstOrDefault(a => a.Id.Equals(msgData.FromId));
            if (user is null)
            {
                var listVm = ServiceProvider.Get<MailListViewModel>();
                user = listVm.UserInfoList.FirstOrDefault(a => a.Id.Equals(msgData.FromId));
                if (user is not null)
                {
                    var result = await _chatUserService.InsertAsync(Settings.Default.LoginUser.Id, user.Id);
                    if (result is not null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ChatUserList.Insert(0, user);
                        });
                    }
                }
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var index = ChatUserList.IndexOf(user);

                    if (!index.Equals(0))
                    {
                        ChatUserList.RemoveAt(index);
                        ChatUserList.Insert(0, user);
                    }
                });
            }
            await Application.Current.Dispatcher.Invoke(async () =>
            {
                if (user is null) return;
                if (user.Id.Equals(SelectedUserInfo?.Id) && SendTextIsFocus)
                    msgDto.IsRead = true;
                else
                    WeakReferenceMessenger.Default.Send(new MainTipModel(MainTipType.AddChatMsg, 1), "UpdateTipCount");

                // 保存导本地数据库
                msgData.IsRead = msgDto.IsRead;
                var msgResult = await _messagesService.SendMessage(msgData);
                if (msgResult is null) return;
                user.Messages.Add(msgDto);
            });
        }
        /// <summary>
        /// 收到好友申请
        /// </summary>
        /// <param name="msgData"></param>
        /// <returns></returns>
        private void ReceivedFriendVerification(Messages msgData)
            => WeakReferenceMessenger.Default.Send(msgData, "FriendVerification");
        /// <summary>
        /// 消息输入框获得焦点
        /// </summary>
        /// <returns></returns>
        private async Task SendTextGotFocus()
        {
            await UpdateMsgState();
        }
        /// <summary>
        /// 连接ws
        /// </summary>
        private void ConnectionServer()
        {
            try
            {
                mainWs = new WebSocket($"{ApiRequest.WsPrefix}?Authorization={ApiConfiguration.TokenValue}");
                mainWs.Connect();
                mainWs.OnClose += OnClose;
                mainWs.OnMessage += OnMessage;
                mainWs.OnError += (sen, args) =>
                {
                    Console.WriteLine(args.Message);
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
            userNote ??= "";
            var userId = Settings.Default.LoginUser?.Id;
            var resp = await _friendService.UpdateUserNote(userId, SelectedUserInfo.Id, userNote.Trim());
            if (resp.Code.Equals(200))
            {
                SelectedUserInfo.UserNote = resp.Data;
                MessageTip.Success("修改成功");
            }
            else
                MessageTip.Error("修改备注出现错误");
        }
        /// <summary>
        /// 选中用户
        /// </summary>
        private async void OnSelectedUserInfo()
        {
            SendTextIsFocus = true;
            await UpdateMsgState();
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        private async Task SendMessage()
        {
            if (SelectedUserInfo is null) return;

            SelectedUserInfo.Messages ??= new ObservableCollection<MessageDto>();
            if (string.IsNullOrEmpty(SendText)) return;
            var msgDto = new MessageDto { Content = SendText, IsSelf = true, IsSending = true, IsRead = true };
            SelectedUserInfo.Messages.Add(msgDto);
            SendText = string.Empty;

            var result = await Send(msgDto);
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
            var index = ChatUserList.IndexOf(SelectedUserInfo);
            var model = ChatUserList.First(a => a.Id.Equals(SelectedUserInfo.Id));
            if (!index.Equals(0))
            {
                ChatUserList.RemoveAt(index);
                ChatUserList.Insert(0, model);
                SelectedUserInfo = model;
            }
            msgDto.IsSending = true;
            msgDto.IsError = false;
            var msg = new Messages()
            {
                FromId = Settings.Default.LoginUser.Id,
                TargetId = SelectedUserInfo.Id,
                Content = msgDto.Content,
                SendTime = msgDto.SendTime,
                IsRead = msgDto.IsRead,
            };
            MessageDto? result = null;
            // ws 发送消息
            var response = await _imService.SendMessage(new SendMessageModel(MsgCategory.SendMessage, msg));
            if (response.Code.Equals(200))
                // ws发送成功消息写入本地数据库
                result = await _messagesService.SendMessage(msg);

            msgDto.IsSending = false;
            return result;
        }
        /// <summary>
        /// 修改当前选中的好友消息状态
        /// </summary>
        /// <returns></returns>
        private async Task UpdateMsgState()
        {
            if (SelectedUserInfo?.Messages != null && SelectedUserInfo.Messages.Any(a => !a.IsRead))
            {
                var isOk = await _messagesService.UpdateMsgIsReadState(SelectedUserInfo.Id, Settings.Default.LoginUser.Id);
                if (isOk)
                {
                    var count = SelectedUserInfo.Messages.Count(a => !a.IsRead);
                    foreach (var messageDto in SelectedUserInfo.Messages.Where(a => !a.IsRead))
                        messageDto.IsRead = true;
                    SelectedUserInfo.UnreadMsgCount = 0;
                    WeakReferenceMessenger.Default.Send(new MainTipModel(MainTipType.RemoveChatMsg, count), "UpdateTipCount");

                }

            }
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
                    ChatUserList.Insert(0, message);
                    SelectedUserInfo = message;
                }
            }
            else
            {
                var index = ChatUserList.IndexOf(user);
                if (index is not 0)
                {
                    ChatUserList.RemoveAt(index);
                    ChatUserList.Insert(0, user);
                }
                SelectedUserInfo = user;
            }

            SendTextIsFocus = true;
        }
        public void Receive(IEnumerable<UserInfoDto> message)
        {
            ChatUserList = new ObservableCollection<UserInfoDto>(message);
            var count = ChatUserList.Select(a => a?.Messages.Count(messageDto => !messageDto.IsRead)).Sum();
            if (count is > 0)
                WeakReferenceMessenger.Default.Send(new MainTipModel(MainTipType.AddChatMsg, count.Value), "UpdateTipCount");

        }
        #endregion

        ~ChatViewModel()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }
    }
}
