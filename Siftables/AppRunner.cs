using System;
using Siftables.Sifteo;
using System.Windows.Threading;
using System.Threading;
using System.Reflection;
using System.Windows;
using System.IO;

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

        public BaseApp App
        {
            get
            {
                return this._app;
            }
        }

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

        private AppRunner()
        {
            this._isRunning = false;
        }

        private static AppRunner _appRunner;

        public static AppRunner getInstance()
        {
            if (_appRunner == null)
            {
                _appRunner = new AppRunner();
            }
            return _appRunner;
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
            /*String pathToAppDLL = @"C:\Users\zimmerka\Siftables-Emulator\Applications\ChangingColorsApp\ChangingColorsApp\bin\Debug\ChangingColorsApp.dll";
            //File.Move(pathToAppDLL, "ChangingColorsApp.dll");

            AssemblyPart ap = new AssemblyPart();
            Assembly appSpace = Assembly.Load("ChangingColorsApp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            Type baseApp = typeof(Sifteo.BaseApp);

            Type appType = appSpace.GetType();
            foreach (Type type in appSpace.GetTypes())
            {
                // Look for any classes which subclass BaseApp - this is the application code
                if (type.IsSubclassOf(baseApp))
                {
                    MessageBox.Show("Found " + type.FullName);
                    this._app = (BaseApp) Activator.CreateInstance(type);
                    appType = type;
                }
            }*/
        }

        public void StartExecution(CubeSet cubes, Dispatcher uiDispatcher)
        {
            Cubes = cubes;
            this._uiDispatcher = uiDispatcher;
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
