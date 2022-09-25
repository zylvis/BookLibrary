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

            CreateMap<Reservation, ReservationDTO>().ReverseMap();
            CreateMap<Reservation, ReservationCreateDTO>().ReverseMap();

            CreateMap<ReturnRegister, ReturnRegisterDTO>().ReverseMap();
            CreateMap<ReturnRegister, ReturnRegisterCreateDTO>().ReverseMap();


            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
