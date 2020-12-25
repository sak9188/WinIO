using Python.Runtime;
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
                version.AddMenuItem(window.CreateMenuItem("加入：快捷注释，快捷反注释功能"));
                version.AddMenuItem(window.CreateMenuItem("优化: 浅色主题下IO工具颜色对比度不高的问题"));
                version.AddMenuItem(window.CreateMenuItem("加入: Lua语法高亮"));
                version.AddMenuItem(window.CreateMenuItem("加入: 可交互IO"));
                version.AddMenuItem(window.CreateMenuItem("加入：拖拽功能"));
                version.AddMenuItem(window.CreateMenuItem("加入：自定义配置功能"));
                version.AddMenuItem(window.CreateMenuItem("取消：alt键多选的功能"));
                version.AddMenuItem(window.CreateMenuItem("TODO：加入代码提示功能"));
                version.AddMenuItem(window.CreateMenuItem("TODO：加入任意panel多选"));
                version.AddMenuItem(window.CreateMenuItem("TODO：加入QDir布局"));
            };
            Instance.Dispatcher.Invoke(del);
        }

        public Tuple<double, double> GetWindowSize()
        {
            Func<Tuple<double, double>> del = () => { return ((MainWindow)Instance.MainWindow).GetWindowSize(); };
            return Instance.Dispatcher.Invoke(del);
        }

        public void SetWindowSize(double hight, double width)
        {
            Action<double, double> del = (h, w) => { ((MainWindow)Instance.MainWindow).SetWindowsSize(h, w); };
            Instance.Dispatcher.Invoke(del, hight, width);
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
