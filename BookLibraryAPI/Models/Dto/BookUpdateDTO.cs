using System.ComponentModel.DataAnnotations;

namespace BookLibraryAPI.Models.Dto
{
    public class BookUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Author { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public double ISBN { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Publisher { get; set; }
        public string Genre { get; set; }
        public string AvailableStatus { get; set; }

    }
}
