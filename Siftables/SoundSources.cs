using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sifteo;

namespace Siftables
{
    public class SoundSources
    {

        public Dictionary<string, string> SoundSource { get; private set; }
        private static readonly string[] ValidSoundExtensions = new[] { ".mp3", ".MP3" };
        private readonly SoundSet _soundSet;

        public delegate void NewSoundHandler(Sound sound);
        public event NewSoundHandler NotifyNewSound = delegate { };

        public SoundSources(string soundPath)
        {
            SoundSource = new Dictionary<string, string>();
            LoadSounds(soundPath);
            _soundSet = new SoundSet(SoundSource.Keys);
            _soundSet.NotifyNewSound += sound => NotifyNewSound(sound);
        }

        public void LoadSounds(string soundPath)
        {
            var directoryInfo = new DirectoryInfo(soundPath);
            try
            {
                var imageList = directoryInfo.EnumerateFiles("*").Where(file => ValidSoundExtensions.Contains(file.Extension));
                foreach (var file in imageList)
                {
                    SoundSource.Add(file.Name, file.FullName);
                }
            }
            catch (DirectoryNotFoundException)
            {
            }
        }

        public string GetSoundPath(string soundName)
        {
            return SoundSource[soundName];
        }

        public SoundSet GetSoundSet()
        {
            return _soundSet;
        }
    }
}
