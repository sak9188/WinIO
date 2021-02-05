using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
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
            resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(x => x.Contains(resourceName+".dll"));
            if(resourceName == null) return null;
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
                //获取项目的绝对路径
                using (FileStream fs = new FileStream("./crash.log", FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(e.Message);
                        if(e.Source.StartsWith("Python"))
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
