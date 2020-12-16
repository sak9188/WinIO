using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinIO.WPF.Control;

namespace WinIO.WPF.ExportPython
{
    public class TabItem
    {
        private IOTabItem tabItem;

        private double fontSize;

        public double FontSize
        {
            set
            {
                if (value > 0)
                {
                    if (SetFontSize(value))
                    {
                        fontSize = value;
                    }
                }
            }

            get
            {
                return fontSize;
            }
        }

        private string fontFamily;

        public string FontFamily
        {
            set
            {
                if(value.Length > 0)
                {
                    if(SetFontFamily(value))
                    {
                        fontFamily = value;
                    }
                }
            }

            get
            {
                return fontFamily;
            }
        }

        #region 隐藏设置方法
        private bool SetFontSize(double fontsize)
        {
            Func<IOTabItem, double, bool> del = (i, d) => { return i.SetFontSize(d); };
            return (bool)WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, fontsize);
        }

        private bool SetFontFamily(string fontFamily)
        {
            Func<IOTabItem, string, bool> del = (i, s) => { return i.SetFontFamily(s); };
            return (bool)WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, fontFamily);
        }
        #endregion

        public void AddButton(string str, PyObject click)
        {
            Action<IOTabItem, string, PyObject> del = (i, s, c) => { i.AddButton(s, c); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, str, click);
        }

        public void AppendString(string str, string color = null, string fontfamily = null, double fontsize = 0)
        {
            Action<IOTabItem, string, string, string, double> del = (i, s, c, sf, d) => { i.AppendString(s, c, sf, d); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, str, color, fontfamily, fontsize);
        }

        public void AppendLine(string str, string color = null, string fontfamily = null, double fontsize = 0)
        {
            Action<IOTabItem, string, string, string, double> del = (i, s, c, sf, d) => { i.AppendLine(s, c, sf, d); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, str, color, fontfamily, fontsize);
        }

        public void CloseTab(uint tabPanel)
        {
            Action<uint, System.Windows.Controls.TabItem> del = (u, s) => { ((MainWindow)WinIOAPP.Instance.MainWindow).CloseTab(u, s); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, tabPanel, (IOTabItem)this);
        }

        public static implicit operator TabItem(IOTabItem item)
        {
            var temp = new TabItem();
            temp.fontFamily = item.FontFamily.ToString();
            temp.fontSize = item.FontSize;
            temp.tabItem = item;
            return temp;
        }

        public static explicit operator IOTabItem(TabItem item)
        {
            return item.tabItem;
        }
    }
}
