using System;
using System.Runtime.InteropServices.Automation;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Siftables
{
    public partial class Siftables
    {

        public Siftables()
        {
            Startup += ApplicationStartup;
            Exit += Application_Exit;
            UnhandledException += ApplicationUnhandledException;

            InitializeComponent();
        }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            RootVisual = new MainWindowView();
        }

        private void Application_Exit(object sender, EventArgs e)
        {
            Console.WriteLine("Exiting");
        }

        private void ApplicationUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                MessageBox.Show(this.MainWindow, "Siftables Emulator has encountered a serious error and needs to close.\n\nTechnical Details: " + e.ExceptionObject.ToString() ,"Unhandled Exception", MessageBoxButton.OK);
                if (AutomationFactory.IsAvailable) //only in Trusted OOB Mode 
                {
                    dynamic _winObject = AutomationFactory.CreateObject("WScript.Shell");
                    _winObject.Run(@"cmd /k taskkill /IM sllauncher.exe & exit", 0);
                }
            }
        }
    }
}
