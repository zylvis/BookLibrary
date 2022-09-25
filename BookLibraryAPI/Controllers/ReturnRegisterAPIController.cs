using AutoMapper;
using BookLibraryAPI.Models;
using BookLibraryAPI.Models.Dto;
using BookLibraryAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookLibraryAPI.Controllers
{
    [Route("api/ReturnRegisterAPI")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ReturnRegisterAPIController : ControllerBase
    {
        protected APIResponse _response;
        private ILogger<ReturnRegisterAPIController> _logger;
        private readonly IReturnRegisterRepository _dbReturnRegister;
        private readonly IMapper _mapper;

        public ReturnRegisterAPIController(IReturnRegisterRepository dbReturnRegister, ILogger<ReturnRegisterAPIController> logger,
                                            IMapper mapper)
        {
            _dbReturnRegister = dbReturnRegister;
            _logger = logger;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetReturnRegisters()
        {
            try
            {
                _logger.LogInformation("Getting All ReturnRegisters");
                IEnumerable<ReturnRegister> returnRegisterList = await _dbReturnRegister.GetAllAsync();
                _response.Result = _mapper.Map<List<ReturnRegisterDTO>>(returnRegisterList);
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

        [HttpGet("{id:int}", Name = "GetReturnRegister")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetReturnRegister(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogInformation("Get ReturnRegister error with Id: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var returnRegister = await _dbReturnRegister.GetAsync(x => x.Id == id);

                if (returnRegister == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<ReturnRegisterDTO>(returnRegister);
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
        public async Task<ActionResult<APIResponse>> CreateReturnRegister([FromBody] ReturnRegisterCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                ReturnRegister returnRegister = _mapper.Map<ReturnRegister>(createDTO);

                await _dbReturnRegister.CreateAsync(returnRegister);
                _response.Result = _mapper.Map<ReturnRegisterCreateDTO>(returnRegister);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetReturnRegister", new { id = returnRegister.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteReturnRegister")]
        public async Task<ActionResult<APIResponse>> DeleteReturnRegister(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var returnRegister = await _dbReturnRegister.GetAsync(x => x.Id == id);
                if (returnRegister == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbReturnRegister.RemoveAsync(returnRegister);
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
