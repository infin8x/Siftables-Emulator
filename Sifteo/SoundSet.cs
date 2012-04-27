using System;
using System.Collections.Generic;

namespace Sifteo
{
    public class SoundSet
    {
        private readonly ICollection<string> _soundNames;
        public SoundSet(ICollection<string> soundNames)
        {
            _soundNames = soundNames;
        }

        public delegate void NewSoundHandler(Sound sound);
        public event NewSoundHandler NotifyNewSound = delegate { };
        public Sound CreateSound(string soundName)
        {
            if (_soundNames.Contains(soundName))
            {
                var newSound = new Sound(soundName);
                NotifyNewSound(newSound);
                return newSound;
            }

            throw new ArgumentException("No Sound object with name " + soundName);
        }
    }
}
