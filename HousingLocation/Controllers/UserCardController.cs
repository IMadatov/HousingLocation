﻿using HousingLocation.Dto;
using HousingLocation.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStatusResult;
using System.Security.Claims;

namespace HousingLocation.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    [Authorize]
    public class UserCardController(IUserCardService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<UserCardDto>>> GetAllUserCards(){
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return await FromServiceResultBaseAsync(_service.GetAllUserCardsAsync(int.Parse(userId)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserCardDto>> GetUserCard(int id) =>
            await FromServiceResultBaseAsync(_service.GetUserCard(id));

        [HttpPost("{id:int}")]
        public async Task<ActionResult<UserCardDto>> PostUserCard(int id)
        {
            var userId=HttpContext.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier)?.Value;
            return await FromServiceResultBaseAsync(_service.InsertUserCard(id, int.Parse(userId)));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteUserCard(int id) =>
            await FromServiceResultBaseAsync(_service.DeleteUserCard(id));

        [HttpPut]
        public async Task<ActionResult<bool>> AddToCard(int id)
        {
            var userId=HttpContext.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier)?.Value;

            return await FromServiceResultBaseAsync(_service.AddToCard(id, int.Parse(userId)));
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
