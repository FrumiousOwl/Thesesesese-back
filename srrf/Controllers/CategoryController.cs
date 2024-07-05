using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
            public CategoryController(ICategory category, IMapper mapper)
            {
                _category = category;
                _mapper = mapper;
            }

            [HttpGet]
            [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
            public IActionResult GetServiceRequests()
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
            [HttpGet("{categoryName}")]
            [ProducesResponseType(200, Type = typeof(Category))]
            [ProducesResponseType(404)] // Updated response type for not found
            public IActionResult GetCategoryByName(string categoryName)
            {
                if (string.IsNullOrEmpty(categoryName))
                {
                    return BadRequest("Service requester name is required."); // Informative message
                }

                var category = _mapper.Map<CategoryDto>(_category.GetCategoryByName(categoryName));

                if (category == null) // Check if serviceRequest is null after retrieval
                {
                    return NotFound("Service request not found for the provided name."); // Informative message
                }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                return Ok(category);
            }
        }
}