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
    public class CategoryModelsController : ControllerBase
    {
        private readonly MyDBContextApplication _context;

        public CategoryModelsController(MyDBContextApplication context)
        {
            _context = context;
        }

        // GET: api/CategoryModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            return await _context.Categories
             .Select(s => new CategoryDTO
             {
                 ID = s.ID,
                 Name = s.Name,
                 Description = s.Description
             }).ToListAsync();
        }

        // GET: api/CategoryModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryModel(int id)
        {
            var categoryModel = await _context.Categories.FindAsync(id);

            if (categoryModel == null)
            {
                return NotFound();
            }

            return new CategoryDTO
            {
                Name = categoryModel.Name,
                Description = categoryModel.Description,
            };
        }

        // PUT: api/CategoryModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryModel(int id, CategoryDTO dto)
        {
            if (id != dto.ID)
            {
                return BadRequest();
            }

            var category = await _context.Categories.FindAsync(id);
            if(category == null)
            {
                return NotFound();
            }

            category.Name = dto.Name;
            category.Description = dto.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/CategoryModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> PostCategoryModel(CategoryDTO dto)
        {
           var category = new CategoryModel { 
               Name = dto.Name,
               Description = dto.Description
           };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            dto.ID = category.ID;
            return CreatedAtAction(nameof(GetCategories), new
            {
                id = dto.ID,
            }, dto);
        }

        // DELETE: api/CategoryModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryModel(int id)
        {
            var categoryModel = await _context.Categories.FindAsync(id);
            if (categoryModel == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(categoryModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryModelExists(int id)
        {
            return _context.Categories.Any(e => e.ID == id);
        }
    }
}
