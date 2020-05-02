using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bam.Asset;

namespace Bam.AssetRepository
{
    // in file asset, need to save the file at the root.
    internal class DirectoryAssetRepository : IAssetRepository
    {
        private class DirectoryAssetIterator : IEnumerator<KeyValuePair<string, IAsset>>
        {
            private readonly IEnumerable<string> fileEnumerator;

            public DirectoryAssetIterator(string path) => fileEnumerator = Directory.EnumerateFiles(path, "*.dat");

            public void Dispose() => fileEnumerator.GetEnumerator().Dispose();
            public bool MoveNext() => fileEnumerator.GetEnumerator().MoveNext();
            public void Reset() => fileEnumerator.GetEnumerator().Reset();
            public KeyValuePair<string, IAsset> Current => new KeyValuePair<string, IAsset>(Key, CreateAsset(fileEnumerator.GetEnumerator().Current));
            object IEnumerator.Current => Current;

            private string Key => Path.GetFileNameWithoutExtension(fileEnumerator.GetEnumerator().Current);
        }

        private readonly string dataPath;

        public DirectoryAssetRepository(string absolutePath)
        {
            dataPath = absolutePath;
        }

        public void Add(string key, IAsset asset)
        {
            var path = GetFilename(key);
            if (File.Exists(path))
                throw new InvalidOperationException($"The file {key} already exists.");

            using (var fileStream = File.Create(path))
            {
                asset.Serialize(fileStream);
            }
        }

        public void Update(string key, IAsset asset)
        {
            var path = GetFilename(key);
            if (File.Exists(path))
                File.Delete(path);

            Add(key, asset);
        }

        public bool Remove(string key)
        {
            var path = GetFilename(key);
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }

        public bool Exists(string key) => File.Exists(GetFilename(key));

        public void CopyTo(IAssetRepository repository)
        {
            if (this == repository)
                return;

            // TODO: enable parallelism
            //Parallel.ForEach(this, (key, asset) =>
            //{
            //    repository.Add(key, asset);
            //});

            foreach (var kvp in this)
                repository.Add(kvp.Key, kvp.Value);
        }

        public IAsset Find(string key)
        {
            var path = GetFilename(key);
            return File.Exists(path) ? CreateAsset(path) : null;
        }

        private static IAsset CreateAsset(string path)
        {
            var binaryAsset = new BinaryAsset();
            binaryAsset.Deserialize(File.OpenRead(path));
            return binaryAsset;
        }

        private string GetFilename(string key) => Path.Combine(dataPath, key + ".dat");

        public IEnumerator<KeyValuePair<string, IAsset>> GetEnumerator()
        {
            return new DirectoryAssetIterator(dataPath);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
