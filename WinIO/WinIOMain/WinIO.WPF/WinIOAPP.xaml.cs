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

        private static void LoadMainWindow()
        {
            Instance.MainWindow = new MainWindow();
            Instance.MainWindow.Show();
            Dispatcher.Run();
        }

        public static void RunWindow()
        {
            Thread worker = new Thread(new ThreadStart(LoadMainWindow));
            worker.SetApartmentState(ApartmentState.STA);
            worker.Name = "WPFThread";
            worker.IsBackground = true;
            worker.Start();
        }
    }
}
