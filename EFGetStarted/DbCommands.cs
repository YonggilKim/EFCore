using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;


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

        public static void ShowGuilds()
        {
            using (AppDbContext db = new AppDbContext())
            {
                foreach (var guild in db.Guilds.MapGuildDto())
                {
                    Console.WriteLine($"GuildId({guild.Name}");
                }
            }
        }

        //
        public static void UpdateByReload()
        {
            //외부에서 수정 원하는 데이터의 ID와 정보를 넘겨줬다고 가정
            int id = 1;
            string name = "DWG";

            using (AppDbContext db = new AppDbContext())
            {
                Guild guild = db.Find<Guild>(id);
                guild.GuildName = name;
                db.SaveChanges();
            }
        }

        public static void UpdateByFull()
        {
            // 외부에서 길드의 모든 정보를 다 받았다고 가정
            Guild guild = GetGuildInfo();
            using (AppDbContext db = new AppDbContext())
            {
                //guild로 넘겨주는데 어떻게 찾음? -> pk로 찾기때문에 괜찮다.
                db.Guilds.Update(guild);
                db.SaveChanges();
            }
        }

        public static Guild GetGuildInfo()
        {
            return new Guild();
        }

        public static void Update_1vN()
        {
            int id = 1; // 입력받은 Guild
            using (AppDbContext db = new AppDbContext())
            {
                Guild guild = db.Guilds
                    .Include(g => g.Members)
                    .Single(g => g.GuildId == id);

                guild.Members = new List<Player>()
                {
                    new Player() {Name = "Dopa"}
                };

                db.SaveChanges();
            }
        }
    }
}
