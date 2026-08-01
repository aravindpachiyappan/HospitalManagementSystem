using System.ComponentModel.DataAnnotations;

namespace Hospital_ManagementSystem_Api.Entity
{
    public class Rooms : BaseEntity
    {
        [Key]
        public int RoomId { get; set; }

        public string RoomNumber { get; set; } = string.Empty;

        public string RoomType { get; set; } = string.Empty;

        public int FloorNumber { get; set; }

        public int BedCount { get; set; }

        public decimal PricePerDay { get; set; }
    }
}
