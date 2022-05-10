using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Shared.EFCore.DbContexts;
using AgileIM.Shared.Models.ClientModels.Message.Entity;

using Microsoft.EntityFrameworkCore;

namespace AgileIM.Shared.EFCore.Data.Repository.Client
{
    public class MessagesRepository : RepositoryBase<Messages>, IRepositoryBase<Messages>
    {
        public MessagesRepository(AgileImClientDbContext dbContext) : base(dbContext)
        {
        }
    }
}
