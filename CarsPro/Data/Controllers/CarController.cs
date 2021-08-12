using AutoMapper;
using CarsPro.Data.Contracts;
using CarsPro.Data.DTO_s;
using CarsPro.Entity;
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
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public CarController(ICarRepository carRepository, ILoggerService logger, IMapper mapper)
        {
            _carRepository = carRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var location = GetControllerActionName();
            //try
            //{
                _logger.LogInfo($"{location}: Attempted call");
                var cars = await _carRepository.FindAll();
                var response = _mapper.Map<IList<CarDTO>>(cars);
                _logger.LogInfo($"{location} Successful");
                return Ok(response);
           // }
            //catch (Exception e)
            //{

            //    return StatusMessage($"{e.Message} - {e.InnerException} - ({location})");
            //}
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar(int id)
        {
            if (id < 0)
            {
                return NotFound();
            }


            var location = GetControllerActionName();
            try
            {
                _logger.LogInfo($"{location} Attempted call for id : {id}");
                var car = await _carRepository.FindById(id);
                if (car == null)
                {
                    _logger.LogWarn($"{location}, id is not exist");
                    return NotFound();
                }
                _logger.LogInfo($"{location} Attempted call for id:{id}");
                var response = _mapper.Map<CarDTO>(car);
                _logger.LogInfo($"Successfuly got record :{id}");
                return Ok(response);

            }
            catch (Exception e)
            {

                return StatusMessage($"{e.Message} - {e.InnerException}");
            }


        }
        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] CarDTO carDTO)
        {
            var location = GetControllerActionName();
            try
            {
                _logger.LogInfo($"{location} create attempted");

                if(carDTO == null)
                {
                    _logger.LogInfo("Trying to put an empty model");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Not all volues are filled");
                    return BadRequest(ModelState);
                }
                var car = _mapper.Map<Car>(carDTO);
                var isSuccess = await _carRepository.Create(car);
                if (!isSuccess)
                {
                    return NotFound();
                }
                _logger.LogInfo($"{location} created was successfull");
                _logger.LogInfo($"{location} : {car}");

                return Created("Create", new { car });

            }
            catch (Exception e)
            {
                return StatusMessage($"{location}: {e.Message} - {e.InnerException}");
            }
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCar([FromBody] CarDTO carDTO, int id)
        {
            var location = GetControllerActionName();
            try
            {
                _logger.LogInfo($"{location} : Update attempted with id {id}");
                if(carDTO == null || id < 1 || id != carDTO.Id)
                {
                    _logger.LogError("Bad request ");
                    return BadRequest();
                }
               
                var isExist = await _carRepository.IsExists(id);
                if(!isExist)
                {
                    _logger.LogInfo($"{location} - couldn't retrieve item");
                    return NotFound();
                }
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updateCar = _mapper.Map<Car>(carDTO);
                var isSuccess = await _carRepository.Update(updateCar);
                if(!isSuccess)
                {
                    return StatusMessage($"{location} - Update is failed");
                }
                _logger.LogInfo("Update was succssful");
                return NoContent();
            }

            catch (Exception e)
            {
                return StatusMessage($"{e.Message} - {e.InnerException}");
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var location = GetControllerActionName();
            if(id < 1)
            {
                _logger.LogInfo("Id is negative");
                return BadRequest();
            }
            _logger.LogInfo($"{location} is attempted");
            var car = await _carRepository.FindById(id);
            if(car == null)
            {
                return NotFound();
            }
            var isSuccess = await _carRepository.Delete(car);
            if(!isSuccess)
            {
                return StatusMessage($"{location} - attempt is failed");
            }
            return Ok();
        }


        private string GetControllerActionName()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;
            return $"{controller} - {action}";
        }
        private ObjectResult StatusMessage(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "something went wrong");
        }

    }
}
