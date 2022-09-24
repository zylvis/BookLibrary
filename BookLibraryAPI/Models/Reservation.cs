namespace BookLibraryAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public DateTime Date { get; set; }
    }
}
