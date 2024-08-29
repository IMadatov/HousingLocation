using HousingLocation.Dto;
using ServiceStatusResult;

namespace HousingLocation.Service.IService
{
    public interface IUserCardService
    {
        Task<ServiceResultBase<List<UserCardDto>>> GetAllUserCardsAsync(int id);
        Task<ServiceResultBase<UserCardDto>> GetUserCard(int id);
        Task<ServiceResultBase<bool>> InsertUserCard(int id,int userId);
        Task<ServiceResultBase<string>> DeleteUserCard(int id);
        Task<ServiceResultBase<bool>> AddToCard(int id, int userId);
    }
}
