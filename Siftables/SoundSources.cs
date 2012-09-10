using System;
using System.Collections.Generic;
using System.Linq;
using Siftables.ViewModel;
using Sifteo;

namespace Siftables
{
    public class SoundSources : MediaSources
    {

        public Dictionary<string, string> SoundSource { get; private set; }
        private static readonly string[] ValidSoundExtensions = new[] { ".mp3" };
        public SoundSet SoundSet { get; private set; }

        public delegate void NewSoundHandler(Sound sound);
        public event NewSoundHandler NotifyNewSound = delegate { };

        public SoundSources(string soundPath, ICollection<SoundViewModel> activeSounds, ICollection<SoundViewModel> inactiveSounds)
        {
            SoundSource = new Dictionary<string, string>();
            LoadMedia(soundPath);
            SoundSet = new SoundSet(SoundSource.Keys);
            SoundSet.NotifyNewSound += sound => NotifyNewSound(sound);
            SetHandlers(activeSounds, inactiveSounds);
        }

        private void LoadMedia(string mediaPath)
        {
            foreach (var file in LoadMediaFiles(mediaPath))
            {
                SoundSource.Add(file.Name, file.FullName);
            }
        }

        public string GetSoundPath(string soundName)
        {
            return SoundSource[soundName];
        }

        private void InitializeSound(Sound sound, ICollection<SoundViewModel> activeSounds, ICollection<SoundViewModel> inactiveSounds)
        {
            var soundViewModel = new SoundViewModel(sound, GetSoundPath(sound.Name).Replace(@"\", @"/"));
            inactiveSounds.Add(soundViewModel);

            soundViewModel.NotifyPlay += () =>
            {
                inactiveSounds.Remove(soundViewModel);
                activeSounds.Add(soundViewModel);
            };
            soundViewModel.NotifyPause += () =>
            {
                activeSounds.Remove(soundViewModel);
                if (!inactiveSounds.Contains(soundViewModel))
                {
                    inactiveSounds.Add(soundViewModel);
                }
            };
            soundViewModel.NotifyResume += () =>
            {
                inactiveSounds.Remove(soundViewModel);
                if (!activeSounds.Contains(soundViewModel))
                {
                    activeSounds.Add(soundViewModel);
                }
            };
        }

        private static void StopAllSounds(ICollection<SoundViewModel> activeSounds, ICollection<SoundViewModel> inactiveSounds)
        {
            foreach (var sound in activeSounds)
            {
                sound.Position = TimeSpan.Zero;
                inactiveSounds.Add(sound);
            }
            activeSounds.Clear();
        }

        internal static void ResumeAllSounds(ICollection<SoundViewModel> activeSounds, ICollection<SoundViewModel> inactiveSounds)
        {
            foreach (var sound in inactiveSounds)
            {
                activeSounds.Add(sound);
                sound.RestoreResumeSpot();
            }
            inactiveSounds.Clear();
        }

        internal static void PauseAllSounds(ICollection<SoundViewModel> activeSounds, ICollection<SoundViewModel> inactiveSounds)
        {
            foreach (var sound in activeSounds)
            {
                sound.SetResumeSpot();
                inactiveSounds.Add(sound);
            }
            activeSounds.Clear();
        }

        protected override bool IsValidExtension(string extension)
        {
            return ValidSoundExtensions.Contains(extension);
        }

        private void SetHandlers(ICollection<SoundViewModel> activeSounds, ICollection<SoundViewModel> inactiveSounds)
        {
            NotifyNewSound += sound => InitializeSound(sound, activeSounds, inactiveSounds);
            Sound.NotifyPauseAllSounds += () => PauseAllSounds(activeSounds, inactiveSounds);
            Sound.NotifyResumeAllSounds += () => ResumeAllSounds(activeSounds, inactiveSounds);
            Sound.NotifyStopAllSounds += () => StopAllSounds(activeSounds, inactiveSounds);
        }
    }
}
