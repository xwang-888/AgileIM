using AgileIM.Shared.EFCore.Data.UnitOfWork;
using AgileIM.Shared.Models.Users.Entity;

namespace AgileIM.Service.Services.Ide4Service.Impl
{
    public class VerifyService : IVerifyService
    {
        public VerifyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork _unitOfWork;

        public async Task<User?> VerifyUser(string account, string password)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password)) return null;

            var userRep = _unitOfWork.GetRepository<User>();

            return await userRep.FirstOrDefaultAsync(u => (u.Account.Equals(account) || u.Phone.Equals(account)) && u.Password.Equals(password));

        }
    }
}
