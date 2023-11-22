using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFGetStarted
{
    //DB 관계 모델링
    //1:1
    //1:다
    //다:다
    // 클래스 이름 = 테이블 이름 = Item	
    [Table("Item")]
    public class Item
    {
        // 이름Id -> PK
        public int ItemId { get; set; }
        public int TemplateId { get; set; } // 101 = 집행검
        public DateTime CreateDate { get; set; }

        // 다른 클래스 참조 -> FK (Navigational Property)
        [ForeignKey("OwnerId")]
        public Player Owner { get; set; }
        //public int OwnerId { get; set; }// 자동으로 포렌키가 생김
    }

    // 클래스 이름 = 테이블 이름 = Player	
    public class Player
    {
        // 이름Id -> PK
        public int PlayerId { get; set; }
        public string Name { get; set; }

        public ICollection<Item> Items { get; set; } // 1대 다
        //public Item EquippedItem { get; set; }
    }

}
