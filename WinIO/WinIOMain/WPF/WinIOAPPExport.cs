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
        private static void CustomInit()
        {
            Action del = () =>
            {
                var window = ((MainWindow)Instance.MainWindow);
                var version_item = window.CreateTopMenuItem("版本菜单", false);
                window.VersionMenu = version_item;
                version_item.AddMenuItem(window.CreateMenuItem("1.8.7：加入可删除的按钮"));
                version_item.AddMenuItem(window.CreateMenuItem("1.8.6：修复配色BUG"));
                version_item.AddMenuItem(window.CreateMenuItem("1.8.4：调整黑色主题"));
                version_item.AddMenuItem(window.CreateMenuItem("1.8.4：巨幅提升性能"));
                version_item.AddMenuItem(window.CreateMenuItem("1.8.3：加强版本控制, 优化崩溃界面"));
                version_item.AddMenuItem(window.CreateMenuItem("1.8.2：加入任意panel多选"));
                version_item.AddMenuItem(window.CreateMenuItem("1.8.1：加入初始化画面"));
                version_item.AddMenuItem(window.CreateMenuItem("1.8.0：加入可以拖动tab按钮, tab按钮现在可以互换位置"));
                //version_item.AddMenuItem(window.CreateMenuItem("1.8.0：控制tab按钮的重排问题"));
                //version_item.AddMenuItem(window.CreateMenuItem("1.8.0：调整WinIO控制颜色,调整整体透明度"));
                //version_item.AddMenuItem(window.CreateMenuItem("1.8.0：调整Menu的颜色,透明度, 添加动画"));
                //version_item.AddMenuItem(window.CreateMenuItem("1.8.0：加入QDir布局"));
                //version_item.AddMenuItem(window.CreateMenuItem("1.0：代码提示功能, 快捷注释，快捷反注释功能"));
                //version_item.AddMenuItem(window.CreateMenuItem("1.0：Lua语法高亮,可交互IO,拖拽功能,自定义配置功能"));
                //version_item.AddMenuItem(window.CreateMenuItem("1.0: 高强度IO环境下，可能会出现卡死的问题"));
                //version_item.AddMenuItem(window.CreateMenuItem("1.0: 浅色主题下IO工具颜色对比度不高的问题"));
                //version_item.AddMenuItem(window.CreateMenuItem("1.0：alt键多选的功能"));
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                Instance.NetSideVersion = $"{version.Major}.{version.Minor}.{version.Build}";
                Instance.VersionMenu = new ExportPython.MenuItem("版本菜单", true, version_item);
            };
            Instance.Dispatcher.Invoke(del);
        }

        public static void AfterPythonInit()
        {
            Action del = () =>
            {
                var window = ((MainWindow)Instance.MainWindow);
                window.MainMenu.Items.Add(window.VersionMenu);
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

        public void SetWindowTitle(string title)
        {
            Action<string> del = (h) => { ((MainWindow)Instance.MainWindow).Title = h;};
            Instance.Dispatcher.Invoke(del, title);
        }
    }
}
