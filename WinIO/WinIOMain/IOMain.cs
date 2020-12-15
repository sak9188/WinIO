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
            WinIOAPP.RunWindow();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                dynamic app = Py.Import("WinIO.CSharp.APP");
                sys.modules["WinIO.CSharp.APP"] = WinIOAPP.Instance.ToPython();
                dynamic io = Py.Import("WinIO.WinIO");
                io.init_winio();
            }
            Console.ReadKey();
        }
    }
}
