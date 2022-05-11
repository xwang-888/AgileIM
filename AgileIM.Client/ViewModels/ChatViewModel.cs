using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Client.Models;

using System.Windows.Input;
using Agile.Client.Service.Services;
using AgileIM.Client.Controls;
using AgileIM.Client.Views;
using AgileIM.Shared.Models.Users.Dto;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using AgileIM.Client.Properties;
using AgileIM.Shared.Models.ClientModels.ChatUser.Entity;

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
        }

        #region Service
        private readonly IFriendService _friendService;
        private readonly IChatUserService _chatUserService;
        private readonly IMessagesService _messagesService;
        #endregion

        #region Property

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
        public ICommand CreateChatCommand => new AsyncRelayCommand(CreateCha);
        public ICommand UpdateUserNoteCommand => new AsyncRelayCommand<string?>(UpdateUserNote);


        #endregion

        #region Methods

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
            _userId ??= Settings.Default.LoginUser.Id;
            foreach (var messageDto in SelectedUserInfo.Messages)
                messageDto.IsRead = true;
            if (string.IsNullOrEmpty(SendText)) return;
            var msgDto = new MessageDto { Content = SendText, IsSelf = true };
            SelectedUserInfo.Messages.Add(msgDto);
            SendText = string.Empty;
            var result = await _messagesService.SendMessage(new Shared.Models.ClientModels.Message.Entity.Messages()
            {
                FromId = Settings.Default.LoginUser.Id,
                TargetId = SelectedUserInfo.Id,
                Content = msgDto.Content,
                SendTime = msgDto.SendTime,
                IsRead = true,
            });

            if (result is null)
            {
                //TODO 发送消息失败
            }


        }

        private Task CreateCha()
        {
            DialogHostHelper.ShowDialog(new CreateChatView());

            return Task.CompletedTask;
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
