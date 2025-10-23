using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Task_1.Models;

namespace Task_1.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public DateTime Publishing_date { get; set; }

        public int Author_Id { get; set; }

        [ForeignKey("Author_Id")]
        public virtual Author Author { get; set; }
    }
}
