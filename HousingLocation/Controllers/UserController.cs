using HousingLocation.Dto;
using HousingLocation.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStatusResult;

namespace HousingLocation.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize()]
    public class UserController(IUserService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()=>
            await FromServiceResultBaseAsync(_service.GetAllUsers());
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)=>
            await FromServiceResultBaseAsync(_service.GetUser(id));
        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateUser(UserDto userDto)=>
            await FromServiceResultBaseAsync(_service.UpdateUser(userDto));
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteUser(int id)=>
            await FromServiceResultBaseAsync(_service.DeleteUser(id));

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
