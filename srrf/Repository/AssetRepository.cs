using srrf.Data;
using srrf.Interface;
using srrf.Models;

namespace srrf.Repository
{
    public class AssetRepository : IAsset
    {
        private readonly SrrfContext _context;
        public AssetRepository(SrrfContext context)
        {
            _context = context;
        }

        public bool AssetExists(int assetId)
        {
            return _context.Assets.Any(a => a.AssetId == assetId);
        }

        public bool CreateAsset(Asset asset)
        {
            _context.Add(asset);
            return Save();
        }

        public bool DeleteAsset(Asset asset)
        {
            _context.Remove(asset);
            return Save();
        }

        public Asset GetAsset(int assetId)
        {
            return _context.Assets.Where(a => a.AssetId == assetId).FirstOrDefault();
        }
        public ICollection<Asset> GetAssets()
        {
            return _context.Assets.OrderBy(a => a.AssetId).ToList();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateAsset(Asset asset)
        {
            _context.Update(asset);
            return Save();
        }
    }
}
