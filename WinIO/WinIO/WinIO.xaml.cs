using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WinIO.WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class WinIOApp : Application
    {
        public WinIOApp()
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("pack://application:,,,/FluentWPF;component/Styles/Controls.xaml");
            this.Resources.MergedDictionaries.Add(rd);
        }
    }
}
