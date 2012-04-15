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

        public BaseApp App { get; private set; }

        public bool IsRunning { get; private set; }

        public const int TimeBetweenTicks = 500;

        // Dispatcher is used to invoke methods on the UI layer.
        private Dispatcher _uiDispatcher;

        // A new thread is used to execute the application logic.
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
        }

        public void RunAppInThread()
        {
            App.AssociateCubes(Cubes);

            _uiDispatcher.BeginInvoke(() => App.Setup());

            while (IsRunning)
            {
                _uiDispatcher.BeginInvoke(() => App.Tick());
                Thread.Sleep(TimeBetweenTicks);
            }
        }

        public void LoadApplication(Stream appStream)
        {
            var assemblyPart = new AssemblyPart();
            var loadedAssembly = assemblyPart.Load(appStream);
            var assemblyTypes = loadedAssembly.GetTypes();

            App = FindBaseApp(assemblyTypes);
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

            throw new TypeLoadException();
        }

        public void StartExecution(CubeSet cubes, Dispatcher uiDispatcher)
        {
            Cubes = cubes;
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
            throw new NotImplementedException();
        }
    }
}
