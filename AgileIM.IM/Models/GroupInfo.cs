namespace AgileIM.IM.Models
{
    public class GroupInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户列表
        /// </summary>
        public List<UserInfo> UserInfos { get; set; }
    }
}
