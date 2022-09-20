using System.ComponentModel.DataAnnotations;

namespace BookLibraryAPI.Models.Dto
{
    public class BookCreateDTO
    {
        [Required]
        public string Author { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public double ISBN { get; set; }
        [Required]
        public int Year { get; set; }
        public int Quantity { get; set; }
    }
}
