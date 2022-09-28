using AutoMapper;
using BookLibraryAPI.Models;
using BookLibraryAPI.Models.Dto;
using BookLibraryAPI.Repository.IRepository;
using BookLibraryAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace BookLibraryAPI.Controllers
{
    [Route("api/ReservationAPI")]
    [ApiController]
    public class ReservationAPIController : ControllerBase
    {
        protected APIResponse _response;
        private ILogger<ReservationAPIController> _logger;
        private readonly IReservationRepository _dbReservation;
        private readonly IBookRepository _dbBook;
        private readonly IBorrowingRepository _dbBorrowing;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationAPIController(IReservationRepository dbReservation, IBookRepository dbBook, ILogger<ReservationAPIController> logger,
            IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IBorrowingRepository dbBorrowing)
        {
            _dbReservation = dbReservation;
            _dbBook = dbBook;
            _logger = logger;
            _mapper = mapper;
            this._response = new();
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _dbBorrowing = dbBorrowing;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "admin, customer")]
        public async Task<ActionResult<APIResponse>> GetReservations()
        {
            try
            {
                _logger.LogInformation("Getting All reservations");
                IEnumerable<Reservation> reservationList = await _dbReservation.GetAllAsync();
                _response.Result = _mapper.Map<List<ReservationDTO>>(reservationList);
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


        [HttpGet("{id:int}", Name = "GetReservation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin, customer")]
        public async Task<ActionResult<APIResponse>> GetReservation(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogInformation("Get Reservation error with Id: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var reservation = await _dbReservation.GetAsync(x => x.Id == id);

                if (reservation == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<ReservationDTO>(reservation);
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
        public async Task<ActionResult<APIResponse>> CreateReservation([FromBody] ReservationCreateDTO createDTO)
        {
            try
            {
                string userName = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _userManager.FindByEmailAsync(userName);

                bool reserved = await _dbBook.GetAsync(x => x.Id == createDTO.BookId && x.Reserved == true) != null;
                if (reserved)
                {
                    ModelState.AddModelError("Message", "Book already Reserved");
                    return BadRequest(ModelState);
                }
                bool available = await _dbBook.GetAsync(x => x.Id == createDTO.BookId && x.AvailableStatus == SD.Available) != null;
                if (available)
                {
                    ModelState.AddModelError("Message", "Can't reserve, because Book is available");
                    return BadRequest(ModelState);
                }
                bool takenByUser = await _dbBorrowing.GetAsync(x => x.BookID == createDTO.BookId && x.UserID == user.Id) != null;
                if (!available && takenByUser)
                {
                    ModelState.AddModelError("Message", "Can't reserve, because Book is taken by You");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Reservation reservation = _mapper.Map<Reservation>(createDTO);

                await _dbReservation.CreateAsync(reservation, user.Id);
                _response.Result = _mapper.Map<Reservation>(reservation);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetReservation", new { id = reservation.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteReservation")]
        public async Task<ActionResult<APIResponse>> DeleteReservation(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var reservation = await _dbReservation.GetAsync(x => x.Id == id);
                if (reservation == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbReservation.RemoveAsync(reservation);
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
