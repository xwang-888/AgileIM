using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Shared.Models.ClientModels.ChatUser.Entity;
using AgileIM.Shared.Models.ClientModels.Message.Entity;

using Microsoft.EntityFrameworkCore;

namespace AgileIM.Shared.EFCore.DbContexts
{
    public class AgileImClientDbContext : DbContext
    {

        public AgileImClientDbContext(DbContextOptions<AgileImClientDbContext> options) : base(options)
        {

        }

        public DbSet<Messages> Messages { get; set; }

        public DbSet<ChatUser> ChatUsers { get; set; }

    }
}
