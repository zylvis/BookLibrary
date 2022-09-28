namespace BookLibraryAPI.Models.Dto
{
    public class BorrowingJoinDTO
    {
        public string UserID { get; set; }
        public int BookID { get; set; }
        public string Name { get; set; }
    }
}
