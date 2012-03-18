using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Siftables.Sifteo;

namespace Siftables
{
    public class CubeImage
    {
        public ImageInfo ImageInfo { get; private set; }
        public BitmapImage BitmapImage { get; private set; }

        public CubeImage(FileSystemInfo fileInfo)
        {
            var imageUri = new Uri(fileInfo.FullName);
            BitmapImage = new BitmapImage();
            BitmapImage.SetSource(new MemoryStream(File.ReadAllBytes(fileInfo.FullName)));
            BitmapImage.ImageFailed += (sender, args) => MessageBox.Show("Load failed");
            ImageInfo = new ImageInfo
                            {height = BitmapImage.PixelHeight, width = BitmapImage.PixelWidth, name = fileInfo.Name};
        }
    }
}
