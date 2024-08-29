using System.ComponentModel.DataAnnotations.Schema;

namespace HousingLocation.Models
{
    public class House
    {
        public int HouseId { get; set; }

        public string HouseName { get; set; } = null!;

        public string City { get; set; } = null!;

        public string State { get; set; } = null!;

        public int AvailableUnits { get; set; }

        public bool Wifi { get; set; }

        public bool Loundry { get; set; }

        [ForeignKey(nameof(CreatedUser))]
        public int CreatedUserId { get; set; }

        public User? CreatedUser { get; set; }

        [ForeignKey(nameof(Photo))]
        public int? PhotoId { get; set; }

        public Photo? Photo { get; set; }
        /*
         id: 0,
    name: 'Acme Fresh Start Housing',
    city: 'Chicago',
    state: 'IL',
    photo: '../assets/bernard-hermant-CLKGGwIBTaY-unsplash.jpg',
    availableUnits: 4,
    wifi: true,
    laundry: true,
         */
    }
}
