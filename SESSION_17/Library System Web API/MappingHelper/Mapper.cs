using AutoMapper;
using Library_System_Web_API.DTOs.Author;
using Library_System_Web_API.DTOs.Borrower;
using Library_System_Web_API.DTOs.Book;
using Library_System_Web_API.DTOs.Loan;
using Library_System_Web_API.Entities;

namespace Library_System_Web_API.MappingHelper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //  Author 

            // Entity -> SlimAuthorDTO
            CreateMap<Author, SlimAuthorDTO>();

            // Entity -> AuthorDTO (includes book list)
            CreateMap<Author, AuthorDTO>()
                .IncludeBase<Author, SlimAuthorDTO>();

            // CreateAuthorRequest -> Entity
            CreateMap<CreateAuthorRequest, Author>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Birthdate))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Books, opt => opt.Ignore());

            // UpdateAuthorRequest -> Entity
            // (inherits CreateAuthorRequest — same map, registered separately for explicitness)
            CreateMap<UpdateAuthorRequest, Author>()
                .IncludeBase<CreateAuthorRequest, Author>();

            //  Book 

            // Entity -> SlimBookDTO
            CreateMap<Book, SlimBookDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN));

            // Entity -> BookWithAuthorDTO
            CreateMap<Book, BookWithAuthorDTO>()
                .IncludeBase<Book, SlimBookDTO>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author));

            // Entity -> BookWithBorrowerDTO
            // Maps the most recent active loan (null if no active loan — resolved in service layer)
            CreateMap<Book, BookWithBorrowerDTO>()
                .IncludeBase<Book, SlimBookDTO>()
                .ForMember(dest => dest.Loan, opt => opt.MapFrom(src =>
                    src.Loans != null
                        ? src.Loans.FirstOrDefault(l => l.ReturnDate == null)
                        : null));

            // Entity -> BookDTO (author + active loan)
            CreateMap<Book, BookDTO>()
                .IncludeBase<Book, SlimBookDTO>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.Loan, opt => opt.MapFrom(src =>
                    src.Loans != null
                        ? src.Loans.FirstOrDefault(l => l.ReturnDate == null)
                        : null));

            // CreateBookRequest -> Entity
            CreateMap<CreateBookRequest, Book>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId!.Value))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Loans, opt => opt.Ignore());

            // UpdateBookRequest -> Entity
            CreateMap<UpdateBookRequest, Book>()
                .IncludeBase<CreateBookRequest, Book>();

            //  Borrower 

            // Entity -> SlimBorrowerDTO
            CreateMap<Borrower, SlimBorrowerDTO>();

            // Entity -> BorrowerDTO (includes loan history)
            CreateMap<Borrower, BorrowerDTO>()
                .IncludeBase<Borrower, SlimBorrowerDTO>()
                .ForMember(dest => dest.Loans, opt => opt.MapFrom(src => src.Loans));

            // CreateBorrowerRequest -> Entity
            CreateMap<CreateBorrowerRequest, Borrower>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.MembershipDate, opt => opt.MapFrom(src => src.MembershipDate!.Value))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Loans, opt => opt.Ignore());

            // UpdateBorrowerRequest -> Entity
            CreateMap<UpdateBorrowerRequest, Borrower>()
                .IncludeBase<CreateBorrowerRequest, Borrower>();

            //  Loan 

            // Entity -> SlimLoanDTO
            CreateMap<Loan, SlimLoanDTO>();

            // Entity -> LoanDTO (includes slim borrower + slim book)
            CreateMap<Loan, LoanDTO>()
                .IncludeBase<Loan, SlimLoanDTO>()
                .ForMember(dest => dest.Borrower, opt => opt.MapFrom(src => src.Borrower))
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book));

            // Entity -> LoanWithBorrowerDTO  (used inside BookDTO / BookWithBorrowerDTO)
            CreateMap<Loan, LoanWithBorrowerDTO>()
                .ForMember(dest => dest.Borrower, opt => opt.MapFrom(src => src.Borrower))
                .ForMember(dest => dest.LoanDate, opt => opt.MapFrom(src => src.LoanDate))
                .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate));

            // Entity -> LoanWithBookDTO  (used inside BorrowerDTO)
            CreateMap<Loan, LoanWithBookDTO>()
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book))
                .ForMember(dest => dest.LoanDate, opt => opt.MapFrom(src => src.LoanDate))
                .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate));

            // CreateLoanRequest -> Entity
            CreateMap<CreateLoanRequest, Loan>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.BorrowerId, opt => opt.MapFrom(src => src.BorrowerId))
                .ForMember(dest => dest.LoanDate, opt => opt.MapFrom(src => src.LoanDate!.Value))
                .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.Borrower, opt => opt.Ignore());

            // UpdateLoanRequest -> Entity
            CreateMap<UpdateLoanRequest, Loan>()
                .IncludeBase<CreateLoanRequest, Loan>();
        }
    }
}