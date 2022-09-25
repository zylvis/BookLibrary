using AutoMapper;
using BookLibraryAPI.Models;
using BookLibraryAPI.Models.Dto;
using BookLibraryAPI.Repository.IRepository;
using BookLibraryAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookLibraryAPI.Controllers
{
    [Route("api/BorrowingAPI")]
    [ApiController]
    public class BorrowingAPIController : ControllerBase
    {
        protected APIResponse _response;
        private ILogger<BorrowingAPIController> _logger;
        private readonly IBorrowingRepository _dbBorrowing;
        private readonly IBookRepository _dbBook;
        private readonly IReservationRepository _dbReservation;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public BorrowingAPIController(IBorrowingRepository dbBorrowing, IBookRepository dbBook, IReservationRepository dbReservation,
                                    ILogger<BorrowingAPIController> logger,IMapper mapper, IHttpContextAccessor httpContextAccessor,
                                    UserManager<ApplicationUser> userManager)
        {
            _dbBorrowing = dbBorrowing;
            _dbBook = dbBook;
            _dbReservation = dbReservation;
            _logger = logger;
            _mapper = mapper;
            this._response = new();
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "admin, customer")]
        public async Task<ActionResult<APIResponse>> GetBorrowings()
        {
            try
            {
                _logger.LogInformation("Getting All borrowings");
                IEnumerable<Borrowing> borrowingList = await _dbBorrowing.GetAllAsync();
                _response.Result = _mapper.Map<List<BorrowingDTO>>(borrowingList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMesseges = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("{id:int}", Name = "GetBorrowing")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin, customer")]
        public async Task<ActionResult<APIResponse>> GetBorrowing(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogInformation("Get Borrowing error with Id: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var borrowing = await _dbBorrowing.GetAsync(x => x.Id == id);

                if (borrowing == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<BorrowingDTO>(borrowing);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMesseges = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "admin, customer")]
        public async Task<ActionResult<APIResponse>> CreateBorrowing([FromBody] BorrowingCreateDTO createDTO)
        {
            try
            {
                bool unavailable = await _dbBook.GetAsync(x => x.Id == createDTO.BookID && x.AvailableStatus == SD.Unavailable) != null;
                if (unavailable)
                {
                    ModelState.AddModelError("", "Book already Taken!");
                    return BadRequest(ModelState);
                }

                string userName = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _userManager.FindByEmailAsync(userName);

                bool reserved = await _dbBook.GetAsync(x => x.Id == createDTO.BookID && x.Reserved == true) != null;
                bool reservedByUser = await _dbReservation.GetAsync(x => x.UserId == user.Id && x.BookId == createDTO.BookID) == null;
                if (reserved && reservedByUser)
                {
                    ModelState.AddModelError("", "Can't Take, beacause Book is Reserved!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                
                Borrowing borrowing = _mapper.Map<Borrowing>(createDTO);

                await _dbBorrowing.CreateAsync(borrowing, user.Id);
                _response.Result = _mapper.Map<Borrowing>(borrowing);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetBorrowing", new { id = borrowing.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMesseges = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}", Name = "DeleteBorrowing")]
        public async Task<ActionResult<APIResponse>> DeleteBorrowing(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var borrowing = await _dbBorrowing.GetAsync(x => x.Id == id);
                if (borrowing == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbBorrowing.RemoveAsync(borrowing);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMesseges = new List<string> { ex.ToString() };
            }
            return _response;
        }

    }
}
