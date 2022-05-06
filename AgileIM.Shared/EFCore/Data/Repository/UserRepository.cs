using AgileIM.Shared.Models.Users.Entity;

namespace AgileIM.Shared.EFCore.Data.Repository
{
    public class UserRepository : RepositoryBase<User>, IRepositoryBase<User>
    {
        public UserRepository(AgileImDbContext dbContext) : base(dbContext) { }
    }
}
