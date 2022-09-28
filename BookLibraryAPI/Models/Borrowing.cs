using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibraryAPI.Models
{
    public class Borrowing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Book")]
        public int BookID { get; set; }
        public Book Book { get; set; }
        public string UserID { get; set; }
        public DateTime Date { get; set; }

    }
}
