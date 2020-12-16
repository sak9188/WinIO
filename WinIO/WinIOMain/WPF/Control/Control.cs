using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinIO.WPF.Control
{
    public static class Control
    {
        public static ResourceDictionary Resources = GetResourceDictionaryByURI(new Uri(@"pack://application:,,,/wpf/control/control.xaml"));

        public static ResourceDictionary GetResourceDictionaryByURI(Uri uri)
        {
             ResourceDictionary rd = new ResourceDictionary();
            rd.Source = uri;
            return rd;
        }
    }
}
