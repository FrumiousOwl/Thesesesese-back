using srrf.Models;

namespace srrf.Interface
{
    public interface IAsset
    {
        ICollection<Asset> GetAssets();
        Asset GetAsset(int assetId);
        bool AssetExists(int assetId);
        bool CreateAsset(Asset asset);
        bool DeleteAsset(Asset asset);
        bool UpdateAsset(Asset asset);
        bool Save();
    }
}
