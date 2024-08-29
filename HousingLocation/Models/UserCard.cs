using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingLocation.Models
{
    public class UserCard
    {
        public int Id { get; set; } 

        [ForeignKey(nameof(House))]
        public int HouseId { get; set; }

        public virtual List<House>? Houses { get; set; }

        [ForeignKey(nameof(BuyerUser))]
        public int BuyerUserId { get; set; }

        public virtual User? BuyerUser { get; set; }
    }
}
