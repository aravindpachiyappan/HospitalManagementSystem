using Hospital_ManagementSystem_Api.DBContext;
using Hospital_ManagementSystem_Api.DTOs.RoomDTO;
using Hospital_ManagementSystem_Api.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_ManagementSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("room-list")]
        public async Task<ActionResult<List<GetAllRoomsListResponseDTO>>> RoomList(GetAllRoomsListRequestDTO requestDTO)
        {
            var query = _context.Rooms
               .Where(x => x.IsActive && !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(requestDTO.SearchString))
            {
                query = query.Where(x => x.RoomType.Contains(requestDTO.SearchString) 
                                      || x.RoomNumber.Contains(requestDTO.SearchString));
            }

            var rooms = await query.Select(x => new GetAllRoomsListResponseDTO
            {
                RoomId = x.RoomId,
                RoomNumber = x.RoomNumber,
                RoomType = x.RoomType,
                FloorNumber = x.FloorNumber,
                BedCount = x.BedCount,
                PricePerDay = x.PricePerDay
            }).OrderBy(x => x.RoomId)
              .ToListAsync();

            return Ok(rooms);
        }

        [HttpPost("add-room")]
        public async Task<ActionResult<CreateRoomsResponseDTO>> AddRoom(CreateRoomsRequestDTO requestDTO)
        {
            var existingDepartment = await _context.Rooms
               .FirstOrDefaultAsync(x => x.RoomNumber == requestDTO.RoomNumber
                                      && x.IsDeleted == false);

            if (existingDepartment != null)
            {
                throw new Exception("Department already exists.");
            }

            var room = new Rooms
            {
                RoomNumber = requestDTO.RoomNumber,
                RoomType = requestDTO.RoomType,
                FloorNumber = requestDTO.FloorNumber,
                BedCount = requestDTO.BedCount,
                PricePerDay = requestDTO.PricePerDay,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1
            };

            _context.Rooms.Add(room);

            await _context.SaveChangesAsync();

            var responseDTO = new CreateRoomsResponseDTO
            {
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                FloorNumber = room.FloorNumber,
                BedCount = room.BedCount,
                PricePerDay = room.PricePerDay,
            };

            return Ok(new
            {
                Message = "Room Created Successfully.",
                Data = responseDTO
            });
        }

        [HttpPost("update-room")]
        public async Task<ActionResult<UpdateRoomResponseDTO>> UpdateRoom(UpdateRoomRequestDTO requestDTO)
        {
            var existingRoom = await _context.Rooms
                .FirstOrDefaultAsync(x => x.RoomId == requestDTO.RoomId
                                       && x.IsDeleted == false);

            if (existingRoom == null)
            {
                throw new Exception("Room not found.");
            }

            var duplicateRoom = await _context.Rooms
                .FirstOrDefaultAsync(x => x.RoomNumber == requestDTO.RoomNumber
                                       && x.RoomId != requestDTO.RoomId
                                       && x.IsDeleted == false);

            if (duplicateRoom != null)
            {
                throw new Exception("Room Number already exists.");
            }

            existingRoom.RoomNumber = requestDTO.RoomNumber;
            existingRoom.RoomType = requestDTO.RoomType;
            existingRoom.FloorNumber = requestDTO.FloorNumber;
            existingRoom.BedCount = requestDTO.BedCount;
            existingRoom.PricePerDay = requestDTO.PricePerDay;
            existingRoom.UpdatedAt = DateTime.UtcNow;
            existingRoom.UpdatedBy = 1;

            await _context.SaveChangesAsync();

            var responseDTO = new UpdateRoomResponseDTO
            {
                RoomId = existingRoom.RoomId,
                RoomNumber = existingRoom.RoomNumber,
                RoomType = existingRoom.RoomType,
                FloorNumber = existingRoom.FloorNumber,
                BedCount = existingRoom.BedCount,
                PricePerDay = existingRoom.PricePerDay
            };

            return Ok(new
            {
                Message = "Room Updated Successfully.",
                Data = responseDTO
            });
        }

        [HttpPost("delete-room")]
        public async Task<IActionResult> DeleteRoom(DeleteRoomRequestDTO requestDTO)
        {
            var existingRoom = await _context.Rooms
                .FirstOrDefaultAsync(x => x.RoomId == requestDTO.RoomId
                                       && x.IsDeleted == false);

            if (existingRoom == null)
            {
                throw new Exception("Room not found.");
            }

            existingRoom.IsDeleted = true;
            existingRoom.IsActive = false;
            existingRoom.UpdatedAt = DateTime.UtcNow;
            existingRoom.UpdatedBy = 1;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Room Deleted Successfully."
            });
        }
    }
}
