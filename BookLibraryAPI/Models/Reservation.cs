using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibraryAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }
        public DateTime Date { get; set; }
    }
}
