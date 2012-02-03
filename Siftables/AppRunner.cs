using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Siftables.Sifteo;
using System.Windows.Threading;
using System.Threading;

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

        public AppRunner(CubeSet cubes, Dispatcher uiDispatcher)
        {
            Cubes = cubes;
            this._uiDispatcher = uiDispatcher;
            this._isRunning = false;
        }

        public void Run()
        {
            _uiDispatcher.BeginInvoke(delegate()
            {
                this._app = new BaseApp(Cubes);
                this._app.Setup();
            });
            Random rand = new Random();
            while (this._isRunning)
            {
                _uiDispatcher.BeginInvoke(delegate()
                {
                    this._app.Tick();
                });
                Thread.Sleep(500);
            }
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
