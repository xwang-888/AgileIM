using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgileIM.Shared.Models.Users.Entity
{
    /// <summary>
    /// 好友表
    /// </summary>
    [Table("Friend")]
    public class Friend : BaseEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public string UserId { get; set; }
        /// <summary>
        /// 朋友Id
        /// </summary>
        [Required]
        public string FriendId { get; set; }
        /// <summary>
        /// 状态 0单向 1双向 
        /// </summary>
        public int State { get; set; }
    }
}
