using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sifteo
{
    public class ImageSet
    {
        private readonly Collection<ImageInfo> _imageInfos;

        public ImageSet(Collection<ImageInfo> imageInfos)
        {
            _imageInfos = imageInfos;
        }

        public bool Contains(string name)
        {
            return _imageInfos.Any(imageInfo => imageInfo.name.Equals(name));
        }

        public ImageInfo InfoFor(string name)
        {
            foreach (var imageInfo in _imageInfos)
            {
                if (imageInfo.name.Equals(name))
                {
                    return imageInfo;
                }
            }

            throw new IndexOutOfRangeException("No ImageInfo object with name " + name);
        }

        public ImageInfo this[string name]
        {
            get { return InfoFor(name); }
        }
    }
}
