using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WinIOCodeEditorTest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application application = new Application();
            ResourceDictionary rd = GetResourceDictionaryByURI( new Uri("pack://application:,,,/codeeditor/texteditor.xaml"));
            application.Resources.MergedDictionaries.Add(rd);
            MainWindow mainWindow = new MainWindow();
            application.Run(mainWindow);
        }

        public static ResourceDictionary GetResourceDictionaryByURI(Uri uri)
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = uri;
            return rd;
        }
    }
}
