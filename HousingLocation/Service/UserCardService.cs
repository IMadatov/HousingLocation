using AutoMapper;
using HousingLocation.Contexts;
using HousingLocation.Dto;
using HousingLocation.Models;
using HousingLocation.Service.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.OpenApi.Validations;
using ServiceStatusResult;

namespace HousingLocation.Service
{
    public class UserCardService(HousingLocationContext _context, IMapper _mapper) : IUserCardService
    {


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
            var result = from card in _context.UserCards
                         join house in _context.Houses
                         on card.HouseId equals house.HouseId
                         where card.BuyerUserId == id
                         select card;

            var resultList = await result.ToListAsync();

            return new OkServiceResult<List<UserCardDto>>(_mapper.Map<List<UserCardDto>>(result));
        }



        public async Task<ServiceResultBase<UserCardDto>> GetUserCard(int id)
        {
            var result = await _context.UserCards.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                return new NotFoundServiceResult<UserCardDto>($"Not found this id:{id}");

            return new OkServiceResult<UserCardDto>(_mapper.Map<UserCardDto>(result));
        }

        public async Task<ServiceResultBase<bool>> InsertUserCard(int id, int userId)
        {
            if (id == 0)
                return new BadRequesServiceResult<bool>("id is 0");

            var house = await _context.Houses.FirstOrDefaultAsync(x => x.HouseId == id);
            if (house == null)
                return new BadRequesServiceResult<bool>("house not found");

            if (userId == 0)
                return new UnauthorizedServiceResult<bool>("u should be sign in");

            if (userId == house.CreatedUserId)
            {
                return new BadRequesServiceResult<bool>("This house is yours, u cannot add to card");
            }

            var checkHasDb = await _context.UserCards.Where(x => x.BuyerUserId == userId).ToListAsync();


            foreach (var item in checkHasDb)
            {
                if (item.HouseId == id)
                {
                    return new BadRequesServiceResult<bool>("This card has in DB");
                }
            }

            var userCard = new UserCard
            {
                IsNew = true,
                BuyerUserId = userId,
                HouseId = id
            };

            var result = _context.UserCards.Add(userCard);

            house.Status=Status.Offer;

            _context.Houses.Entry(house).State=EntityState.Modified;

            await _context.SaveChangesAsync();
            return new OkServiceResult<bool>(true);
        }
        

        public async Task<ServiceResultBase<List<UserCardDto>>> GetInCardsMyHouse(int userId)
        {
            if (userId == 0) return new UnauthorizedServiceResult<List<UserCardDto>>();

            var inCardMyHouse = from card in _context.UserCards
                                join house in _context.Houses
                                on card.HouseId equals house.HouseId
                                where house.CreatedUserId == userId
                                select card;

            var result = await inCardMyHouse.ToListAsync();
            return new OkServiceResult<List<UserCardDto>>(_mapper.Map<List<UserCardDto>>(result));
        }

        public async Task<ServiceResultBase<bool>> OldCard(List<UserCardDto> userCardDtos)
        {
            var userCards = _mapper.Map<List<UserCard>>(userCardDtos);

            userCards.ForEach(x =>
            {
                _context.Entry(x).State = EntityState.Modified;
            });

            await _context.SaveChangesAsync();
            return new OkServiceResult<bool>(true);
        }

        public async Task<ServiceResultBase<bool>> AcceptedCard(int cardId)
        {
            var card = await _context.UserCards.FirstOrDefaultAsync(x => x.Id == cardId);

            if (card == null)
            {
                return new NotFoundServiceResult<bool>();
            }
            card.IsAccepted = true;

            _context.Entry(card).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return new OkServiceResult<bool>(true);

        }
    }
}
