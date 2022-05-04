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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User()
            {
                Account = "Y12345678_",
                Password = "admin123",
                Phone = "15991388888",
                Nick = "Private_",
                Address = "陕西 西安",
                Gender = 1,
                LastLoginTime = DateTime.Now
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Friend> Friend { get; set; }
    }
}
