using Hospital_ManagementSystem_Api.DBContext;
using Hospital_ManagementSystem_Api.DTOs.UsersDTO;
using Hospital_ManagementSystem_Api.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_ManagementSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser(AddUsersRequestDTO requestDTO)
        {
            // 1. Check whether username or email already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.UserName == requestDTO.UserName ||
                    x.Email == requestDTO.Email);

            if (existingUser != null)
            {
                return BadRequest(new
                {
                    Message = "Username or Email already exists"
                });
            }

            // 2. Create new user
            var newUser = new User
            {
                UserName = requestDTO.UserName,
                Email = requestDTO.Email,
                Password = requestDTO.Password,
                CreatedBy = 1,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            _context.Users.Add(newUser);

            // Save user first to get generated UserId
            await _context.SaveChangesAsync();

            // 3. Get default role
            var defaultRole = await _context.Roles
                .FirstOrDefaultAsync(x => x.RoleName == "Patient");

            if (defaultRole == null)
            {
                return BadRequest(new
                {
                    Message = "Default role 'Patient' not found"
                });
            }

            // 4. Assign default role to user
            var userRole = new UserRole
            {
                UserId = newUser.UserId,
                RoleId = defaultRole.RoleId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1,
                IsActive= true,
                IsDeleted= false
            };

            _context.userRoles.Add(userRole);

            await _context.SaveChangesAsync();

            // 5. Return response
            return Ok(new
            {
                Message = "Created Successfully"
            });
        }

        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser(UpdateUserRequestDTO requestDTO)
        {
            // 1. Find User
            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.UserId == requestDTO.UserId &&
                    !x.IsDeleted);

            if (user == null)
            {
                return NotFound(new UpdateUserResponseDTO
                {
                    UserId = requestDTO.UserId,
                    Message = "User not found"
                });
            }

            // 2. Check duplicate username
            var existingUserName = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.UserName == requestDTO.UserName &&
                    x.UserId != requestDTO.UserId &&
                    !x.IsDeleted);

            if (existingUserName != null)
            {
                return BadRequest(new UpdateUserResponseDTO
                {
                    UserId = requestDTO.UserId,
                    Message = "Username already exists"
                });
            }

            // 3. Check duplicate email
            var existingEmail = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email == requestDTO.Email &&
                    x.UserId != requestDTO.UserId &&
                    !x.IsDeleted);

            if (existingEmail != null)
            {
                return BadRequest(new UpdateUserResponseDTO
                {
                    UserId = requestDTO.UserId,
                    Message = "Email already exists"
                });
            }

            // 4. Check Role exists
            var role = await _context.Roles
                .FirstOrDefaultAsync(x =>
                    x.RoleId == requestDTO.RoleId &&
                    !x.IsDeleted);

            if (role == null)
            {
                return BadRequest(new UpdateUserResponseDTO
                {
                    UserId = requestDTO.UserId,
                    Message = "Role not found"
                });
            }

            // 5. Update User
            user.UserName = requestDTO.UserName;
            user.Email = requestDTO.Email;
            user.Password = requestDTO.Password;

            // 6. Find existing UserRole
            var userRole = await _context.userRoles
                .FirstOrDefaultAsync(x =>
                    x.UserId == requestDTO.UserId &&
                    !x.IsDeleted);

            if (userRole == null)
            {
                // If UserRole doesn't exist, create it
                userRole = new UserRole
                {
                    UserId = user.UserId,
                    RoleId = requestDTO.RoleId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    IsActive = true,
                    IsDeleted = false
                };

                _context.userRoles.Add(userRole);
            }
            else
            {
                // Update existing role
                userRole.RoleId = requestDTO.RoleId;
            }

            // 7. Save
            await _context.SaveChangesAsync();

            // 8. Response
            var response = new UpdateUserResponseDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Message = "User and role updated successfully"
            };

            return Ok(response);
        }

        [HttpPost("user-list")]
        public async Task<ActionResult<List<UserLIstResponseDTO>>> UserList(GetAllUserRequestDTO requestDTO)
        {
            var query = _context.userRoles
                .Include(x => x.User)
                .Include(x => x.Role)
                .Where(x => x.IsActive && !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(requestDTO.SearchString))
            {
                string search = requestDTO.SearchString.Trim().ToLower();

                query = query.Where(x =>
                    (x.User != null && x.User.UserName != null && x.User.UserName.ToLower().Contains(search)) ||
                    (x.User != null && x.User.Email != null && x.User.Email.ToLower().Contains(search)) ||
                    (x.Role != null && x.Role.RoleName != null && x.Role.RoleName.ToLower().Contains(search))
                );
            }

            var result = await query
                .Select(x => new UserLIstResponseDTO
                {
                    UsertId = x.User!.UserId,
                    UserName = x.User.UserName!,
                    Email = x.User.Email!,
                    UserRoles = x.Role!.RoleName!
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
