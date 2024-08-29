using AutoMapper;
using HousingLocation.Contexts;
using HousingLocation.Dto;
using HousingLocation.Models;
using HousingLocation.Service.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.OpenApi.Validations;
using ServiceStatusResult;

namespace HousingLocation.Service
{
    public class UserCardService(HousingLocationContext _context, IMapper _mapper) : IUserCardService
    {
        public async Task<ServiceResultBase<bool>> AddToCard(int id, int userId)
        {
            var card = await _context.Houses.FirstOrDefaultAsync(x => x.HouseId == id);

            if (card == null)
            {
                return new NotFoundServiceResult<bool>();
            }

            _context.UserCards.Add(new UserCard
            {
                BuyerUserId = userId,
                HouseId = card.HouseId,
            });
            await _context.SaveChangesAsync();
            
            return new OkServiceResult<bool>(true);
        }

        public async Task<ServiceResultBase<string>> DeleteUserCard(int id)
        {
            var result = await _context.UserCards.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                return new NotFoundServiceResult<string>($"Not found this id:{id}");

            _context.UserCards.Remove(result);

            await _context.SaveChangesAsync();

            return new OkServiceResult<string>($"Deleted this id:{id}");
        }

        public async Task<ServiceResultBase<List<UserCardDto>>> GetAllUserCardsAsync(int id)
        {
            var result = _context.UserCards.AsNoTracking().ToList();

            return new OkServiceResult<List<UserCardDto>>(_mapper.Map<List<UserCardDto>>(result));
        }

        

        public async Task<ServiceResultBase<UserCardDto>> GetUserCard(int id)
        {
            var result = await _context.UserCards.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                return new NotFoundServiceResult<UserCardDto>($"Not found this id:{id}");

            return new OkServiceResult<UserCardDto>(_mapper.Map<UserCardDto>(result));
        }

        public async Task<ServiceResultBase<bool>> InsertUserCard(int id,int userId)
        {
            if (id == 0)
                return new BadRequesServiceResult<bool>("id is 0");

            var house = await _context.Houses.FirstOrDefaultAsync(x => x.HouseId == id);
            if (house == null)
                return new BadRequesServiceResult<bool>("house not found");

            if (userId == 0)
                return new UnauthorizedServiceResult<bool>("u should be sign in");

            var userCard = new UserCard
            {
                BuyerUserId = userId,
                HouseId = id
            };

            var result = _context.UserCards.Add(userCard);

            await _context.SaveChangesAsync();  
            return new OkServiceResult<bool>(true);
        }


    }
}
