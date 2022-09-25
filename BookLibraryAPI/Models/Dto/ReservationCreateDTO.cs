using System.ComponentModel.DataAnnotations;

namespace BookLibraryAPI.Models.Dto
{
    public class ReservationCreateDTO
    {
        [Required]
        public int BookId { get; set; }
    }
}
