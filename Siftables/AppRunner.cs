using System;
using Siftables.Sifteo;
using System.Windows.Threading;
using System.Threading;
using System.Reflection;
using System.Windows;
using System.IO;
using System.Net;
using System.Collections.ObjectModel;

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

        private App _app;

        public App App
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

        public bool LoadAssembly(Stream appStream)
        {
            AssemblyPart assemblyPart = new AssemblyPart();
            Assembly ass = assemblyPart.Load(appStream);

            int i = 0;
            bool foundApp = false;
            Type[] allTypes = ass.GetTypes();

            while ((i < allTypes.Length) && !foundApp)
            {
                if (allTypes[i].BaseType == typeof(BaseApp))
                {
                    Object o = Activator.CreateInstance(allTypes[i]);
                    this._app = new App(o);
                    foundApp = true;
                }
                i++;
            }

            return foundApp;
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
