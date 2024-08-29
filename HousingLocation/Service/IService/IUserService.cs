using HousingLocation.Dto;
using HousingLocation.Models;
using ServiceStatusResult;

namespace HousingLocation.Service.IService
{
    public interface IUserService
    {
        Task<ServiceResultBase<List<UserDto>>> GetAllUsers();
        Task<ServiceResultBase<UserDto>> GetUser(int id);
        Task<ServiceResultBase<UserDto>> UpdateUser(UserDto userDto);
        Task<ServiceResultBase<string>> DeleteUser(int id);
        Task<User> GetUserByEmail(string email);
        Task<ServiceResultBase<UserInfoDto>> GetUserInfoById(int id);
    }
}
