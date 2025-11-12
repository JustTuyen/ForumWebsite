using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForumWebsite.Data;
using ForumWebsite.Models.Reply;
using ForumWebsite.DTO.Reply;
using ForumWebsite.Services.CloudinaryHelper;
using System.Threading;
using ForumWebsite.DTO.Thread;

namespace ForumWebsite.Controllers.Reply
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplyModelsController : ControllerBase
    {
        private readonly MyDBContextApplication _context;

        public ReplyModelsController(MyDBContextApplication context)
        {
            _context = context;
        }   
        // DELETE: api/ReplyModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReplyModel(int id)
        {
            var replyModel = await _context.Replies.FindAsync(id);
            if (replyModel == null)
            {
                return NotFound();
            }

            _context.Replies.Remove(replyModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReplyModelExists(int id)
        {
            return _context.Replies.Any(e => e.ID == id);
        }

        [HttpPut("LikeAndStatus/{id}")]
        public async Task<ActionResult> UpdateForLikeAndReport(int id,
            [FromBody] LikeAndStatusReplyDTO dto)
        {           
            if (dto == null)
            {
                return BadRequest("No data to updated");
            }

            var reply = await _context.Replies.FindAsync(id);
            if(reply == null)
            {
                return NotFound("No reply with the id was found");
            }

            reply.LikeCount = reply.LikeCount + 1;
            if(dto.StatusID.HasValue)
            {
                reply.StatusID = dto.StatusID.Value;
                reply.UpdatedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Reply like count and/or status updated successfully." });
        }

        [HttpPost]
        public async Task<ActionResult<CreateReplyDTO>> creatReply(
            [FromForm] CreateReplyDTO dto,
            IFormFile? imageFile,
            [FromServices] CloudinaryService cloudinaryService)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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

            var thread = await _context.Threads
                .Include(t => t.Replies)
                .FirstOrDefaultAsync(t => t.ID == dto.ThreadID);

            if (thread == null)
            {
                return BadRequest("Missing UserID");
            }
            if( thread.StatusID == 7)
            {
                return StatusCode(403, new { error = "Thread is archived. No new replies allowed." });
            }

            var replyCount = thread.Replies?.Count ?? 0;
            if(replyCount >= thread.ReplyLimit)
            {
                return StatusCode(403, new { error = "The reply in this thread is Maxed. No new replies allowed." });
            }

            if (dto.ParentReplyID.HasValue)
            {
                var parent = await _context.Replies.FindAsync(dto.ParentReplyID.Value);
                if(parent == null || parent.ThreadID != dto.ThreadID)
                {
                    return BadRequest("Parent with this id was not found");
                }
            }

            //image handle
            int? imageID = null;
            string? imageUrl = null;

            if (imageFile != null)
            {
                var uploadResult = await cloudinaryService.UploadImageAsync(imageFile);
                if (uploadResult != null)
                {
                    var image = new ImageModel
                    {
                        URL = uploadResult.SecureUrl?.AbsoluteUri,
                        PublicID = uploadResult.PublicId,
                        CreatedAt = DateTime.UtcNow,
                    };

                    _context.Images.Add(image);
                    await _context.SaveChangesAsync();
                    imageID = image.ID;
                    imageUrl = image.URL;
                }
            }

            var reply = new ReplyModel
            {
                Name = dto.Name,
                Content = dto.Content,
                LikeCount = dto.LikeCount,
                UserID = user.ID,
                ThreadID = thread.ID,
                ImageID = imageID,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ParentReplyID = dto.ParentReplyID,
                StatusID = dto.StatusID,
            };
            _context.Replies.Add(reply);
            await _context.SaveChangesAsync();

            //updatingthread
            thread.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            
            if (imageID.HasValue)
            {
                var image = await _context.Images.FindAsync(imageID.Value);
                if (image != null)
                {
                    image.ReplyID = reply.ID;
                    await _context.SaveChangesAsync();
                }
            }

            return CreatedAtAction(nameof(getReplyContent), new
            {
                id = reply.ID,
            }, new
            {
                reply.ID,
                reply.Name,
                reply.ImageID,
                ImageUrl = imageUrl
            });
        }

        //get thread
        [HttpGet("{id}")]
        public async Task<ActionResult<DisplayReplyDTO>> getReplyContent(int id)
        {
            var reply = await _context.Replies
                .Include( c => c.Image)
                .Include(r => r.ParentReply)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.ID == id);

            if(reply == null)
            {
                return NotFound("No reply with this id was found");
            }

            var dto = new DisplayReplyDTO { 
                ID = reply.ID,
                Name = reply.Name,
                Content = reply.Content,
                LikeCount = reply.LikeCount,
                CreatedAt = reply.CreatedAt,
                UpdatedAt = reply.UpdatedAt,
                ParentReplyID = reply.ParentReplyID,
                ParentReplyName = reply.ParentReply != null ? reply.ParentReply.Name : null,
                StatusID = reply.StatusID,
                ImageUrl = reply.Image != null ? reply.Image.URL : null,
            };

            return Ok(dto);
        }

    }
}
