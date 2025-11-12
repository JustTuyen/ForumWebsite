using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForumWebsite.Data;
using ForumWebsite.Models.Thread;
using ForumWebsite.Services.CloudinaryHelper;
using ForumWebsite.DTO.Thread;
using ForumWebsite.DTO.Reply;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ForumWebsite.Models.Reply;

namespace ForumWebsite.Controllers.Thread
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadModelsController : ControllerBase
    {
        private readonly MyDBContextApplication _context;
        private readonly CloudinaryService _cloudinaryService;
        public ThreadModelsController(MyDBContextApplication context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThreadModel(int id)
        {
            var threadModel = await _context.Threads.FindAsync(id);
            if (threadModel == null)
            {
                return NotFound();
            }

            _context.Threads.Remove(threadModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThreadModelExists(int id)
        {
            return _context.Threads.Any(e => e.ID == id);
        }

        //get
        [HttpGet("ByViewCount")]
        public async Task<ActionResult<IEnumerable<ListingThreadDTO>>> GetThreadByViewCount()
        {
            var dto = await _context.Threads
           .Include(p => p.Image)
           .Include(p => p.Category)
           .AsNoTracking()
           .OrderByDescending(p => p.ViewCount)
           .Take(10)
           .Select(thread => new ListingThreadDTO
           {
               ID = thread.ID,
               Name = thread.Name,
               Title = thread.Title,
               CreatedAt = thread.CreatedAt,
               UpdatedAt = thread.UpdatedAt,
               ExpirationAt = thread.ExpirationAt,
               ViewCount = thread.ViewCount,
               LikeCount = thread.LikeCount,
               StatusID = thread.StatusID,
               CategoryID = thread.CategoryID,
               ImageID = thread.ImageID,
               ImageUrl = thread.Image != null? thread.Image.URL : null,
           })
           .ToListAsync();
            return Ok(dto);
        }

        //
        [HttpGet("ByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ListingThreadDTO>>> GetThreadsByCategory(int categoryId, [FromQuery] string sortBy = "updated")
        {
            //get list
            var query = _context.Threads
                .Include(p => p.Category)
                .Include(p => p.Image)
                .AsNoTracking()
                .Where(t => t.CategoryID == categoryId);

            //sorting
            query = sortBy.ToLower() switch
            {
                "views" => query.OrderByDescending(t => t.ViewCount),
                "likes" => query.OrderByDescending(t => t.LikeCount),
                "created" => query.OrderByDescending(t => t.CreatedAt),
                "updated" => query.OrderByDescending(t => t.UpdatedAt),
            };

            //display
            var dto = await query
           .Select(thread => new ListingThreadDTO
           {
               ID = thread.ID,
               Name = thread.Name,
               Title = thread.Title,
               CreatedAt = thread.CreatedAt,
               UpdatedAt = thread.UpdatedAt,
               ExpirationAt = thread.ExpirationAt,
               ViewCount = thread.ViewCount,
               LikeCount = thread.LikeCount,
               StatusID = thread.StatusID,
               CategoryID = thread.CategoryID,
               ImageID = thread.ImageID,
               ImageUrl = thread.Image != null ? thread.Image.URL : null,
           })
           .ToListAsync();

            //if no thread
            if (dto == null || dto.Count == 0)
            {
                return NotFound();
            }

            return Ok(dto);
        }


        //Display thread content
        [HttpGet("{id}")]
        public async Task<ActionResult<DisplayThreadDTO>> GetThreadContent(int id)
        {
            var thread = await _context.Threads
             .Include(t => t.Image)
             .Include( t => t.Category)
             .Include(t => t.Replies)
                .ThenInclude(t => t.Image)
             //.AsNoTracking()
             .FirstOrDefaultAsync(t => t.ID == id);

            if (thread == null)
            {
                return NotFound();
            }

            var dto = new DisplayThreadDTO
            {
                ID = thread.ID,
                Name = thread.Name,
                Title = thread.Title,
                Content = thread.Content,
                CreatedAt = thread.CreatedAt,
                UpdatedAt = thread.UpdatedAt,
                ExpirationAt = thread.ExpirationAt,
                ViewCount = thread.ViewCount,
                LikeCount = thread.LikeCount,
                StatusID = thread.StatusID,
                CategoryID = thread.CategoryID,
                CategoryName = thread.Category != null ? thread.Category.Name : null,
                ImageID = thread.ImageID,
                ImageUrl = thread.Image != null ? thread.Image.URL : null,
                Replies = thread.Replies.Select(r => new DisplayReplyDTO
                {
                    ID = r.ID,
                    Name = r.Name,
                    Content = r.Content,
                    LikeCount = r.LikeCount,
                    ImageID = r.ImageID,
                    ImageUrl = r.Image != null ? r.Image.URL : null,
                    ParentReplyID = r.ParentReplyID,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                    StatusID = r.StatusID,
                }).OrderByDescending(r => r.CreatedAt).ToList()

            };

            thread.ViewCount += 1;


            await _context.SaveChangesAsync();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CreateThreadDTO>> createThread(
            [FromForm]CreateThreadDTO dto,
            IFormFile? imageFile,
            [FromServices] CloudinaryService cloudinaryService)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _context.Categories.FindAsync(dto.CategoryID);
            if(category == null)
            {
                return BadRequest("Missing categoryID");
            }

            var status = await _context.Statuses.FindAsync(dto.StatusID);
            if (status == null)
            {
                return BadRequest("Missing statusID");
            }

            var user = await _context.Users.FindAsync(dto.UserID);
            if (user == null)
            {
                return BadRequest("Missing UserID");
            }

            //image handle
            int? imageID = null;
            string? imageUrl = null;

            if (imageFile!= null)
            {
                var uploadResult = await cloudinaryService.UploadImageAsync(imageFile);
                if (uploadResult != null)
                {
                    var image = new ImageModel
                    {
                        URL = uploadResult.SecureUri?.AbsoluteUri,
                        PublicID = uploadResult.PublicId,
                        CreatedAt = DateTime.UtcNow,
                    };

                    _context.Images.Add(image);
                    await _context.SaveChangesAsync();
                    imageID = image.ID;
                    imageUrl = image.URL;
                }
            }

            var thread = new ThreadModel
            {
                Name = dto.Name,
                Title = dto.Title,
                Content = dto.Content,
                LikeCount = dto.LikeCount,
                ViewCount = dto.ViewCount,
                ReplyLimit = dto.ReplyLimit,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ExpirationAt = DateTime.UtcNow.AddDays(20),
                UserID = user.ID,
                CategoryID = category.ID,
                StatusID = status.ID,
                ImageID = imageID,
            };

            _context.Threads.Add(thread);
            await _context.SaveChangesAsync();

            //
            if (imageID.HasValue)
            {
                var image = await _context.Images.FindAsync(imageID.Value);
                if (image != null)
                {
                    image.ThreadID = thread.ID;
                    await _context.SaveChangesAsync();
                }
            }

            return CreatedAtAction(nameof(GetThreadContent), new
            {
                id = thread.ID,
            },
            new
            {
                thread.ID,
                thread.Title,
                thread.ImageID,
                ImageUrl = imageUrl
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateThreadDTO>> updateContentThread(
            int id, [FromForm] UpdateThreadDTO dto,
            IFormFile? imageFile,
            [FromServices] CloudinaryService cloudinaryService)
        {
            if (dto == null)
            {
                return BadRequest("No data from dto");
            }

            if (id != dto.ID)
            {
                return BadRequest("No id");
            }

            var thread = await _context.Threads.FindAsync(id);
            if (thread != null)
            {
                return NotFound("No thread with this id was found");
            }

            //image handle
            if(imageFile != null)
            {
                if (thread.ImageID.HasValue)
                {
                    var oldImage = await _context.Images.FindAsync(thread.ImageID.Value);
                    if(oldImage != null)
                    {
                        await cloudinaryService.DeleteImageAsync(oldImage.PublicID);
                        _context.Images.Remove(oldImage);
                    }
                }

                var uploadResult = await cloudinaryService.UploadImageAsync(imageFile);
                if(uploadResult != null)
                {
                    var newImage = new ImageModel
                    {
                        URL = uploadResult.SecureUrl?.AbsoluteUri,
                        PublicID = uploadResult.PublicId,
                        CreatedAt = DateTime.UtcNow,
                    };

                    _context.Images.Add(newImage);
                    await _context.SaveChangesAsync();
                    thread.ImageID = newImage.ID;
                }
            }

            thread.Title = dto.Title;
            thread.Content = dto.Content;
            thread.Name = dto.Name;
            thread.CategoryID = dto.CategoryID;
            thread.UpdatedAt = DateTime.UtcNow;

            //thread.ImageID = imageID;

            await _context.SaveChangesAsync();

            return NotFound();
        }

        [HttpPut("LikeAndStatus/{id}")]
        public async Task<ActionResult<LikeAndStatusThreadDTO>> updatedLikeAndStatus(
            int id, [FromBody] LikeAndStatusThreadDTO dto)
        {
            if( dto  == null)
            {
                return BadRequest("No data in dto");
            }

            if(id == null)
            {
                return BadRequest("No id data");
            }

            var thread = await _context.Threads.FindAsync(id);
            if(thread == null)
            {
                return NotFound("No thread with this id was found");
            }

            thread.LikeCount = thread.LikeCount + 1 ;

            if( dto.StatusID.HasValue )
            {
                thread.StatusID = dto.StatusID.Value;
                thread.UpdatedAt = DateTime.UtcNow;
                //thread.ExpirationAt = DateTime.UtcNow.AddDays(20);
            }

            if (dto.ExpirationAt.HasValue)
            {
                thread.ExpirationAt = dto.ExpirationAt.Value;
                thread.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Thread updated successfully" });
        }

       
    }
}
