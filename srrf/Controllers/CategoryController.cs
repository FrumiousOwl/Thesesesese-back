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

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_category.CategoriesExists(categoryId))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_category.GetCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpGet("{assetId}/defective")]
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

        [HttpGet("{assetId}/deployed")]
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

        [HttpGet("{assetId}/available")]
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
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_category.CategoriesExists(categoryId))
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
