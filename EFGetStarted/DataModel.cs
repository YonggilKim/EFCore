using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFGetStarted
{
    [Table("Item")]
    public class Item
    {
        public int ItemId { get; set; }
        public int TemplateId { get; set; } 
        public DateTime CreateDate { get; set; }

        [ForeignKey("OwnerId")]
        public Player Owner { get; set; }
    }

    [Table("Player")]
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public Item Item { get; set; }
        public Guild Guild { get; set; }
    }

    [Table("Guild")]
    public class Guild
    {
		public int GuildId { get; set; }
		public string GuildName { get; set; }
		public ICollection<Player> Members { get; set; }
    }

    //DTO
    public class GuildDto
    {
        public string Name { get; set; }
        public int MemberCount { get; set; }
    }

}
