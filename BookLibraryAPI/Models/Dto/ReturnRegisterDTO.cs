using System.ComponentModel.DataAnnotations;

namespace BookLibraryAPI.Models.Dto
{
    public class ReturnRegisterDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public DateTime Date { get; set; }
    }
}
