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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var user1id = "7435D03A-43E0-E6E8-58FC-A5C371A32170";
            var user2id = "1E765565-2ED2-5FA9-D132-AD94B31CC1EB";
            var user3id = "3BCDF24E-919B-74C0-B031-D619693D2D29";
            modelBuilder.Entity<ChatUser>().HasData(
                new ChatUser { UserId = user1id, FriendId = user2id },
                new ChatUser { UserId = user2id, FriendId = user1id },
                new ChatUser { UserId = user3id, FriendId = user2id });
            modelBuilder.Entity<Messages>().HasData(
                new Messages { FromId = user1id, TargetId = user2id, Content = "Hello World!", IsRead = true, SendTime = new DateTime(2022, 5, 10, 8, 21, 30) },
                new Messages { FromId = user2id, TargetId = user1id, Content = "Hello Captain America!", IsRead = true, SendTime = new DateTime(2022, 5, 10, 8, 22, 30) },
                new Messages { FromId = user2id, TargetId = user1id, Content = "Nice to meet you!", IsRead = true, SendTime = new DateTime(2022, 5, 10, 8, 23, 30) },
                new Messages { FromId = user1id, TargetId = user2id, Content = "Me too!", IsRead = true, SendTime = new DateTime(2022, 5, 10, 8, 24, 30) },
                new Messages { FromId = user1id, TargetId = user2id, Content = "Thank You!", IsRead = true, SendTime = new DateTime(2022, 5, 10, 8, 25, 30) },
                new Messages { FromId = user3id, TargetId = user2id, Content = "Hello World!", IsRead = true, SendTime = new DateTime(2022, 5, 10, 7, 21, 50) },
                new Messages { FromId = user2id, TargetId = user3id, Content = "Hello Avengers!", IsRead = true, SendTime = new DateTime(2022, 5, 10, 7, 22, 50) },
                new Messages { FromId = user3id, TargetId = user2id, Content = "Nice to meet you!", IsRead = true, SendTime = new DateTime(2022, 5, 10, 7, 23, 50) },
                new Messages { FromId = user2id, TargetId = user3id, Content = "Me too!", IsRead = true, SendTime = new DateTime(2022, 5, 10, 7, 24, 50) },
                new Messages { FromId = user3id, TargetId = user2id, Content = "Thank You!", IsRead = true, SendTime = new DateTime(2022, 5, 10, 7, 25, 50) }
            );
        }

        public DbSet<Messages> Messages { get; set; }

        public DbSet<ChatUser> ChatUsers { get; set; }

    }
}
