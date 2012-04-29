using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Siftables.ViewModel;
using Sifteo;

namespace Siftables
{
    public class SoundSources : ViewModelNotifier
    {

        public Dictionary<string, string> SoundSource { get; private set; }
        private static readonly string[] ValidSoundExtensions = new[] { ".mp3" };
        public SoundSet SoundSet { get; private set; }

        public delegate void NewSoundHandler(Sound sound);
        public event NewSoundHandler NotifyNewSound = delegate { };

        public SoundSources(string soundPath)
        {
            SoundSource = new Dictionary<string, string>();
            LoadSounds(soundPath);
            SoundSet = new SoundSet(SoundSource.Keys);
            SoundSet.NotifyNewSound += sound => NotifyNewSound(sound);
        }

        private void LoadSounds(string soundPath)
        {
            var directoryInfo = new DirectoryInfo(soundPath);
            try
            {
                var imageList = directoryInfo.EnumerateFiles("*").Where(file => ValidSoundExtensions.Contains(file.Extension.ToLower()));
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

        public void InitializeSound(Sound sound, ObservableCollection<SoundViewModel> activeSounds, Collection<SoundViewModel> inactiveSounds)
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

        public void StopAllSounds(ObservableCollection<SoundViewModel> activeSounds, Collection<SoundViewModel> inactiveSounds)
        {
            foreach (var sound in activeSounds)
            {
                sound.Position = TimeSpan.Zero;
                inactiveSounds.Add(sound);
            }
            activeSounds.Clear();
        }

        public void ResumeAllSounds(ObservableCollection<SoundViewModel> activeSounds, Collection<SoundViewModel> inactiveSounds)
        {
            foreach (var sound in inactiveSounds)
            {
                activeSounds.Add(sound);
                sound.RestoreResumeSpot();
            }
            inactiveSounds.Clear();
        }

        public void PauseAllSounds(ObservableCollection<SoundViewModel> activeSounds, Collection<SoundViewModel> inactiveSounds)
        {
            foreach (var sound in activeSounds)
            {
                sound.SetResumeSpot();
                inactiveSounds.Add(sound);
            }
            activeSounds.Clear();
        }
    }
}
