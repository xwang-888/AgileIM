using AgileIM.Shared.EFCore;
using AgileIM.Shared.Models.Users;
using AgileIM.Shared.Models.Users.Entity;
using Microsoft.EntityFrameworkCore;

namespace AgileIM.Service.Data.Repository
{
    public class UserRepository : RepositoryBase<User>, IRepositoryBase<User>
    {
        public UserRepository(AgileImDbContext dbContext) : base(dbContext) { }
    }
}
