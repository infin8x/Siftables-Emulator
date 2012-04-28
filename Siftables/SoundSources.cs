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
    public class SoundSources
    {
        public ICollection<SoundViewModel> InactiveSounds { get; private set; }
        public ObservableCollection<SoundViewModel> ActiveSounds { get; private set; }

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
            ActiveSounds = new ObservableCollection<SoundViewModel>();
            InactiveSounds = new Collection<SoundViewModel>();
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

        public void InitializeSound(Sound sound)
        {
            var soundViewModel = new SoundViewModel(sound, GetSoundPath(sound.Name).Replace(@"\", @"/"));
            InactiveSounds.Add(soundViewModel);
            soundViewModel.NotifyPlay += () =>
            {
                InactiveSounds.Remove(soundViewModel);
                ActiveSounds.Add(soundViewModel);
                MessageBox.Show(ActiveSounds.Count.ToString());
            };
            soundViewModel.NotifyPause += () =>
            {
                ActiveSounds.Remove(soundViewModel);
                if (!InactiveSounds.Contains(soundViewModel))
                {
                    InactiveSounds.Add(soundViewModel);
                }
            };
            soundViewModel.NotifyResume += () =>
            {
                InactiveSounds.Remove(soundViewModel);
                if (!ActiveSounds.Contains(soundViewModel))
                {
                    ActiveSounds.Add(soundViewModel);
                }
            };
        }

        public void StopAllSounds()
        {
            foreach (var sound in ActiveSounds)
            {
                sound.Position = TimeSpan.Zero;
                InactiveSounds.Add(sound);
            }
            ActiveSounds.Clear();
        }

        public void ResumeAllSounds()
        {
            foreach (var sound in InactiveSounds)
            {
                ActiveSounds.Add(sound);
                sound.RestoreResumeSpot();
            }
            InactiveSounds.Clear();
        }

        public void PauseAllSounds()
        {
            foreach (var sound in ActiveSounds)
            {
                sound.SetResumeSpot();
                InactiveSounds.Add(sound);
            }
            ActiveSounds.Clear();
        }
    }
}
