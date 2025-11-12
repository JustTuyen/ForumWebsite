using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForumWebsite.Data;
using ForumWebsite.Models.Activity;
using ForumWebsite.DTO.Activity;

namespace ForumWebsite.Controllers.Activity
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportModelsController : ControllerBase
    {
        private readonly MyDBContextApplication _context;

        public ReportModelsController(MyDBContextApplication context)
        {
            _context = context;
        }

        // GET: api/ReportModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportDTO>>> GetReportModels()
        {
            return await _context.ReportModels
            .Select(s => new ReportDTO
            {
                ID = s.ID,
                UserID = s.UserID,
                ThreadID = s.ThreadID,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                Reason = s.Reason,
                StatusID = s.StatusID

            }).ToListAsync();
        }

        // GET: api/ReportModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReportModel>> GetReportModel(int id)
        {
            var reportModel = await _context.ReportModels.FindAsync(id);

            if (reportModel == null)
            {
                return NotFound();
            }

            return reportModel;
        }

        // PUT: api/ReportModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReportModel(int id, UpdateReportDTO dto)
        {
           if(id == null)
            {
                return BadRequest("Missing id");
            }

            if (dto == null)
            {
                return BadRequest("Missing dto");
            }

            var report = await _context.ReportModels.FindAsync(id);
            if(report == null)
            {
                return NotFound("No report with this id was found");
            }

            report.Reason = dto.Reason;
            report.StatusID = dto.StatusID;
            report.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/ReportModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReportModel>> PostReportModel(CreateReportDTO dto)
        {
            if(dto.UserID == null)
            {
                return BadRequest("No user ID");
            }

            if (dto.ThreadID == null)
            {
                return BadRequest("No thread ID");
            }
            if (dto.StatusID == null)
            {
                return BadRequest("No status ID");
            }
            var report = new ReportModel
            {
                ThreadID = dto.ThreadID,
                UserID = dto.UserID,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Reason = dto.Reason,
                StatusID = dto.StatusID
            };

            _context.ReportModels.Add(report);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReportModel), new { id = report.ID }, report);
        }

        // DELETE: api/ReportModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReportModel(int id)
        {
            var reportModel = await _context.ReportModels.FindAsync(id);
            if (reportModel == null)
            {
                return NotFound();
            }

            _context.ReportModels.Remove(reportModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReportModelExists(int id)
        {
            return _context.ReportModels.Any(e => e.ID == id);
        }
    }
}
