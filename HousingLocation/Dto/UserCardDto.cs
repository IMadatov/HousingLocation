using HousingLocation.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingLocation.Dto
{
    public class UserCardDto
    {
        public int Id { get; set; }

        public bool IsNew { get; set; }

        public int HouseId { get; set; }

        public int BuyerUserId { get; set; }

        public bool IsAccepted { get; set; }

    }
}
