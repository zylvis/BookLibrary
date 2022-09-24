using System.ComponentModel.DataAnnotations;

namespace BookLibraryAPI.Models.Dto
{
    public class ReservationDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
    }
}
