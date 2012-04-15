namespace Sifteo
{
    public class Sound
    {
        #region Public Member Functions

        public void Play(float volume, int loops = 0)
        {
            Play(volume, volume, loops);
        }

        public void Play(float volumeLeft, float volumeRight, int loops=0)
        {
            
        }

        public void Stop()
        {
            
        }

        public void Pause()
        {
            
        }

        public void Resume()
        {
            
        }

        public void SetVolume(float volume)
        {
            SetVolume(volume, volume);
        }

        public void SetVolume(float volumeLeft, float volumeRight)
        {
            
        }

        #endregion

        #region Static Public Member Functions

        public static void PauseAll()
        {
            
        }

        public static void ResumeAll()
        {
            
        }

        public static void StopAll()
        {
            
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public bool IsPlaying { get; private set; }

        public bool IsPaused { get; set; }

        #endregion

        #region Events

        public SoundStoppedHandler StoppedEvent;
        public SoundStartedHandler StartedEvent;

        #endregion
    }
}
