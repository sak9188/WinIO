using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinIO
{
    class IOMain
    {
        static System.Windows.Application application = new System.Windows.Application();

        [STAThread]
        static void Main(string[] args)
        {
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
