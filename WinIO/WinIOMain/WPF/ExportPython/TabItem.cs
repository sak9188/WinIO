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

        private PyObject onTextEntered;

        public PyObject OnTextEntered
        {
            set
            {
                onTextEntered = value;
                SetOnTextEntered(value);
            }
            get
            {
                return onTextEntered;
            }
        }

        private PyObject onMouseClick;

        public PyObject OnMouseClick
        {
            set
            {
                onMouseClick = value;
                SetOnMouseDown(value);
            }
            get
            {
                return onMouseClick;
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

        public PyObject CompleteData
        {
            set
            {
                if(!PyList.IsListType(value))
                {
                    return;
                }
                SetCompleteData(value);
            }
            get
            {
                return GetCompleteData();
            }
        }

        public int CurrentLineNumber
        {
            get
            {
                return GetCurrentLineNumber();
            }
        }

        public string AboveText
        {
            get
            {
                return GetAboveText();
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
            Action<IOTabItem, string> del = (i, s) => { i.Header = s;  i.AfterSetHeader(); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, str);
        }

        private void SetOnSelected(PyObject onSlected)
        {
            Action<IOTabItem, PyObject> del = (i, o) => { i.PyOnSelected = o; };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, onSlected);
        }

        private void SetOnTextEntered(PyObject onEntered)
        {
            Action<IOTabItem, PyObject> del = (i, o) => { i.PyTextEntered = o; };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, onEntered);
        }

        private void SetOnMouseDown(PyObject onDown)
        {
            Action<IOTabItem, PyObject> del = (i, o) => { i.PyMouseDown = o; };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, onDown);
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

        private int GetCurrentLineNumber()
        {
            Func<IOTabItem, int> del = (i) => { return i.GetLineNumber(); };
            return (int)WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
        }

        private string GetAboveText()
        {
            Func<IOTabItem, string> del = (i) => { return i.GetAboveText(); };
            return (string)WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
        }

        private void SetCompleteData(PyObject tup)
        {
            Action<IOTabItem, PyObject> del = (i, t) => { i.PyComDataList = t; };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, tup);
        }

        private PyObject GetCompleteData()
        {
            Func<IOTabItem, PyObject> del = (i) => { return i.PyComDataList; };
            return (PyObject)WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
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

        public string GetCurrentLineBefore()
        {
            Func<IOTabItem, string> del = (i) => { return i.GetCurrentLineBefore(); };
            return (string)WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
        }

        public string GetLineString(int line)
        {
            Func<IOTabItem, int, string> del = (i, l) => { return i.GetLineString(l); };
            return (string)WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, line);
        }

        public int GetLineCusorPos()
        {
            Func<IOTabItem, int> del = (i) => { return i.GetLineCusorPos(); };
            return (int)WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
        }

        public void Clear()
        {
            Action<IOTabItem> del = (i) => { i.Clear(); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
        }

        public void ShowProgressBar()
        {
            Action<IOTabItem> del = (i) => { i.ShowProgressBar(); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
        }

        public void HideProgressBar()
        {
            Action<IOTabItem> del = (i) => { i.HideProgressBar(); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this);
        }

        public void SetProgressBarValue(double value)
        {
            Action<IOTabItem, double> del = (i, v) => { i.SetProgressBarValue(v); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, value);
        }

        public void SetStatusText(string text)
        {
            Action<IOTabItem, string> del = (i, v) => { i.SetStatusText(v); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, text);
        }

        public void SetStatusSuffixText(string text)
        {
            Action<IOTabItem, string> del = (i, v) => { i.SetStatusSuffixText(v); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, text);
        }

        public void ShowCloseButton(bool show)
        {
            Action<IOTabItem, bool> del = (i, v) => { i.ShowCloseButton(v); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, (IOTabItem)this, show);
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
