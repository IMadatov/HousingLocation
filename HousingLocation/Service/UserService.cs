using AutoMapper;
using HousingLocation.Contexts;
using HousingLocation.Dto;
using HousingLocation.Models;
using HousingLocation.Service.IService;
using Microsoft.EntityFrameworkCore;
using ServiceStatusResult;

namespace HousingLocation.Service;

public class UserService(HousingLocationContext _context, IMapper _mapper) : IUserService
{
    private readonly string _papper = "Admin";
    private readonly int _iteration = 2;

    public async Task<ServiceResultBase<string>> DeleteUser(int id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id);

        if (user == null)
            return new NotFoundServiceResult<string>("it doesn't has in DB");

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        return new OkServiceResult<string>("Deleted user");

    }

    public async Task<ServiceResultBase<List<UserDto>>> GetAllUsers()
    {
        var users = await _context.Users.Select(x => new User
        {
            UserId = x.UserId,
            UserName = x.UserName,
            UserLastName = x.UserLastName,
            Email = x.Email,
            Password = "password"
        }).ToListAsync();

        var usersDto = _mapper.Map<List<UserDto>>(users);

        return new OkServiceResult<List<UserDto>>(usersDto);

    }

    public async Task<ServiceResultBase<UserDto>> GetUser(int id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id);

        if (user == null)
            return new NotFoundServiceResult<UserDto>();
        user.Password = "not access";
        return new OkServiceResult<UserDto>(_mapper.Map<UserDto>(user));

    }

    public async Task<ServiceResultBase<bool>> UpdateUser(UserDto userDto)
    {
        var hasDbUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userDto.UserId);

        if (hasDbUser == null)
            return new NotFoundServiceResult<bool>();

        var user = new User
        {
            UserId = userDto.UserId,
            Email = userDto.Email,
            UserLastName = userDto.UserLastName,
            UserName = userDto.UserName,
        };

        _context.Users.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return new OkServiceResult<bool>(_mapper.Map<bool>(user));
    }

    public async Task<ServiceResultBase<bool>> ChangePassword(ChangePassword changePassword, int userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

        if (user == null) return new NotFoundServiceResult<bool>();

        var hashNewPassword = PasswordHasher.ComputeHash(changePassword.OldPassword, _papper, _iteration);

        if (hashNewPassword != user.Password)
        {
            return new BadRequesServiceResult<bool>("Old password is incorrect");
        }

        user.Password = PasswordHasher.ComputeHash(changePassword.NewPassword,_papper,_iteration);

        _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return new OkServiceResult<bool>(true);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
            return new User();

        return user;
    }



    public async Task<ServiceResultBase<UserInfoDto>> GetUserInfoById(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);

        if (user == null)
            return new NotFoundServiceResult<UserInfoDto>();

        var userInfo = new UserInfoDto
        {
            UserId = user.UserId,
            Email = user.Email,
            UserName = user.UserName,
            UserLastName = user.UserLastName
        };
        return new OkServiceResult<UserInfoDto>(userInfo);
    }
}
