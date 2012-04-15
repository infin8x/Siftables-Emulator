using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Sifteo;

namespace Siftables
{
    public class ImageSources
    {

        public Dictionary<string, BitmapImage> ImageSource { get; private set; }
        private static readonly string[] ValidImageExtensions = new[] {".png", ".PNG", ".gif", ".GIF"};

        public ImageSources(string imagePath)
        {
            ImageSource = new Dictionary<string, BitmapImage>();
            LoadImages(imagePath);
        }

        public void LoadImages(string imagePath)
        {
            var directoryInfo = new DirectoryInfo(imagePath);
            try
            {
                var imageList = directoryInfo.EnumerateFiles("*").Where(file => ValidImageExtensions.Contains(file.Extension));
                foreach (var file in imageList)
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(new MemoryStream(File.ReadAllBytes(file.FullName)));
                    ImageSource.Add(file.Name, bitmapImage);
                }
            }
            catch (DirectoryNotFoundException)
            {
            }
        }

        public ImageInfo GetImageInfo(string imageName)
        {
            return new ImageInfo
                       {
                           name = imageName,
                           height = ImageSource[imageName].PixelHeight,
                           width = ImageSource[imageName].PixelWidth
                       };
        }

        public BitmapImage GetBitmapImage(string imageName)
        {
            return ImageSource[imageName];
        }

        public ImageSet GetImageSet()
        {
            var imageNames = ImageSource.Keys;
            var imageInfosCollection = new Collection<ImageInfo>();
            foreach (var imageName in imageNames)
            {
                imageInfosCollection.Add(GetImageInfo(imageName));
            }
            var imageSet = new ImageSet(imageInfosCollection);
            return imageSet;
        }

        public bool Contains(string imageName)
        {
            if (imageName == null)
            {
                return false;
            }
            return ImageSource.ContainsKey(imageName);
        }
    }
}
