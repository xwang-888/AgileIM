using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Shared.Models.Users;
using AgileIM.Shared.Models.Users.Entity;
using Microsoft.EntityFrameworkCore;

namespace AgileIM.Shared.EFCore
{
    public class AgileImDbContext : DbContext
    {
        public AgileImDbContext(DbContextOptions<AgileImDbContext> options) : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Friend> Friend { get; set; }
    }
}
