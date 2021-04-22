using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
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

        public PyObject PyClosed;
        public PyObject IsClosing;

        private static void LoadMainWindow()
        {
            Instance.MainWindow = new MainWindow(Thread.CurrentThread);
            var dispatcher = Instance.MainWindow.Dispatcher;
            Instance.MainWindow.Closing += (sender, e) =>
            {
                using(Py.GIL())
                {
                    if (Instance.PyClosed != null)
                    {
                        Instance.IsClosing = true.ToPython();
                        Instance.PyClosed.Invoke();
                    }
                }
                dispatcher.InvokeShutdown();
            };
            System.Windows.Threading.Dispatcher.Run();
        }

        public static void RealMain()
        {
            PythonEngine.Initialize();
            WinIOAPP.RunWindow();
            dynamic CSharp = Py.Import("WinIO.CSharp");
            CSharp.APP = WinIOAPP.Instance;
            try
            {
                dynamic io = Py.Import("WinIOStart");
                var state = PythonEngine.BeginAllowThreads();
                WinIOAPP.CustomInit();
                PythonEngine.EndAllowThreads(state);
            }
            catch (Exception e)
            {
                MessageBox.Show("程序崩溃了,大概率是因为载入了不存在的Python模块所导致的.具体情况请在程序文件夹下的crash.log查看");
                //获取项目的绝对路径
                using (FileStream fs = new FileStream("./crash.log", FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(e.Message);
                        if (e.Source.StartsWith("Python"))
                        {
                            string stack = e.StackTrace;
                            int idx = stack.IndexOf(']');
                            stack = stack.Substring(0, idx);
                            string pattern = "(')(?:(?!\\1).)*?\\1";
                            sw.Write(e.Message + "\n");   //将内容写入文件
                            sw.WriteLine("Traceback");
                            foreach (Match match in Regex.Matches(stack, pattern))
                            {
                                var tims = match.Value.Trim('\'');
                                tims = tims.Replace("\\n", "\n");
                                sw.Write(tims);
                            }
                        }
                        sw.WriteLine();
                    }
                }
                Environment.Exit(1);
            }
            while (true)
            {
                if (!WinIOAPP.WindowThread.IsAlive)
                {
                    WinIOAPP.WindowThread.Abort();
                    break;
                }
                var state = PythonEngine.BeginAllowThreads();
                Thread.Sleep(10);
                PythonEngine.EndAllowThreads(state);
            }
            PythonEngine.Shutdown();
        }

        /// <summary>
        /// Interaction logic for App.xaml
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            //根据图片路径，实例化启动图片
            SplashScreen splashScreen = new SplashScreen("\\splash.png");
            splashScreen.Show(false);
            splashScreen.Close(new TimeSpan(0, 0, 1));
            base.OnStartup(e);
            MainWindow.Show();
            // Thread.Sleep(1000);
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
#if DEBUG
            if (e.ExceptionObject is System.Exception)
            {
                Exception ex = (System.Exception)e.ExceptionObject;
                Console.WriteLine("Wrong:" + e.ToString());
            }
#endif
        }
        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //可以记录日志并转向错误bug窗口友好提示用户
            e.Handled = true;
            string estring = e.Exception.Message + "\r\n" + e.Exception.StackTrace;
#if DEBUG
            Console.WriteLine("Wrong:" + estring);
#endif
            if (e.Exception.Source.StartsWith("Python"))
            {
                using (Py.GIL())
                {
                    string stack = e.Exception.StackTrace;
                    int idx = stack.IndexOf(']');
                    stack = stack.Substring(0, idx);
                    string pattern = "(')(?:(?!\\1).)*?\\1";
                    PyPrint(e.Exception.Message);
                    PyPrint("Traceback");
                    foreach (Match match in Regex.Matches(stack, pattern))
                    {
                        var tims = match.Value.Trim('\'');
                        tims = tims.Replace("\\n", "\n");
                        PyPrint(tims);
                    }
                }
            }
        }

        public void PyPrint(string s)
        {
            using (Py.GIL())
            {
                using (var pyScope = Py.CreateScope())
                {
                    pyScope.Set("exp", s);
                    pyScope.Exec("print exp");
                }
            }
        }
    }
}
