namespace AgileIM.Shared.Models.Friend.Request
{
    public class FriendRequest
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }
    }
    public class UpdateUserNoteRequest
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public string UserNote { get; set; }
    }
}
