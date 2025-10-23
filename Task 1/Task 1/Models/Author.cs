using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Task_1.Models;

namespace Task_1.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string First_name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Last_name { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

        [NotMapped]
        public string Full_name => $"{First_name} {Last_name}";
    }
}
