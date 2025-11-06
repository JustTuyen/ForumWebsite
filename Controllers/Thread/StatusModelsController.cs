using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForumWebsite.Data;
using ForumWebsite.Models.Thread;
using ForumWebsite.DTO.Thread;

namespace ForumWebsite.Controllers.Thread
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusModelsController : ControllerBase
    {
        private readonly MyDBContextApplication _context;

        public StatusModelsController(MyDBContextApplication context)
        {
            _context = context;
        }

        // GET: api/StatusModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusDTO>>> GetStatuses()
        {
            return await _context.Statuses
                .Select(s => new StatusDTO  
                {
                   ID = s.ID,
                   StatusName = s.StatusName,
                   About = s.About,
                   CreatedAt = s.CreatedAt,
                   UpdatedAt = s.UpdatedAt,
                }).ToListAsync();
        }

        // GET: api/StatusModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StatusDTO>> GetStatusModel(int id)
        {
            var statusModel = await _context.Statuses.FindAsync(id);

            if (statusModel == null)
            {
                return NotFound();
            }

            return new StatusDTO
            {
                StatusName= statusModel.StatusName,
                About = statusModel.About
            };
        }

        // PUT: api/StatusModels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatusModel(int id, StatusDTO dto)
        {
            if (id != dto.ID)
            {
                return BadRequest();
            }

            var statusModel = await _context.Statuses.FindAsync(id);
            if(statusModel == null)
            {
                return NotFound();
            }

            statusModel.StatusName = dto.StatusName;
            statusModel.About = dto.About;
            statusModel.CreatedAt = DateTime.UtcNow;
            statusModel.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/StatusModels
        [HttpPost]
        public async Task<ActionResult<StatusDTO>> PostStatusModel(StatusDTO dto)
        {
            var status = new StatusModel
            {
                StatusName = dto.StatusName,
                About = dto.About,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();

            dto.ID = status.ID;
            return CreatedAtAction(nameof(GetStatuses),
                new
                {
                    id = dto.ID,
                }, dto);
        }

        // DELETE: api/StatusModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatusModel(int id)
        {
            var statusModel = await _context.Statuses.FindAsync(id);
            if (statusModel == null)
            {
                return NotFound();
            }

            _context.Statuses.Remove(statusModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatusModelExists(int id)
        {
            return _context.Statuses.Any(e => e.ID == id);
        }
    }
}
