using AgileIM.Shared.Models.Friend.Entity;
using AgileIM.Shared.Models.Users.Entity;

namespace AgileIM.Shared.EFCore.Data.Repository
{
    public class FriendRepository : RepositoryBase<Friend>, IRepositoryBase<Friend>
    {
        public FriendRepository(AgileImDbContext dbContext) : base(dbContext) { }
    }
}
