using AutoMapper;
using BookLibraryAPI.Models;
using BookLibraryAPI.Models.Dto;

namespace BookLibraryAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Book, BookDTO>();
            CreateMap<BookDTO, Book>();

            CreateMap<Book, BookCreateDTO>().ReverseMap();
            CreateMap<Book, BookUpdateDTO>().ReverseMap();

            CreateMap<Borrowing, BorrowingDTO>().ReverseMap();
            CreateMap<Borrowing, BorrowingCreateDTO>().ReverseMap();
            CreateMap<Borrowing, BorrowingUpdateDTO>().ReverseMap();


            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
