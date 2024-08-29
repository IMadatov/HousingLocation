using HousingLocation.Dto;
using ServiceStatusResult;

namespace HousingLocation.Service.IService
{
    public interface IHousingService
    {
        Task<ServiceResultBase<List<HouseDto>>> GetAllHousesAsync();
        Task<ServiceResultBase<HouseDto>> GetByIdHouseAsync(int id);
        Task<ServiceResultBase<HouseDto>> InsertHouseAsync(HouseDto houseDto);
        Task<ServiceResultBase<HouseDto>> UpdateHouseAsync(HouseDto houseDto);
        Task<ServiceResultBase<string>> DeleteHouseAsync(int id,int idUser);
        Task<ServiceResultBase<List<HouseDto>>> MyCards(int id);
    }
}
