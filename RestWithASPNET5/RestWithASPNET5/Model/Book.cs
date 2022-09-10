using RestWithASPNET5.Model.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithASPNET5.Model
{
    [Table("books")]
    public class Book : BaseEntity
    {
        [Column("title")]
        public string Title { get; set; }
        [Column("author")]
        public string Author { get; set; }
        [Column("price")]
        public double Price { get; set; }
        [Column("launch_date")]
        public DateTime LauchDate { get; set; }
    }
}
