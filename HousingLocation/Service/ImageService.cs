using AutoMapper;
using HousingLocation.Contexts;
using HousingLocation.Dto;
using HousingLocation.Models;
using HousingLocation.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceStatusResult;
using System.Drawing;
using System.Drawing.Imaging;

namespace HousingLocation.Service
{
    public class ImageService : IImageService
    {
        private readonly HousingLocationContext _context;
        private readonly IMapper _mapper;
        public ImageService(HousingLocationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResultBase<string>> DeleteImageAsync(int id)
        {
            var image = await _context.Images.FirstOrDefaultAsync(x => x.PhotoId == id);
            if (image == null)
                return new NotFoundServiceResult<string>();
            
            var filePath = Path.Combine(@"C:\ImageToDoProject\", image.FileName);
            
            try{
                File.Delete(filePath);
                _context.Images.Remove(image);
            }catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            await _context.SaveChangesAsync();

            return new OkServiceResult<string>($"Deleted this id {id}");
        }

        public async Task<ServiceResultBase<List<PhotoDto>>> GetAllImageAsync()
        {
            var list = await _context.Images.ToListAsync();

            var dto= _mapper.Map<List<PhotoDto>>(list);
            return new OkServiceResult<List<PhotoDto>>(dto);
        }

        public async Task<ServiceResultBase<PhotoDto>> GetByIdImageAsync(int id)
        {
            var image = await _context.Images.FirstOrDefaultAsync(x => x.PhotoId == id);

            if (image == null)
                return new NotFoundServiceResult<PhotoDto>();
                
            return new OkServiceResult<PhotoDto>(_mapper.Map<PhotoDto>(image));
        }

        public async Task<ServiceResultBase<int>> InsertImageAsync(IFormFile file)
        {
            var fileName = file.FileName;
            var extension = Path.GetExtension(fileName);
            var guid = Guid.NewGuid();
            var newFileName = guid + extension;
            var photoInfo = new Photo
            {
                FileName = newFileName,
                OriginalName = fileName
            };
            try
            {

                _context.Images.Add(photoInfo);

                using var memoryStream = new MemoryStream();

                file.CopyTo(memoryStream);

                using Image image = Image.FromStream(memoryStream);

                var filePath = Path.Combine(@"C:\ImageToDoProject\", newFileName);

                image.Save(filePath, ImageFormat.Jpeg);

                await _context.SaveChangesAsync();
            }
            catch
            {

                return new ProblemServiceResult<int>();
            }



            return new OkServiceResult<int>(photoInfo.PhotoId);
        }

        public async Task<ServiceResultBase<byte[]>> GetImageAsStreamAsync(int id)
        {
            var photoName = await _context.Images.FirstOrDefaultAsync(x=>x.PhotoId==id);
            if (photoName == null)
            {
                return new NotFoundServiceResult<byte[]>();
            }
            var filePath = Path.Combine(@"C:\ImageToDoProject\", photoName.FileName);

            var fileAtBytes=await File.ReadAllBytesAsync(filePath);

            var fileBytes=await File.ReadAllBytesAsync(filePath);
            return new OkServiceResult<byte[]>(fileBytes);
        }

    }
}
