using AutoMapper;
using BookLibraryAPI.Data;
using BookLibraryAPI.Models;
using BookLibraryAPI.Models.Dto;
using BookLibraryAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookLibraryAPI.Controllers
{  
    [Route("api/BookAPI")]
    [ApiController]
    public class BookAPIController : ControllerBase
    {
        protected APIResponse _response;
        private ILogger<BookAPIController> _logger;
        private readonly IBookRepository _dbBook;
        private readonly IMapper _mapper;

        public BookAPIController(IBookRepository dbBook, ILogger<BookAPIController> logger, IMapper mapper)
        {
            _dbBook = dbBook;
            _logger = logger;
            _mapper = mapper;
            this._response = new();

        }
      
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Authorize(Roles = "admin, customer")]
        public async Task<ActionResult<APIResponse>> GetBooks()
        {
            try
            {
                _logger.LogInformation("Getting All books");
                IEnumerable<Book> bookList = await _dbBook.GetAllAsync();
                _response.Result = _mapper.Map<List<BookDTO>>(bookList);
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


        [HttpGet("/book/{search}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Authorize(Roles = "admin, customer")]
        public async Task<ActionResult<APIResponse>> SearchBooks(string search)
        {
            try
            {
                _logger.LogInformation("Getting All books");
                IEnumerable<Book> bookList = await _dbBook.GetAllAsync(x => x.SearchColumn.Contains(search.ToLower()));
                _response.Result = _mapper.Map<List<BookDTO>>(bookList);
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

        [HttpGet("{id:int}", Name = "GetBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "admin, customer")]
        public async Task<ActionResult<APIResponse>> GetBook(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogInformation("Get Book error with Id: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var book = await _dbBook.GetAsync(x => x.Id == id);

                if (book == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<BookDTO>(book);
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
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> CreateBook([FromBody] BookCreateDTO createDTO)
        {
            try
            {
                if (await _dbBook.GetAsync(x => x.Title.ToLower() == createDTO.Title.ToLower()) != null)
                {
                    ModelState.AddModelError("", "Book already Exists!");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Book book = _mapper.Map<Book>(createDTO);

                await _dbBook.CreateAsync(book);
                _response.Result = _mapper.Map<BookCreateDTO>(book);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetBook", new { id = book.Id }, _response);
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
        //[Authorize(Roles = "admin")]
        [HttpDelete("{id:int}", Name = "DeleteBook")]
        public async Task<ActionResult<APIResponse>> DeleteBook(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var book = await _dbBook.GetAsync(x => x.Id == id);
                if (book == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbBook.RemoveAsync(book);
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

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateBook")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdateBook(int id, [FromBody] BookUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                Book model = _mapper.Map<Book>(updateDTO);

                await _dbBook.UpdateAsync(model);
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
