using System;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using Sifteo;

namespace Siftables.ViewModel
{
    public class SoundViewModel : ViewModelNotifier
    {
        private Uri _path;
        public Uri Path
        {
            get { return _path; }
            set
            {
                _path = value;
                NotifyPropertyChanged("Path");
            }
        }

        private readonly Sound _sound;
        private float _volumeLeft;
        public float VolumeLeft
        {
            get { return _volumeLeft; }
            set
            {
                _volumeLeft = value;
                NotifyPropertyChanged("VolumeLeft");
            }
        }
        private float _volumeRight;
        public float VolumeRight
        {
            get { return _volumeRight; }
            set
            {
                _volumeRight = value;
                NotifyPropertyChanged("VolumeRight");
            }
        }

        private bool _isPaused;
        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                _isPaused = value;
                NotifyPropertyChanged("IsPaused");
            }
        }

        private TimeSpan _position;
        public TimeSpan Position
        {
            get { return _position; }
            set
            {
                _position = value;
                NotifyPropertyChanged("Position");
            }
        }

        private TimeSpan _resumeSpot;

        public delegate void SoundEventHandler();
        public event SoundEventHandler NotifyPlay = delegate { };
        public event SoundEventHandler NotifyPause = delegate { };
        public event SoundEventHandler NotifyResume = delegate { };
        public SoundViewModel(Sound sound, string path)
        {
            _resumeSpot = TimeSpan.Zero;
            _sound = sound;
            Path = new Uri(path);
            IsPaused = false;
            _sound.NotifySetVolume += (volumeLeft, volumeRight) =>
                                          {
                                              VolumeLeft = volumeLeft;
                                              VolumeRight = volumeRight;
                                          };
            _sound.NotifyPlaySound += (left, right, loops) =>
                                          {
                                              VolumeLeft = left;
                                              VolumeRight = right;
                                              NotifyPlay();
                                          };
            _sound.NotifyPauseSound += () =>
                                           {
                                               IsPaused = true;
                                               NotifyPause();
                                               _resumeSpot = Position;
                                           };
            _sound.NotifyResumeSound += () =>
            {
                IsPaused = false;
                NotifyResume();
            };
            SetPosition = new ActionCommand(RestoreResumeSpot);
        }

        public ICommand SetPosition { get; set; }

        public void SetResumeSpot()
        {
            _resumeSpot = Position;
        }

        public void RestoreResumeSpot()
        {
            Position = _resumeSpot;
        }
    }
}
