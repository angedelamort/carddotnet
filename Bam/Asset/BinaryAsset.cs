using System.IO;

namespace Bam.Asset
{
    public class BinaryAsset : IAsset
    {
        private byte[] data;

        public BinaryAsset()
        {
        }

        public BinaryAsset(byte[] data)
        {
            this.data = data;
        }

        public void Serialize(Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(data.Length);
                stream.Write(data, 0, data.Length);
            }
        }

        public void Deserialize(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                var length = reader.ReadInt32();
                data = reader.ReadBytes(length);
            }
        }

        public byte[] GetBytes()
        {
            return data;
        }
    }
}
