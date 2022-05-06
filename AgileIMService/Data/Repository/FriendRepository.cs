using AgileIM.Shared.EFCore;
using AgileIM.Shared.Models.Users.Entity;

using Microsoft.EntityFrameworkCore;

namespace AgileIM.Service.Data.Repository
{
    public class FriendRepository : RepositoryBase<Friend>, IRepositoryBase<Friend>
    {
        public FriendRepository(AgileImDbContext dbContext) : base(dbContext) { }
    }
}
