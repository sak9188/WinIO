using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WinIO.WPF;

namespace WinIO.IOConsole
{
    class IOMain
    {
        // static WinIOApp application = null;

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
            WinIOApp application = new WinIOApp();

            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                Console.WriteLine(sys.path);

                application.Run(new MainWindow());
            }

            //Thread worker = new Thread(new ThreadStart(LoadWpfApp));
            //worker.SetApartmentState(ApartmentState.STA);
            //worker.Name = "WpfThread";
            //worker.IsBackground = true;
            //worker.Start();
        }
    }
}
