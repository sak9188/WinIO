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

        private string header;

        public string Header
        {
            set 
            {
                header = value;
                SetHeader(value);
            }
            get 
            {
                return header;
            }
        }

        private PyObject onSelcted;

        public PyObject OnSelcted
        {
            set
            {
                onSelcted = value;
                SetOnSelected(value);
            }
            get
            {
                return onSelcted;
            }
        }

        private string syntax;

        public string Syntax
        {
            set
            {
                syntax = value;
                SetSyntax(value);
            }
            get
            {
                return syntax;
            }
        }

        public int TabIndex
        {
            get
            {
                return GetTabIndex();
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

        private void SetHeader(string str)
        {
            Action<IOTabItem, string> del = (i, s) => { i.Header = s; };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, str);
        }

        private void SetOnSelected(PyObject onSlected)
        {
            Action<IOTabItem, PyObject> del = (i, o) => { i.PyOnSelected = o; };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, onSlected);
        }

        private void SetSyntax(string format)
        {
            Action<IOTabItem, string> del = (i, s) => { i.SetSyntaxHighlighting(s); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, format);
        }

        private int GetTabIndex()
        {
            Func<IOTabItem, int> del = (i) => { return i.TabPanelID; };
            return (int)WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
        }
        #endregion

        public bool IsCurrentTab()
        {
            Func<IOTabItem, bool> del = (i) => { return i.IsCurrent(); };
            return (bool)WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
        }

        public void AddButton(string str, PyObject click)
        {
            Action<IOTabItem, string, PyObject> del = (i, s, c) => { i.AddButton(s, c); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, str, click);
        }

        public void SetButtonClick(string str, PyObject click)
        {
            Action<IOTabItem, string, PyObject> del = (i, s, c) => { i.SetButtonClick(s, c); };
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

        public void Close()
        {
            Action<IOTabItem> del = (s) => { ((MainWindow)WinIOAPP.Instance.MainWindow).CloseTab(s); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
        }

        public void SetKeyDown(PyObject pyKeyDown)
        {
            Action<IOTabItem, PyObject> del = (i, p) => { i.SetRichBoxKeyDownEvent(p); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, pyKeyDown);
        }

        public void SetKeyUp(PyObject pyKeyDown)
        {
            Action<IOTabItem, PyObject> del = (i, p) => { i.SetRichBoxKeyUpEvent(p); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, pyKeyDown);
        }

        public void QuickAnotation()
        {
            Action<IOTabItem> del = (i) => { i.QuickAnnotation(); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
        }

        public void QuickAntiAnnotation()
        {
            Action<IOTabItem> del = (i) => { i.QuickAntiAnnotation(); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
        }

        public string GetText()
        {
            Func<IOTabItem, string> del = (i) => { return i.GetText(); };
            return (string)WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
        }


        public void Clear()
        {
            Action<IOTabItem> del = (i) => { i.Clear(); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
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
