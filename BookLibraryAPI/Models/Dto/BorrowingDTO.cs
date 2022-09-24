namespace BookLibraryAPI.Models.Dto
{
    public class BorrowingDTO
    {
        public int Id { get; set; }
        public int BookID { get; set; }
        public string UserID { get; set; }
    }
}
