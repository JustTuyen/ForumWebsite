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
    public class BookmarkModelsController : ControllerBase
    {
        private readonly MyDBContextApplication _context;

        public BookmarkModelsController(MyDBContextApplication context)
        {
            _context = context;
        }

        // GET: api/BookmarkModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookmarkDTO>>> GetBookmarks()
        {
            return await _context.Bookmarks
            .Select(s => new BookmarkDTO
            {
                ID = s.ID,
                UserID = s.UserID,
                ThreadID = s.ThreadID,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                Remark = s.Remark,
                
            }).ToListAsync();
        }

        // GET: api/BookmarkModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookmarkModel>> GetBookmarkModel(int id)
        {
            var bookmarkModel = await _context.Bookmarks.FindAsync(id);

            if (bookmarkModel == null)
            {
                return NotFound();
            }

            return bookmarkModel;
        }

        // PUT: api/BookmarkModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookmarkModel(int id, BookmarkDTO dto)
        {
            if (id == null)
            {
                return BadRequest("Missing ID");
            }

            if (dto == null)
            {
                return BadRequest("Missing dto data");
            }

            var bookmark = await _context.Bookmarks.FindAsync(id);
            if (bookmark == null)
            {
                return NotFound("no bookmark with this id was found");
            }

            bookmark.Remark = dto.Remark;
            bookmark.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();            
            return NoContent();
        }

        // POST: api/BookmarkModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookmarkDTO>> PostBookmarkModel(BookmarkDTO dto)
        {
            var bookmark = new BookmarkModel
            {
                ThreadID = dto.ThreadID,
                UserID = dto.UserID,
                CreatedAt = DateTime.UtcNow,
                Remark = dto.Remark,
                UpdatedAt = DateTime.UtcNow,
            };
            _context.Bookmarks.Add(bookmark);
            await _context.SaveChangesAsync();
            dto.ID = bookmark.ID;
            return CreatedAtAction(nameof(GetBookmarkModel), new
            {
                id = dto.ID,
            }, dto);
        }

        // DELETE: api/BookmarkModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookmarkModel(int id)
        {
            var bookmarkModel = await _context.Bookmarks.FindAsync(id);
            if (bookmarkModel == null)
            {
                return NotFound();
            }

            _context.Bookmarks.Remove(bookmarkModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookmarkModelExists(int id)
        {
            return _context.Bookmarks.Any(e => e.ID == id);
        }
    }
}
