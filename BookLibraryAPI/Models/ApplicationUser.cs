using Microsoft.AspNetCore.Identity;

namespace BookLibraryAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
