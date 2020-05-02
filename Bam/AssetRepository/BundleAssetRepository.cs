using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Bam.Asset;

namespace Bam.AssetRepository
{
    internal class BundleAssetRepository : IAssetRepository
    {
        private readonly string filename;
        private readonly Dictionary<string, long> offsetTable = new Dictionary<string, long>();
        private long tableOffset;

        private class BundleAssetIterator : IEnumerator<KeyValuePair<string, IAsset>>
        {
            private readonly BundleAssetRepository repository;
            private readonly List<string> keys;
            private int index = -1;

            public BundleAssetIterator(BundleAssetRepository repository)
            {
                this.repository = repository;
                keys = repository.offsetTable.Keys.ToList();
            }

            public void Dispose() { }
            public bool MoveNext() => keys.Count != ++index;
            public void Reset() => index = -1;
            public KeyValuePair<string, IAsset> Current => new KeyValuePair<string, IAsset>(keys[0], repository.Find(keys[0]));
            object IEnumerator.Current => Current;
        }

        public BundleAssetRepository(string filename)
        {
            this.filename = filename;

            if (File.Exists(filename))
            {
                using (var stream = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    using (var binaryReader = new BinaryReader(stream))
                    {
                        stream.Seek(sizeof(long), SeekOrigin.End);
                        tableOffset = binaryReader.ReadInt64();

                        stream.Seek(tableOffset, SeekOrigin.Begin);
                        var count = binaryReader.ReadInt32();
                        for (var i = 0; i < count; i++)
                            offsetTable.Add(binaryReader.ReadString(), binaryReader.ReadInt64());
                    }
                }
            }
        }

        // todo: have a version of bundle
        // todo: dictionary of resources with offset.
        public void Add(string key, IAsset asset)
        {
            offsetTable.Add(key, tableOffset);

            using (var stream = new FileStream(filename, FileMode.OpenOrCreate))
            {
                stream.Seek(tableOffset, SeekOrigin.Begin);
                using (var binaryWriter = new BinaryWriter(stream))
                {
                    binaryWriter.Write(0);
                }

                using (var gzipStream = new GZipStream(stream, CompressionMode.Compress))
                {
                    var data = asset.GetBytes();
                    gzipStream.Write(data, 0, data.Length);
                }

                // Update the length of the asset
                var newTableOffset = stream.Position;
                var assetSize = stream.Position - tableOffset;

                stream.Seek(tableOffset, SeekOrigin.Begin);
                using (var binaryWriter = new BinaryWriter(stream))
                {
                    binaryWriter.Write((int) assetSize);
                }

                tableOffset = newTableOffset;

                WriteTable(stream);
            }
        }

        public void Update(string key, IAsset asset)
        {
            throw new InvalidOperationException($"Cannot update for this type of bundle {nameof(BundleAssetRepository)}");
        }

        public bool Remove(string key)
        {
            throw new InvalidOperationException($"Cannot remove for this type of bundle {nameof(BundleAssetRepository)}");
        }

        public bool Exists(string key)
        {
            return offsetTable.ContainsKey(key);
        }

        public void CopyTo(IAssetRepository repository)
        {
            throw new NotImplementedException();
        }

        public IAsset Find(string key)
        {
            if (offsetTable.TryGetValue(key, out var offset))
            {
                using (var stream = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    int length;
                    using (var binaryReader = new BinaryReader(stream))
                    {
                        stream.Seek(offset, SeekOrigin.Begin);
                        length = binaryReader.ReadInt32();
                    }

                    using (var gzipReader = new GZipStream(stream, CompressionMode.Decompress, true))
                    {
                        var bytes = new byte[length];
                        gzipReader.Read(bytes, 0, bytes.Length);

                        return new BinaryAsset(bytes); // TODO: might not be a binary asset.
                    }
                }
            }

            return null;
        }

        public IEnumerator<KeyValuePair<string, IAsset>> GetEnumerator()
        {
            return new BundleAssetIterator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Always write the table at the end of the stream so we can erase it with every write.
        /// </summary>
        /// <param name="stream"></param>
        private void WriteTable(FileStream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                // TODO: could add a GZipStream later on.
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(offsetTable.Count);

                    foreach (var kvp in offsetTable)
                    {
                        binaryWriter.Write(kvp.Key);
                        binaryWriter.Write(kvp.Value);
                    }

                    binaryWriter.Write(tableOffset); // NOTE: never gzip that.
                }

                stream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Position);
            }
        }
    }
}
