using HousingLocation.Dto;
using ServiceStatusResult;

namespace HousingLocation.Service.IService
{
    public interface IImageService
    {
        Task<ServiceResultBase<List<PhotoDto>>> GetAllImageAsync();
        Task<ServiceResultBase<PhotoDto>> GetByIdImageAsync(int id);
        Task<ServiceResultBase<int>> InsertImageAsync(IFormFile imageDto);
        Task<ServiceResultBase<string>> DeleteImageAsync(int id);
        Task<ServiceResultBase<byte[]>> GetImageAsStreamAsync(int id);
    }
}
