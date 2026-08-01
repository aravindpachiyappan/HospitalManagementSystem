namespace Hospital_ManagementSystem_Api.DTOs.RoomDTO
{
    public class UpdateRoomRequestDTO
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public int FloorNumber { get; set; }
        public int BedCount { get; set; }
        public decimal PricePerDay { get; set; }
    }
}
