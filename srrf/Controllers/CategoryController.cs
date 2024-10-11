using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto;
using srrf.Interface;
using srrf.Models;

namespace srrf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _category;
        private readonly IMapper _mapper;
        private readonly SrrfContext _context;
        public CategoryController(ICategory category, IMapper mapper, SrrfContext context)
        {
            _category = category;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var category = _mapper.Map<List<CategoryDto>>(_category.GetCategories());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        /*       [HttpGet]
               public async Task<ActionResult<IEnumerable<Category>>> GetAssets(int page = 0, int pageSize = 0)
               {
                   var items = await _context.Categories
                       .Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .ToListAsync();


                   if (!ModelState.IsValid)
                       return BadRequest(ModelState);

                   return Ok(items);
               }*/

        [HttpGet("searchCategoryId/{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategoryById([FromQuery] int categoryId)
        {
                return NotFound();
            }

            var catId = await _context.Categories
            .Where(c => c.CategoryId == categoryId)
            .ToListAsync();

            var nameMap = _mapper.Map<List<CategoryDto>>(catId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(nameMap);
        }

        [HttpGet("defective/{assetId}")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAssetDefectiveStatus(int assetDefectiveId)
        {
            var asset = await _context.Categories.FindAsync(assetDefectiveId);

            if (asset == null)
            {
                return NotFound();
            }

            return Ok(asset.Defective);
        }

        [HttpGet("deployed/{assetId}")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAssetDeployedStatus(int assetDeployedId)
        {
            var asset = await _context.Categories.FindAsync(assetDeployedId);

            if (asset == null)
            {
                return NotFound();
            }

            return Ok(asset.Deployed);
        }

        [HttpGet("available/{assetId}")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAssetAvaialbleStatus(int assetAvailableId)
        {
            var asset = await _context.Categories.FindAsync(assetAvailableId);

            if (asset == null)
            {
                return NotFound();
            }

            var assetStatusDto = new AvailableAssetDto
            {
                AssetId = asset.CategoryId,
                Name = asset.Name,
                DatePurchased = asset.DatePurchased,
                Available = asset.Defective,
                Deployed = asset.Deployed
            };

            return Ok(assetStatusDto);
        }

        [HttpGet("searchCategory/{categoryName}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategoryByName([FromQuery] string name)
        {
            if (name == null)
            {
                return NotFound();
            }

            var namae = await _context.Categories
                .Where(c => c.Name.ToUpper().StartsWith(name.ToUpper()))
                .ToListAsync();

            var nameMap = _mapper.Map<List<CategoryDto>>(namae);

            return Ok(nameMap);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto category)
        {
            if (category == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(category);

            if (!_category.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest(ModelState);

            if (categoryId != updatedCategory.CategoryId)
                return BadRequest(ModelState);

            if (!_category.CategoriesExists(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var categoryMap = _mapper.Map<Category>(updatedCategory);

            if (!_category.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            if ( !_category.CategoriesExists(categoryId))
            {
                return NotFound();
            }

            var categoryToDelete = _category.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_category.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return NoContent();
        }
    }
}
