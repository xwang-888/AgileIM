using AgileIM.Shared.Models.Friend.Entity;
using AgileIM.Shared.Models.Users.Entity;

using Microsoft.EntityFrameworkCore;

namespace AgileIM.Shared.EFCore.DbContexts
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
                    Signature = "但愿日子清净.抬头看见的满是柔情。",
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
                    Signature = "日无事，事复日日，忙忙亦茫茫",
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
                    Signature = "打工人，打工魂。",
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
