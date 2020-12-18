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
        public static void CustomInit()
        {
            Action del = () =>
            {
                var window = ((MainWindow)Instance.MainWindow);
                var s_version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                var version = window.CreateTopMenuItem("当前版本："+ s_version);
                var change_1 = window.CreateMenuItem("取消：alt键多选的功能");
                var plan_1 = window.CreateMenuItem("计划：尝试加入拖拽功能");
                version.AddMenuItem(change_1);
                version.AddMenuItem(plan_1);
            };
            Instance.Dispatcher.Invoke(del);
        }

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
