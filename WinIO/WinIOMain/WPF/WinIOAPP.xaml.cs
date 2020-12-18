using Python.Runtime;
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

        public Python.Runtime.PyObject PyClosed;

        private static void LoadMainWindow()
        {
            Instance.MainWindow = new MainWindow(Thread.CurrentThread);
            Instance.MainWindow.Show();
            var dispatcher = Instance.MainWindow.Dispatcher;
            Instance.MainWindow.Closed += (sender, e) =>
            {
                using(Py.GIL())
                {
                    if (WinIOAPP.Instance.PyClosed != null)
                    {
                        WinIOAPP.Instance.PyClosed.Invoke();
                    }
                }
                dispatcher.InvokeShutdown();
            };
            System.Windows.Threading.Dispatcher.Run();
        }



        public static void RunWindow()
        {
            WindowThread = new Thread(new ThreadStart(LoadMainWindow));
            WindowThread.SetApartmentState(ApartmentState.STA);
            WindowThread.Name = "WPFThread";
            WindowThread.IsBackground = true;
            WindowThread.Start();
        }

        public WinIOAPP()
        {
            // 在异常由应用程序引发但未进行处理时发生。主要指的是UI线程。
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            //  当某个异常未被捕获时出现。主要指的是非UI线程
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //可以记录日志并转向错误bug窗口友好提示用户
            if (e.ExceptionObject is System.Exception)
            {
                Exception ex = (System.Exception)e.ExceptionObject;
                Console.WriteLine("Wrong:" + e.ToString());
            }
        }
        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //可以记录日志并转向错误bug窗口友好提示用户
            e.Handled = true;
            Console.WriteLine("Wrong:" + e.Exception.Message + "\r\n" + e.Exception.StackTrace);
        }
    }
}
