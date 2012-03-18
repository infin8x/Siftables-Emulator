using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;
using Siftables.Sifteo;

namespace Siftables
{
    public class ImageManager
    {

        public Dictionary<string, CubeImage> ImageNameToImageInfo { get; private set; }

        public ImageManager()
        {
            ImageNameToImageInfo = new Dictionary<string, CubeImage>();
        }

        public ImageManager(string imagePath)
        {
            ImageNameToImageInfo = new Dictionary<string, CubeImage>();
            LoadImages(imagePath);
        }

        public void LoadImages(string imagePath)
        {
            var directoryInfo = new DirectoryInfo(imagePath);
            var imageList = directoryInfo.EnumerateFiles("*.png");
            foreach (var file in imageList)
            {
                var image = new CubeImage(file);
                ImageNameToImageInfo.Add(file.Name, image);
            }
        }

        public ImageInfo GetImageInfo(string imageName)
        {
            return ImageNameToImageInfo[imageName].ImageInfo;
        }

        public BitmapImage GetBitmapImage(string imageName)
        {
            return ImageNameToImageInfo[imageName].BitmapImage;
        }

        public ImageSet GetImageSet()
        {
            var images = ImageNameToImageInfo.Values;
            var imageInfosCollection = new Collection<ImageInfo>();
            foreach (var image in images)
            {
                imageInfosCollection.Add(image.ImageInfo);
            }
            var imageSet = new ImageSet(imageInfosCollection);
            return imageSet;
        }
    }
}
