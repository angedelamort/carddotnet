using System;
using Bam.Asset;
using Bam.AssetRepository;

namespace Bam
{
    // todo: needs a dictionary of assets.
    // todo: might need to put it async
    // todo: create a proper cache for resources. For now they are loaded every time.
    public class AssetManager
    {
        private readonly IAssetRepository assetRepository;

        /// <summary>
        /// Create / Open a location with all the resources.
        ///
        /// Supported schemes: {dir, bundle}
        /// </summary>
        /// <param name="uri">Using the scheme, will instantiate the proper asset manager.</param>
        public AssetManager(Uri uri)
        {
            switch (uri.Scheme)
            {
                case "dir":
                    assetRepository = new DirectoryAssetRepository(uri.AbsolutePath);
                    break;

                case "bundle":
                    assetRepository = new BundleAssetRepository(uri.AbsolutePath);
                    break;

                default:
                    throw new NotSupportedException($"The scheme {uri.Scheme} is not supported.");
            }
        }

        public IAsset GetAsset(string key)
        {
            return assetRepository.Find(key);
        }

        public void AddAsset(string key, IAsset asset)
        {
            assetRepository.Add(key, asset);
        }

        public bool RemoveAsset(string key)
        {
            return assetRepository.Remove(key); ;
        }

        /// <summary>
        /// Convert the current resource manager to a single file based resource package.
        /// </summary>
        /// <param name="filename"></param>
        public void Bundle(string filename)
        {
            var bundle = new BundleAssetRepository(filename);
            assetRepository.CopyTo(bundle); // TODO: maybe pass some options...
        }
    }
}
