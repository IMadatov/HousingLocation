using AutoMapper;
using HousingLocation.Contexts;
using HousingLocation.Controllers;
using HousingLocation.Dto;
using HousingLocation.Migrations;
using HousingLocation.Models;
using HousingLocation.Service.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ServiceStatusResult;

namespace HousingLocation.Service
{
    public class HousingService : IHousingService
    {

        private readonly HousingLocationContext _context;
        private readonly IMapper _mapper;
        public HousingService(HousingLocationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<ServiceResultBase<List<HouseDto>>> GetAllHousesAsync()
        {
            var housesList = _context.Houses;
            var houreCard = _context.UserCards;

            var rightOuter = from house in housesList
                             join card in houreCard
                             on house.HouseId equals card.HouseId into houseUserCardGroup
                             from card in houseUserCardGroup.DefaultIfEmpty()
                             where house.HouseId != card.HouseId
                             select house;


            var list = await rightOuter.ToListAsync();
            if (list == null)
            {
                return new NotFoundServiceResult<List<HouseDto>>();
            }
            var listDto = _mapper.Map<List<HouseDto>>(list);
            return new OkServiceResult<List<HouseDto>>(listDto);
        }

        public async Task<ServiceResultBase<HouseDto>> GetByIdHouseAsync(int id)
        {
            var AHouse = await _context.Houses.FirstOrDefaultAsync(x => x.HouseId == id);
            if (AHouse == null)
                return new NotFoundServiceResult<HouseDto>();
            var AHouseDto = _mapper.Map<HouseDto>(AHouse);
            return new OkServiceResult<HouseDto>(AHouseDto);
        }

        public async Task<ServiceResultBase<HouseDto>> InsertHouseAsync(HouseDto houseDto)
        {
            if (houseDto.HouseId != 0 || houseDto == null)
            {
                return new BadRequesServiceResult<HouseDto>();
            }
            var house = _mapper.Map<House>(houseDto);



            _context.Houses.Add(house);

            await _context.SaveChangesAsync();

            return new OkServiceResult<HouseDto>(_mapper.Map<HouseDto>(house));
        }

        public async Task<ServiceResultBase<HouseDto>> UpdateHouseAsync(HouseDto houseDto)
        {
            if (houseDto.HouseId == 0)
            {
                return new BadRequesServiceResult<HouseDto>();
            }
            var houseFound = await _context.Houses.AsNoTracking().FirstOrDefaultAsync(x => x.HouseId == houseDto.HouseId);

            if (houseFound == null)
            {
                return new NotFoundServiceResult<HouseDto>();
            }

            var house = _mapper.Map<House>(houseDto);

            _context.Houses.Entry(house).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            houseDto = _mapper.Map<HouseDto>(house);

            return new OkServiceResult<HouseDto>(houseDto);
        }

        public async Task<ServiceResultBase<string>> DeleteHouseAsync(int id, int idUser)
        {

            var house = await _context.Houses.FirstOrDefaultAsync(x => x.HouseId == id);

            if (house == null)
                return new NotFoundServiceResult<string>();
            if (house.CreatedUserId != idUser) return new BadRequesServiceResult<string>();

            _context.Houses.Remove(house);

            await _context.SaveChangesAsync();

            return new NoContentServiceResult<string>($"Deleted this id {id}");
        }

        public async Task<ServiceResultBase<List<HouseDto>>> MyCards(int id)
        {
            var myCards = await _context.Houses.Where(x => x.CreatedUserId == id).ToListAsync();
            return new OkServiceResult<List<HouseDto>>(_mapper.Map<List<HouseDto>>(myCards));
        }
    }
}
