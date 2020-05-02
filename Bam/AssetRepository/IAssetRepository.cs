using System.Collections.Generic;
using Bam.Asset;

namespace Bam.AssetRepository
{
    internal interface IAssetRepository: IEnumerable<KeyValuePair<string, IAsset>>
    {
        void Add(string key, IAsset asset);
        void Update(string key, IAsset asset);
        bool Remove(string key);
        bool Exists(string key);
        void CopyTo(IAssetRepository repository);
        IAsset Find(string key);
    }
}
