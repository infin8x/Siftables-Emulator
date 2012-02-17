using System;
using System.Reflection;
using Siftables.Sifteo;
namespace Siftables
{
    public class App
    {
        private Object _dApp;
        private MethodInfo _appSetup;
        private MethodInfo _appTick;
        private MethodInfo _appAssociateCubes;

        public App(Object dApp) {
            this._dApp = dApp;

            Type dAppType = dApp.GetType();
            this._appSetup = dAppType.GetMethod("Setup");
            this._appTick = dAppType.GetMethod("Tick");
            this._appAssociateCubes = dAppType.GetMethod("AssociateCubes");
        }

        public void AssociateCubes(CubeSet cubes) {
            this._appAssociateCubes.Invoke(this._dApp, new object[] { cubes });
        }

        public void Setup()
        {
            this._appSetup.Invoke(this._dApp, null);
        }

        public void Tick()
        {
            this._appTick.Invoke(this._dApp, null);
        }

    }
}
