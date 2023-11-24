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
                //new Item()
                //{
                //    TemplateId = 103,
                //    CreateDate = DateTime.Now,
                //    Owner = deft
                //}
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

        /// <summary>
        /// Update 3단계
        /// Tracked Entity를 얻어온다.
        /// Entity 클래스의 property를 변경
        /// SaveChanges 호출
        /// </summary>
        /// <param name="db"></param>

        public static void UpdateTest()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var guild = db.Guilds.Single(g => g.GuildName == "T1");
                guild.GuildName = "DWG";
                db.SaveChanges();
            }
        }

    }
}
