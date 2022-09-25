using System.ComponentModel.DataAnnotations;

namespace BookLibraryAPI.Models.Dto
{
    public class ReturnRegisterCreateDTO
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int BookId { get; set; }
    }
}
