using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WinIO.WPF;

namespace WinIO.IOConsole
{
    class IOMain
    {
        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            string resourceName = new AssemblyName(args.Name).Name;
            resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().First(x => x.Contains(resourceName+".dll"));
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
            RealMain();
        }

        static void RealMain()
        {
            PythonEngine.Initialize();
            WinIOAPP.RunWindow();
            // dynamic sys = Py.Import("sys");
            dynamic CSharp = Py.Import("WinIO.CSharp");
            CSharp.APP = WinIOAPP.Instance;
            try
            {
                dynamic io = Py.Import("WinIOStart");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            while (true)
            {
                if(!WinIOAPP.WindowThread.IsAlive)
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
    }
}
