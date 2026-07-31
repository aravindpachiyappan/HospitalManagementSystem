using Hospital_ManagementSystem_Api.DBContext;
using Hospital_ManagementSystem_Api.DTOs.Departments;
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

    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("departments-list")]
        public async Task<ActionResult<List<GetAllListDepartmentsResponseDTO>>> DepartmentsList(GetAllListDepartmentsRequestListDTO requestDTO)
        {
            var query = _context.Departments
                .Where(x => x.IsActive && !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(requestDTO.SearchString))
            {
                query = query.Where(x => x.DepartmentName.Contains(requestDTO.SearchString));
            }

            var departments = await query
                .Select(x => new GetAllListDepartmentsResponseDTO
                {
                    DepartmentId = x.DepartmentId,
                    DepartmentName = x.DepartmentName,
                    Description = x.Description
                })
                .OrderBy(x => x.DepartmentId)
                .ToListAsync();

            return Ok(departments);
        }

        //[HttpPost("departments-list")]
        //public async Task<ActionResult<PaginatedDepartmentListResponseDTO>> DepartmentsList(GetAllListDepartmentsRequestListDTO requestDTO)
        //{
        //    var query = _context.Departments
        //        .Where(x => x.IsActive && !x.IsDeleted);

        //    if (!string.IsNullOrWhiteSpace(requestDTO.SearchString))
        //    {
        //        query = query.Where(x => x.DepartmentName.Contains(requestDTO.SearchString));
        //    }

        //    var totalRecords = await query.CountAsync();

        //    var departments = await query
        //        .OrderBy(x => x.DepartmentId)
        //        .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
        //        .Take(requestDTO.PageSize)
        //        .Select(x => new GetAllListDepartmentsResponseDTO
        //        {
        //            DepartmentId = x.DepartmentId,
        //            DepartmentName = x.DepartmentName,
        //            Description = x.Description
        //        })
        //        .ToListAsync();

        //    var response = new PaginatedDepartmentListResponseDTO
        //    {
        //        Departments = departments,
        //        TotalRecords = totalRecords,
        //        PageNumber = requestDTO.PageNumber,
        //        PageSize = requestDTO.PageSize,
        //        TotalPages = (int)Math.Ceiling((double)totalRecords / requestDTO.PageSize)
        //    };

        //    return Ok(response);
        //}

        [HttpPost("create-departments")]
        public async Task<ActionResult<CreateDepartmentsResponseDTO>> AddDepartments(CreateDepartmentsRequestDTO requestDTO)
        {
            var existingDepartment = await _context.Departments
                .FirstOrDefaultAsync(x => x.DepartmentName == requestDTO.DepartmentName
                                       && x.IsDeleted == false);

            if (existingDepartment != null)
            {
                throw new Exception("Department already exists.");
            }

            var department = new Department
            {
                DepartmentName = requestDTO.DepartmentName,
                Description = requestDTO.Description,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            var responseDTO = new CreateDepartmentsResponseDTO
            {
                DepartmentName = department.DepartmentName,
                Description = department.Description
            };

            return Ok(new
            {
                Message = "Department Created Successfully.",
                Data = responseDTO
            });
        }

        [HttpPost("update-departments")]
        public async Task<ActionResult<UpdateDepartmentsResponseDTO>> UpdateDepartments(UpdateDepartmentsRequestDTO requestDTO)
        {
            // Check if department exists
            var existingDepartment = await _context.Departments
                .FirstOrDefaultAsync(x => x.DepartmentId == requestDTO.DepartmentId
                                       && x.IsActive
                                       && !x.IsDeleted);

            if (existingDepartment == null)
            {
                throw new Exception("Department not found.");
            }

            // Check duplicate department name
            var duplicateDepartment = await _context.Departments
                .FirstOrDefaultAsync(x => x.DepartmentName == requestDTO.DepartmentName
                                       && x.DepartmentId != requestDTO.DepartmentId
                                       && !x.IsDeleted);

            if (duplicateDepartment != null)
            {
                throw new Exception("Department name already exists.");
            }

            // Update
            existingDepartment.DepartmentName = requestDTO.DepartmentName;
            existingDepartment.Description = requestDTO.Description;
            existingDepartment.UpdatedAt = DateTime.UtcNow;
            existingDepartment.UpdatedBy = 1;

            await _context.SaveChangesAsync();

            var responseDTO = new UpdateDepartmentsResponseDTO
            {
                DepartmentId = existingDepartment.DepartmentId,
                DepartmentName = existingDepartment.DepartmentName,
                Description = existingDepartment.Description
            };

            return Ok(new
            {
                Message = "Department Updated Successfully.",
                Data = responseDTO
            });
        }

        [HttpPost("delete-departments")]
        public async Task<ActionResult<DeleteDepartmentsResponseDTO>> DeleteDepartments(DeleteDepartmentsRequestDTO requestDTO)
        {
            var existingDepartment = await _context.Departments
                .FirstOrDefaultAsync(x => x.DepartmentId == requestDTO.DepartmentId
                                       && x.IsActive
                                       && !x.IsDeleted);

            if (existingDepartment == null)
            {
                throw new Exception("Department not found.");
            }

            // Soft Delete
            existingDepartment.IsActive = false;
            existingDepartment.IsDeleted = true;
            existingDepartment.UpdatedAt = DateTime.UtcNow;
            existingDepartment.UpdatedBy = 1;

            await _context.SaveChangesAsync();

            var responseDTO = new DeleteDepartmentsResponseDTO
            {
                DepartmentId = existingDepartment.DepartmentId,
                DepartmentName = existingDepartment.DepartmentName
            };

            return Ok(new
            {
                Message = "Department Deleted Successfully.",
                Data = responseDTO
            });
        }
    }
}
