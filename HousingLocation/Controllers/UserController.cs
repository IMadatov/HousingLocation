using HousingLocation.Dto;
using HousingLocation.Models;
using HousingLocation.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStatusResult;
using System.Security.Claims;

namespace HousingLocation.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize()]
    public class UserController(IUserService _service) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()=>
            await FromServiceResultBaseAsync(_service.GetAllUsers());
        
        [HttpGet("{id}")]
        [AllowAnonymous()]
        public async Task<ActionResult<UserDto>> GetUser(int id)=>
            await FromServiceResultBaseAsync(_service.GetUser(id));
        
        [HttpPut]
        public async Task<ActionResult<bool>> UpdateUser(UserDto userDto)=>
            await FromServiceResultBaseAsync(_service.UpdateUser(userDto));
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteUser(int id)=>
            await FromServiceResultBaseAsync(_service.DeleteUser(id));

        [HttpPut]
        public async Task<ActionResult<bool>> ChangePassword(ChangePassword changePassword)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier)?.Value;

            return await FromServiceResultBaseAsync(_service.ChangePassword(changePassword,int.Parse(userId!)));
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
