using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WinIO.WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class WinIOAPP : Application
    {
        private static readonly Lazy<WinIOAPP> lazy =
               new Lazy<WinIOAPP>(() => new WinIOAPP());

        public static WinIOAPP Instance { get { return lazy.Value; } }

        public static Thread WindowThread;

        private static void LoadMainWindow()
        {
            Instance.MainWindow = new MainWindow(Thread.CurrentThread);
            Instance.MainWindow.Show();
            var dispatcher = Instance.MainWindow.Dispatcher;
            Instance.MainWindow.Closed += (sender, e) => 
            {
                dispatcher.InvokeShutdown();
            };
            Dispatcher.Run();
        }

        public static void RunWindow()
        {
            WindowThread = new Thread(new ThreadStart(LoadMainWindow));
            WindowThread.SetApartmentState(ApartmentState.STA);
            WindowThread.Name = "WPFThread";
            WindowThread.IsBackground = true;
            WindowThread.Start();
        }
    }
}
