using HousingLocation.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingLocation.Dto
{
    public class UserCardDto
    {
        public int Id { get; set; }

        public int HouseId { get; set; }

        public int BuyerUserId { get; set; }

    }
}
