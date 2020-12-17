using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WinIO.WPF
{
    public partial class WinIOAPP : Application
    {
        public ExportPython.MenuItem CreateMenuItem(string header, bool toplevel=false)
        {
            return new ExportPython.MenuItem(header, toplevel);
        }

        public ExportPython.TabItem CreateOutputTab(uint tabPanel, string name)
        {
            Func<uint, string, ExportPython.TabItem> del = (u, s) => { return ((MainWindow)Instance.MainWindow).CreateOutputTab(u, s); };
            return (ExportPython.TabItem)Instance.Dispatcher.Invoke(del, tabPanel, name);
        }

        public ExportPython.TabItem CreateInputTab(uint tabPanel, string name)
        {
            Func<uint, string, ExportPython.TabItem> del = (u, s) => { return ((MainWindow)Instance.MainWindow).CreateInputTab(u, s); };
            return (ExportPython.TabItem)Instance.Dispatcher.Invoke(del, tabPanel, name);
        }

        public void CloseTabPanel(uint tabPanel)
        {
            Action<uint> del = (u) => { ((MainWindow)Instance.MainWindow).CloseTabPanel(u); };
            Instance.Dispatcher.Invoke(del, tabPanel);
        }
    }
}
