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
        public ExportPython.TabItem CreateOutputTab(uint tabPanel, string name)
        {
            Func<uint, string, ExportPython.TabItem> del = (u, s) => { return ((MainWindow)Instance.MainWindow).CreateOutputTab(u, s); };
            return (ExportPython.TabItem)Instance.Dispatcher.Invoke(del, tabPanel, name);
        }

        public void CreateInputTab(uint tabPanel, string name)
        {
            Action<uint, string> del = (u, s) => { ((MainWindow)Instance.MainWindow).CreateInputTab(u, s); };
            Instance.Dispatcher.Invoke(del, tabPanel, name);
        }

        public void CloseTabPanel(uint tabPanel)
        {
            Action<uint> del = (u) => { ((MainWindow)Instance.MainWindow).CloseTabPanel(u); };
            Instance.Dispatcher.Invoke(del, tabPanel);
        }

        public void CloseTab(uint tabPanel, ExportPython.TabItem item)
        {
            Action<uint, ExportPython.TabItem> del = (u, s) => { ((MainWindow)Instance.MainWindow).CloseTab(u, s); };
            Instance.Dispatcher.Invoke(del, tabPanel, item);
        }
    }
}
