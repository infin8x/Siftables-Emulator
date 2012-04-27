using System;
using System.Windows.Threading;
using System.Threading;
using System.Windows;
using System.IO;
using Sifteo;

namespace Siftables
{
    public class AppRunner
    {
        public CubeSet Cubes { get; set; }
        public SoundSet Sounds { get; set; }

        public BaseApp App { get; private set; }

        public bool IsRunning { get; private set; }
        public bool IsLoaded { get; private set; }

        public const int MillisecondsBetweenTicks = 500;

        private Dispatcher _uiDispatcher;

        private Thread _runner;

        private static AppRunner _appRunner;
        public static AppRunner GetInstance()
        {
            if (_appRunner == null)
            {
                _appRunner = new AppRunner();
            }
            return _appRunner;
        }

        private AppRunner()
        {
            IsRunning = false;
            IsLoaded = false;
        }

        public void RunAppInThread()
        {
            App.AssociateCubes(Cubes);
            App.AssociateSounds(Sounds);

            _uiDispatcher.BeginInvoke(() => App.Setup());

            while (IsRunning)
            {
                _uiDispatcher.BeginInvoke(() => App.Tick());
                Thread.Sleep(MillisecondsBetweenTicks);
            }
        }

        public void LoadApplication(Stream appStream)
        {
            var assemblyPart = new AssemblyPart();
            var loadedAssembly = assemblyPart.Load(appStream);
            Type[] assemblyTypes;
            if (loadedAssembly == null)
            {
                throw new TypeLoadException("Invalid class library specified");
            }
            try
            {
                assemblyTypes = loadedAssembly.GetTypes();
            } catch (Exception)
            {
                throw new TypeLoadException("Unable to load library types");
            }

            App = FindBaseApp(assemblyTypes);
            IsLoaded = true;
        }

        public BaseApp FindBaseApp(Type[] assemblyTypes)
        {
            foreach (var t in assemblyTypes)
            {
                if (t.BaseType == typeof(BaseApp))
                {
                    return (BaseApp) Activator.CreateInstance(t);
                }
            }

            throw new TypeLoadException("No class found subclassing BaseApp");
        }

        public void StartExecution(CubeSet cubes, Dispatcher uiDispatcher, SoundSet sounds)
        {
            Cubes = cubes;
            Sounds = sounds;
            _uiDispatcher = uiDispatcher;
            if (!IsRunning)
            {
                _runner = new Thread(RunAppInThread);
                IsRunning = true;
                _runner.Start();
            }
        }

        public void StopExecution()
        {
            IsRunning = false;
            _runner.Join();
            ResetCubes();
        }

        public void ResetCubes()
        {
            foreach (var cube in Cubes)
            {
                cube.FillScreen(Color.Black);
            }
        }

        public void PauseExecution()
        {
            IsRunning = false;
            _runner.Join();
        }

        public void ResumeExecution()
        {
            StartExecution(Cubes, _uiDispatcher, Sounds);
        }
    }
}
