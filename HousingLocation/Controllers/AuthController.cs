using HousingLocation.Models;
using HousingLocation.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceStatusResult;
using System.Security.Claims;
using HousingLocation.Contexts;
using HousingLocation.Dto;
using Microsoft.AspNetCore.Authorization;

namespace HousingLocation.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class AuthController(HousingLocationContext _context) : ControllerBase
    {
        private readonly string _papper = "Admin";
        private readonly int _iteration = 2;

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> Login(LoginModel loginModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == loginModel.Email);

            if (user == null || PasswordHasher.ComputeHash(loginModel.Password, _papper, _iteration) != user.Password)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            claims.Add(new Claim(ClaimTypes.GivenName, user.UserLastName));

            claims.Add(new Claim(ClaimTypes.Role, "User"));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true
                }
            );

            return Ok(true);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok(true);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> SignUp(UserDto userDto)
        {
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Email == userDto.Email);

            if (result != null)
            {
                return BadRequest("This email already has DB");
            }

            var user = new User
            {
                UserLastName = userDto.UserLastName,
                UserName = userDto.UserName,
                Email = userDto.Email,
                Password = PasswordHasher.ComputeHash(userDto.Password, _papper, _iteration)
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok(true);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<UserInfoDto>> GetUser()
        {
            var id = HttpContext.User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier)?.Value;

            if(id==null)
                return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(x=>x.UserId==int.Parse(id));

            var userInfoDto = new UserInfoDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                UserLastName = user.UserLastName
            };
            return Ok(userInfoDto);
        }
    }
}
