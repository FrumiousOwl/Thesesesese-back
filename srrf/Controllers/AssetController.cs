using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using srrf.Data;
using srrf.Dto;
using srrf.Interface;
using srrf.Models;
using System.Collections.Generic;

namespace srrf.Controllers
{
    [ApiController]
    [Route("api[controller]")]
    public class AssetController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAsset _asset;
        private readonly SrrfContext _context;

        public AssetController(IMapper mapper, IAsset asset, SrrfContext context)
        {
            _mapper = mapper;
            _asset = asset;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Asset>))]
        public IActionResult GetAssets()
        {
            var asset = _mapper.Map<List<AssetDto>>(_asset.GetAssets());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(asset);
        }

        [HttpGet("{assetId}")]
        [ProducesResponseType(200, Type = typeof(Asset))]
        [ProducesResponseType(400)]
        public IActionResult GetAsset(int assetId)
        {
            if (!_asset.AssetExists(assetId))
                return NotFound();

            var asset = _mapper.Map<AssetDto>(_asset.GetAsset(assetId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(asset);
        }

        [HttpGet("{assetId}/defective")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAssetDefectiveStatus(int assetDefectiveId)
        {
            var asset = await _context.Assets.FindAsync(assetDefectiveId);

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
            var asset = await _context.Assets.FindAsync(assetDeployedId);

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
            var asset = await _context.Assets.FindAsync(assetAvailableId);

            if (asset == null)
            {
                return NotFound();
            }

            var assetStatusDto = new AvailableAssetDto
            {
                Name = asset.Name,
                Available = asset.Defective,
                Deployed = asset.Deployed
            };

            return Ok(assetStatusDto);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateAsseet([FromBody] AssetDto asset)
        {
            if (asset == null)
                return BadRequest(ModelState);

            var assets = _asset.GetAssets()
                .Where(a => a.Name.Trim().ToUpper() == asset.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (assets != null)
            {
                ModelState.AddModelError("", "Asset already exists");
                return StatusCode(422, ModelState);
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var assetMap = _mapper.Map<Asset>(asset);

            if (!_asset.CreateAsset(assetMap))
            {
                ModelState.AddModelError("", "Asset already exists");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Succefully");
        }

        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAsset(int assetId, [FromBody] AssetDto updateAsset)
        {
            if (updateAsset == null)
                return BadRequest(ModelState);

            if (assetId != updateAsset.AssetId)
                return BadRequest(ModelState);

            if (!_asset.AssetExists(assetId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var assetMap = _mapper.Map<Asset>(updateAsset);

            if (!_asset.UpdateAsset(assetMap))
            {
                ModelState.AddModelError("", "Something went wrong updating asset");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{assetId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int assetId)
        {
            if (!_asset.AssetExists(assetId))
            {
                return NotFound();
            }

            var asssetToDelete = _asset.GetAsset(assetId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_asset.DeleteAsset(asssetToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting asset");
            }

            return NoContent();
        }
    }
}
