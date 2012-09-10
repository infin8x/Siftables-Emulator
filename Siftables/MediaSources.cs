using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace Siftables
{
    public abstract class MediaSources
    {
        protected IEnumerable<FileInfo> LoadMediaFiles(string mediaPath)
        {
            var directoryInfo = new DirectoryInfo(mediaPath);
            try
            {
                return directoryInfo.EnumerateFiles("*").Where(file => IsValidExtension(file.Extension));
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Failed to load media from directory: " + mediaPath);
            }
            return new Collection<FileInfo>();
        }

        protected abstract bool IsValidExtension(string extension);

    }
}
