using AutoMapper.Configuration.Annotations;
using HousingLocation.Dto;
using HousingLocation.Service.IService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStatusResult;
using System.Security.Claims;

namespace HousingLocation.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "User")]
    public class HomeController : ControllerBase
    {
        public IHousingService _service;

        public HomeController(IHousingService service)
        {
            _service = service;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HouseDto>>> GetAllHouses()
        {
            return await FromServiceResultBaseAsync(_service.GetAllHousesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HouseDto>> GetByIdHouse(int id)
            => await FromServiceResultBaseAsync(_service.GetByIdHouseAsync(id));

        [HttpPost]
        public async Task<ActionResult<HouseDto>> PostHouse(HouseDto houseDto)
        {
            var id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            houseDto.CreatedUserId = int.Parse(id!);
            return await FromServiceResultBaseAsync<HouseDto>(_service.InsertHouseAsync(houseDto));
        }

        [HttpPost]
        public async Task<ActionResult<List<HouseDto>>> PostHouses(List<HouseDto> houseDtos)
        {
            var result = new List<HouseDto>();

            foreach (var house in houseDtos)
            {
                var home = await _service.InsertHouseAsync(house);
                result.Add(home);
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<HouseDto>> UpdateHouse(HouseDto houseDto)
            => await FromServiceResultBaseAsync<HouseDto>(_service.UpdateHouseAsync(houseDto));

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHouse(int id)
        { 
            var idUser = HttpContext.User.Claims.FirstOrDefault(x=>x.Type== ClaimTypes.NameIdentifier)?.Value;
            return await FromServiceResultBaseAsync(_service.DeleteHouseAsync(id, int.Parse(idUser)));
        }

        [HttpGet]
        public async Task<ActionResult<List<HouseDto>>> GetMyHouses()
        {
            var id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return await FromServiceResultBaseAsync(_service.MyCards(int.Parse(id!)));
        }


        protected async Task<ActionResult> FromServiceResultBaseAsync<T>(Task<ServiceResultBase<T>> task)
        {
            var result = await task;
            if (result == null)
            {
                return NoContent();
            }
            var isOk = result.StatusCode < 400;
            if (isOk)
            {
                return StatusCode(result.StatusCode, result.Result);
            }
            return StatusCode(result.StatusCode, "Request failed");
        }
    }
}
