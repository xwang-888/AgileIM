using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Shared.EFCore.DbContexts;
using AgileIM.Shared.Models.ClientModels.ChatUser.Entity;

using Microsoft.EntityFrameworkCore;

namespace AgileIM.Shared.EFCore.Data.Repository.Client
{
    public class ChatUserRepository : RepositoryBase<ChatUser>, IRepositoryBase<ChatUser>
    {
        public ChatUserRepository(AgileImClientDbContext dbContext) : base(dbContext)
        {
        }
    }
}
