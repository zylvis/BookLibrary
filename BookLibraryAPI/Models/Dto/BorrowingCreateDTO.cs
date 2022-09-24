﻿using System.ComponentModel.DataAnnotations;

namespace BookLibraryAPI.Models.Dto
{
    public class BorrowingCreateDTO
    {
        [Required]
        public int BookID { get; set; }
        [Required]
        public string UserID { get; set; }
    }
}
