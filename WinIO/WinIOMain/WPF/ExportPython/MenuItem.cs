using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinIO.WPF.Control;

namespace WinIO.WPF.ExportPython
{
    public class MenuItem
    {
        public MenuItem(string header, bool topLevel=false)
        {
            this.header = header;
            this.topLevel = topLevel;
            this.menuItem = topLevel ? CreateToplevelMenuItem(this) : CreateMenuItem(this);
        }

        public MenuItem(string header, bool topLevel, IOMenuItem menuItem)
        {
            this.header = header;
            this.topLevel = topLevel;
            this.menuItem = menuItem;
        }

        private IOMenuItem menuItem;

        private List<MenuItem> menuItems = new List<MenuItem>();

        private bool topLevel = false;

        private string header;

        public string Header
        {
            set 
            {
                header = value;
                SetHeader(this, header);
            }
            get 
            {
                return header;
            }
        }

        private PyObject onClick;

        public PyObject OnClick
        {
            set
            {
                onClick = value;
                AddOnClick(this, onClick);
            }
            get
            {
                return onClick;
            }
        }


        public void AddMenuItem(MenuItem item)
        {
            menuItems.Add(item);
            MenuItemAddMenuItem(this, item);
        }

        public void SetForeColor(string color)
        {
            Action<IOMenuItem, string> del = (i, c) => {i.SetForeColor(c);};
            WinIOAPP.Current.Dispatcher.Invoke(del, (IOMenuItem)this, color);
        }

        private static IOMenuItem CreateToplevelMenuItem(MenuItem item)
        {
            Func<string, System.Windows.Controls.MenuItem> del = (s) => { return ((MainWindow)WinIOAPP.Instance.MainWindow).CreateTopMenuItem(s); };
            return (IOMenuItem)WinIOAPP.Current.Dispatcher.Invoke(del, item.header);
        }

        private static IOMenuItem CreateMenuItem(MenuItem item)
        {
            Func<string, System.Windows.Controls.MenuItem> del = (s) => { return ((MainWindow)WinIOAPP.Instance.MainWindow).CreateMenuItem(s); };
            return (IOMenuItem)WinIOAPP.Current.Dispatcher.Invoke(del, item.header);
        }

        private static void MenuItemAddMenuItem(MenuItem parent, MenuItem menuItem)
        {
            Action<IOMenuItem, IOMenuItem> del = (p, s) => { p.AddMenuItem(s); };
            WinIOAPP.Current.Dispatcher.Invoke(del, (IOMenuItem)parent, (IOMenuItem)menuItem);
        }

        private static void AddOnClick(MenuItem item, PyObject click)
        {
            Action<IOMenuItem, PyObject> del = (i, c) => { i.PyOnClick = c;};
            WinIOAPP.Current.Dispatcher.Invoke(del, (IOMenuItem)item, click);
        }

        private static void SetHeader(MenuItem item, string header)
        {
            Action<IOMenuItem, string> del = (i, c) => { i.Header = c; };
            WinIOAPP.Current.Dispatcher.Invoke(del, (IOMenuItem)item, header);
        }


        //public static implicit operator MenuItem(System.Windows.Controls.MenuItem item)
        //{
        //    bool b = (item.Parent is System.Windows.Controls.Menu);
        //    var temp = new MenuItem(item.Header.ToString(), (item.Parent is System.Windows.Controls.Menu));
        //    temp.menuItem = item;
        //    return temp;
        //}

        public static explicit operator IOMenuItem(MenuItem item)
        {
            return item.menuItem;
        }
    }
}
