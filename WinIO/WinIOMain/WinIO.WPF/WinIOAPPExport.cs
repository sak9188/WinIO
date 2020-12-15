using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinIO.WPF
{
    public partial class WinIOAPP : Application
    {
        public void CreateOutputTabPanel(uint tabPanel, string name)
        {
            Action<uint, string> del = (u, s) => { ((MainWindow)Instance.MainWindow).CreateOutputTabPanel(u, s); };
            Instance.Dispatcher.BeginInvoke(del, tabPanel, name);
        }

        public void CloseOutputTabPanel(uint tabPanel)
        {
            Action<uint> del = (u) => { ((MainWindow)Instance.MainWindow).CloseOutputTabPanel(u); };
            Instance.Dispatcher.Invoke(del, tabPanel);
        }

        public void CloseOutputTermminal(uint tabPanel, string name)
        {
            Action<uint, string> del = (u, s) => { ((MainWindow)Instance.MainWindow).CloseOutputTermminal(u, s); };
            Instance.Dispatcher.Invoke(del, tabPanel, name);
        }
    }
}
