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
            var user1id = Guid.NewGuid().ToString();
            var user2id = Guid.NewGuid().ToString();
            var user3id = Guid.NewGuid().ToString();
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = user1id,
                    Account = "Y12345678_",
                    Password = "admin123",
                    Phone = "15991388888",
                    Nick = "Private_",
                    Address = "陕西 西安",
                    Gender = 1,
                    LastLoginTime = DateTime.Now
                },
                new User()
                {
                    Id = user2id,
                    Account = "X1987872",
                    Password = "admin123",
                    Phone = "18790998728",
                    Nick = "Patton",
                    Address = "广东 东莞",
                    Gender = 2,
                    LastLoginTime = DateTime.Now
                },
                new User()
                {
                    Id = user3id,
                    Account = "WX1987921",
                    Password = "admin123",
                    Phone = "17098787827",
                    Nick = "Venus",
                    Address = "广东 广州",
                    Gender = 1,
                    LastLoginTime = DateTime.Now
                });
            modelBuilder.Entity<Friend>().HasData(
                new Friend() { UserId = user1id, FriendId = user2id, State = 1, UserNote = "Captain America" },
                new Friend() { UserId = user1id, FriendId = user3id, State = 1 },
                new Friend() { UserId = user2id, FriendId = user1id, State = 1, UserNote = "Iron Man" },
                new Friend() { UserId = user2id, FriendId = user3id, State = 1 },
                new Friend() { UserId = user3id, FriendId = user1id, State = 1 },
                new Friend() { UserId = user3id, FriendId = user2id, State = 1 }
                    );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Friend> Friend { get; set; }
    }
}
