using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Windows.Threading;

namespace demoSoftware
{

    internal delegate void Invoker();
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ApplicationInitialize = _applicationInitialize;
        }
        public static new App Current
        {
            get { return Application.Current as App; }
        }
        internal delegate void ApplicationInitializeDelegate(Splash splashWindow);
        internal ApplicationInitializeDelegate ApplicationInitialize;
        private void _applicationInitialize(Splash splashWindow)
        {

            // fake workload, but with progress updates.
            Thread.Sleep(500);
            splashWindow.SetProgress(1);

            Thread.Sleep(400);
            splashWindow.SetProgress(2);

            Thread.Sleep(300);
            splashWindow.SetProgress(3);

            Thread.Sleep(100);
            splashWindow.SetProgress(4);

            Thread.Sleep(50);
            splashWindow.SetProgress(5);

            // Create the main window, but on the UI thread.
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
            {
                MainWindow window = new MainWindow();
                window.Show();
            });
        }
    }
}