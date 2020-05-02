using System.IO;

namespace Bam.Asset
{
    public interface IAsset
    {
        void Serialize(Stream stream);
        void Deserialize(Stream stream);

        byte[] GetBytes();

        // TODO: should we have a freeMemory?? or even implement IDispose?
    }
}
