using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EFGetStarted
{
    public class DbCommands
    {
        public static void InitializeDB(bool forceReset = false)
        {
            using (AppDbContext db = new AppDbContext())
            {
                if (!forceReset && (db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                    return;

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                CreateTestData(db);
                Console.WriteLine("DB Initialized");
            }
        }

        public static void CreateTestData(AppDbContext db)
        {
            var rookiss = new Player() { Name = "Rookiss" };
            var faker = new Player() { Name = "Faker" };
            var deft = new Player() { Name = "Deft" };

            List<Item> items = new List<Item>()
            {
                new Item()
                {
                    TemplateId = 101,
                    CreateDate = DateTime.Now,
                    Owner = rookiss
                },
                new Item()
                {
                    TemplateId = 102,
                    CreateDate = DateTime.Now,
                    Owner = faker
                },
                new Item()
                {
                    TemplateId = 103,
                    CreateDate = DateTime.Now,
                    Owner = deft
                }
            };

            Guild guild = new Guild()
            {
                GuildName = "T1",
                Members = new List<Player>() { rookiss, faker, deft }
            };

            db.Items.AddRange(items);
            db.Guilds.Add(guild);

            db.SaveChanges();
        }

        public static void ReadAll()
        {
            using (var db = new AppDbContext())
            {
                // AsNoTracking : ReadOnly << Tracking Snapshot이라고 데이터 변경 탐지하는 기능 때문
                // Include : Eager Loading (즉시 로딩) << 나중에 알아볼 것
                foreach (Item item in db.Items.AsNoTracking().Include(i => i.Owner))
                {
                    Console.WriteLine($"TemplateId({item.TemplateId}) Owner({item.Owner.Name}) Created({item.CreateDate})");
                }
            }
        }
        // 1 + 2) 특정 길드에 있는 길드원들이 소지한 모든 아이템들을 보고 싶다!

        // 장점 : DB 접근 한 번으로 다 로딩 (JOIN)
        // 단점 : 다 필요한지 모르겠는데?
        public static void EagerLoading()
        {
            Console.WriteLine("길드 이름을 입력하세요");
            Console.Write(" > ");
            string name = Console.ReadLine();

            using (var db = new AppDbContext())
            {
                Guild guild = db.Guilds.AsNoTracking()
                    .Where(g => g.GuildName == name)
                    .Include(g => g.Members)
                        .ThenInclude(p => p.Item)
                    .First();

                foreach (Player player in guild.Members)
                {
                    Console.WriteLine($"Owner({player.Name})");
                }
            }
        }

        // 장점 : 필요한 시점에 필요한 데이터만 로딩 가능
        // 단점 : DB 접근 비용
        public static void ExplicitLoading()
        {
            Console.WriteLine("길드 이름을 입력하세요");
            Console.Write(" > ");
            string name = Console.ReadLine();

            using (var db = new AppDbContext())
            {
                Guild guild = db.Guilds
                    .Where(g => g.GuildName == name)
                    .First();

                // 명시적
                db.Entry(guild).Collection(g => g.Members).Load();

                foreach (Player player in guild.Members)
                {
                    db.Entry(player).Reference(p => p.Item).Load();
                }

                foreach (Player player in guild.Members)
                {
                    Console.WriteLine($"TemplateId({player.Item.TemplateId}) Owner({player.Name})");
                }
            }
        }

        // 3) 특정 길드에 있는 길드원 수는?

        // SELECT COUNT(*)
        // 장점 : 필요한 정보만 쏘옥~ 빼서 로딩
        // 단점 : 일일히 Select 안에 만들어줘야 함
        public static void SelectLoading()
        {
            Console.WriteLine("길드 이름을 입력하세요");
            Console.Write(" > ");
            string name = Console.ReadLine();

            using (var db = new AppDbContext())
            {
                var info = db.Guilds
                    .Where(g => g.GuildName == name)
                    .MapGuildDto()
                    .First();

                Console.WriteLine($"GuildName({info.Name}), MemberCount({info.MemberCount})");
            }
        }

        public static void ShowItems()
        {
            Console.WriteLine("플레이어 이름 입력");
            Console.WriteLine(" > ");
            string name = Console.ReadLine();

            //using (var db = new AppDbContext())
            //{
            //    foreach (Player Player in db.Players.AsNoTracking().Where(p => p.Name == name).Include(i => i.Items))
            //    {
            //        foreach (Item item in Player.Items)
            //        {
            //            Console.WriteLine($"{item.TemplateId}");
            //        }
            //    }

            //}
        }

    }
}
