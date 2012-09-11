using System.Collections.Generic;
using System.IO;

namespace Siftables
{
    public interface IMediaSources
    {
        IEnumerable<FileInfo> LoadMediaFiles(string mediaPath);

        bool IsValidExtension(string extension);

        bool Contains(string mediaName);

        object GetMediaSet();

        object this[string mediaName] { get; }
    }
}
