using HousingLocation.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingLocation.Dto
{
    public class HouseDto
    {
        public int HouseId { get; set; }

        public string HouseName { get; set; } = null!;

        public string City { get; set; } = null!;

        public string State { get; set; } = null!;

        public int AvailableUnits { get; set; }

        public bool Wifi { get; set; }

        public bool Loundry { get; set; }

        public int CreatedUserId { get; set; }

        public int? PhotoId { get; set; }
    }
}
