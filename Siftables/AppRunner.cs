using System;
using Siftables.Sifteo;
using System.Windows.Threading;
using System.Threading;
using System.Reflection;

namespace Siftables
{
    public class AppRunner
    {
        private CubeSet _cubes;

        public CubeSet Cubes
        {
            get
            {
                return this._cubes;
            }
            set
            {
                this._cubes = value;
            }
        }

        private Dispatcher _uiDispatcher;

        private BaseApp _app;

        private bool _isRunning;

        public bool Running
        {
            get { return this._isRunning; }
        }

        private Thread _runner;

        private String _appPath;

        public String AppPath
        {
            get
            {
                return this._appPath;
            }
        }

        private delegate void SetupMethod();

        public delegate void TickMethod();

        public AppRunner(CubeSet cubes, Dispatcher uiDispatcher)
        {
            Cubes = cubes;
            this._uiDispatcher = uiDispatcher;
            this._isRunning = false;
        }

        public void Run()
        {
            this._app = new MyApp();
            this._app.AssociateCubes(Cubes);
            _uiDispatcher.BeginInvoke(delegate()
            {
                this._app.Setup();
            });

            while (this._isRunning)
            {
                _uiDispatcher.BeginInvoke(delegate()
                {
                    this._app.Tick();
                });
                Thread.Sleep(500);
            }
        }

        public void LoadAssembly(String appPath)
        {
            this._appPath = appPath;
            // Where the generated assembly will reside
            /*String pathToAppDLL = @"C:\Users\zimmerka\App.dll";

            Assembly app = Assembly.LoadFrom(pathToAppDLL);

            Type appType = helloworld.GetType();
            foreach (Type type in helloworld.GetTypes())
            {
                // Look for any classes which subclass BaseApp - this is the application code
                if (type.IsSubclassOf(baseApp))
                {
                    Console.WriteLine("Found " + type.FullName);
                    //var app = Activator.CreateInstance(type);
                    appType = type;
                }
            }*/
        }

        public void StartExecution()
        {
            if (!_isRunning)
            {
                this._runner = new Thread(Run);
                this._isRunning = true;
                this._runner.Start();
            }
        }

        public void StopExecution()
        {
            this._isRunning = false;
            this._runner.Join();
        }

        public void PauseExecution()
        {
            StopExecution();
        }
    }
}
