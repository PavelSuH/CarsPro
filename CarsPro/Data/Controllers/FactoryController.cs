using AutoMapper;
using CarsPro.Data.Contracts;
using CarsPro.Data.DTO_s;
using CarsPro.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsPro.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    
    public class FactoryController : ControllerBase
    {
        private readonly IFactoryRepository _factoryRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public FactoryController(IFactoryRepository carRepository, ILoggerService logger, IMapper mapper)
        {
            _factoryRepository = carRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFactories()
        {

            try
            {
                _logger.LogInfo("Attempted are all cars");
                var factories = await _factoryRepository.FindAll();
                var response = _mapper.Map<IList<FactoryDTO>>(factories).ToList();
                _logger.LogInfo("Successfully got all factories");
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} - {e.InnerException}");
                return StatusCode(500, "something went wrong");
            }

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFactory(int id)
        {
            try
            {
                _logger.LogInfo($"Attempted get factory with id: {id}");
                var factory = await _factoryRepository.FindById(id);
                if (factory == null)
                {
                    _logger.LogInfo("Factory wasn't  found");
                    return NotFound();
                }
                var response = _mapper.Map<FactoryDTO>(factory);
                _logger.LogInfo($"Successfully get factory with id: {id}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusMessage($"{e.Message}, {e.InnerException}");
            }
        }
        /// <summary>
        /// Create car
        /// </summary>
        /// <param name="carDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] FactoryDTO factoryDTO)
        {
            try
            {
                _logger.LogInfo("Factory submission atempted");
                if (factoryDTO == null)
                {
                    _logger.LogWarn("Trying to put empty request");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("request is not valid");
                    return BadRequest(ModelState);
                }
                var factory = _mapper.Map<Factory>(factoryDTO);
                
               var isSuccess = await _factoryRepository.Create(factory);
                if(!isSuccess)
                {
                    _logger.LogWarn("Factory creation filed");
                    return BadRequest(ModelState);
                }
                _logger.LogInfo("Factory created");
                return Created("", new { factory });

            }
            catch (Exception e)
            {

                return StatusMessage($"{e.Message} - {e.InnerException}");
            }
        }
        [Authorize(Roles = "Administrator, Customer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FactoryDTO factoryDto)
        {
            try
            {
                if (id < 1 || factoryDto == null || id != factoryDto.Id)
                {
                    _logger.LogInfo("Trying to put empty model");
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo("Some field's didnt write");
                    return BadRequest(ModelState);
                }
                var isExist = await _factoryRepository.IsExists(id);
                if (!isExist)
                {
                    _logger.LogWarn("Car is not found");
                    return NotFound();
                }


                var factory = _mapper.Map<Factory>(factoryDto);
                var isSuccess = await _factoryRepository.Update(factory);
                if (!isSuccess)
                {
                    return (StatusMessage("Bad bad bad"));
                }
                return Ok();
            }
            catch (Exception e)
            {
                return StatusMessage($"{e.Message} - {e.InnerException}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            var isExist = await _factoryRepository.IsExists(id);
            if (!isExist)
            {
                _logger.LogWarn("Car is not found");
                return NotFound();
            }
            var car = await _factoryRepository.FindById(id);

            var isSuccess = await _factoryRepository.Delete(car);
            if (!isSuccess)
            {
                return StatusMessage("Delete failed");
            }
            return NoContent();
        }

        private ObjectResult StatusMessage(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "something went wrong");
        }
    }
}
