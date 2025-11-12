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
using ForumWebsite.DTO.Thread;

namespace ForumWebsite.Controllers.Activity
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeModelsController : ControllerBase
    {
        private readonly MyDBContextApplication _context;

        public LikeModelsController(MyDBContextApplication context)
        {
            _context = context;
        }

        // GET: api/LikeModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDTO>>> GetLikes()
        {
            return await _context.Likes
             .Select(s => new LikeDTO
             {
                 ID = s.ID,
                 UserID = s.UserID,
                 ThreadID = s.ThreadID,
                 CreatedAt = s.CreatedAt

             }).ToListAsync();
        }

        // GET: api/LikeModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LikeModel>> GetLikeModel(int id)
        {
            var likeModel = await _context.Likes.FindAsync(id);

            if (likeModel == null)
            {
                return NotFound();
            }

            return likeModel;
        }


        // POST: api/LikeModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LikeDTO>> PostLikeModel(LikeDTO dto)
        {
          if(dto == null)
            {
                return BadRequest("No data in dto");
            }

          if(dto.UserID == null)
            {
                return BadRequest("No user ID data in dto");
            }

            if (dto.ThreadID == null)
            {
                return BadRequest("No Thread ID data in dto");
            }

            var like = new LikeModel
            {
                UserID = dto.UserID,
                ThreadID = dto.ThreadID,
                CreatedAt = DateTime.UtcNow,
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
            dto.ID = like.ID;
            return CreatedAtAction(nameof(GetLikeModel), new
            {
                id = dto.ID,

            }, dto);
        }

        // DELETE: api/LikeModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLikeModel(int id)
        {
            var likeModel = await _context.Likes.FindAsync(id);
            if (likeModel == null)
            {
                return NotFound();
            }

            _context.Likes.Remove(likeModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LikeModelExists(int id)
        {
            return _context.Likes.Any(e => e.ID == id);
        }
    }
}
