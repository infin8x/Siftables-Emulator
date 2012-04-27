namespace Sifteo
{
    public class Sound
    {
        #region Public Member Functions

        public void Play(float volume, int loops = 0)
        {
            Play(volume, volume, loops);
        }
        
        public delegate void PlaySoundHandler(float volumeLeft, float volumeRight, int loops);
        public event PlaySoundHandler NotifyPlaySound = delegate { };
        public void Play(float volumeLeft, float volumeRight, int loops=0)
        {
            NotifyPlaySound(volumeLeft, volumeRight, loops);
            IsPaused = false;
        }

        public delegate void StopSoundHandler();
        public event StopSoundHandler NotifyStopSound = delegate { };
        public void Stop()
        {
            NotifyStopSound();
            IsPaused = false;
        }

        public delegate void PauseSoundHandler();
        public event PauseSoundHandler NotifyPauseSound = delegate { };
        public void Pause()
        {
            NotifyPauseSound();
            IsPaused = true;
        }

        public delegate void ResumeSoundHandler();
        public event ResumeSoundHandler NotifyResumeSound = delegate { };
        public void Resume()
        {
            NotifyResumeSound();
            IsPaused = false;
        }

        public void SetVolume(float volume)
        {
            SetVolume(volume, volume);
        }

        public delegate void SetVolumeHandler(float volumeLeft, float volumeRight);
        public event SetVolumeHandler NotifySetVolume = delegate { };
        public void SetVolume(float volumeLeft, float volumeRight)
        {
            NotifySetVolume(volumeLeft, volumeRight);
        }

        #endregion

        #region Static Public Member Functions
        public delegate void EventHandler();

        public static event EventHandler NotifyPauseAllSounds = delegate { }; 
        public void PauseAll()
        {
            NotifyPauseAllSounds();
        }

        public static event EventHandler NotifyResumeAllSounds = delegate { }; 
        public static void ResumeAll()
        {
            NotifyResumeAllSounds();
        }

        public static event EventHandler NotifyStopAllSounds = delegate { }; 
        public static void StopAll()
        {
            NotifyStopAllSounds();
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public bool IsPlaying
        {
            get { return !IsPaused; }
        }

        public bool IsPaused { get; set; }

        #endregion

        #region Events

        public delegate void SoundStartedHandler(Sound sound);
        public SoundStartedHandler StartedEvent;
        public delegate void SoundStoppedHandler(Sound sound);
        public SoundStoppedHandler StoppedEvent;

        #endregion

        internal Sound(string name)
        {
            Name = name;
        }
    }
}
