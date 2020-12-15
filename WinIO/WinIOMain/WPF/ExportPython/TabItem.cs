using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinIO.WPF.ExportPython
{
    public class TabItem
    {
        private System.Windows.Controls.TabItem tabItem;

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

        private bool SetFontSize(double fontsize)
        {
            Func<TabItem, double, bool> del = (i, d) => { return ((MainWindow)WinIOAPP.Instance.MainWindow).SetFontSize((System.Windows.Controls.TabItem)i, d); };
            return (bool)WinIOAPP.Instance.Dispatcher.Invoke(del, this, fontsize);
        }

        private bool SetFontFamily(string fontFamily)
        {
            Func<TabItem, string, bool> del = (i, s) => { return ((MainWindow)WinIOAPP.Instance.MainWindow).SetFontFamily((System.Windows.Controls.TabItem)i, s); };
            return (bool)WinIOAPP.Instance.Dispatcher.Invoke(del, this, fontFamily);
        }

        public void AppendString(string str, string color = null, string fontfamily = null, double fontsize = 0)
        {
            Action<TabItem, string, string, string, double> del = (i, s, c, sf, d) => { ((MainWindow)WinIOAPP.Instance.MainWindow).AppendString((System.Windows.Controls.TabItem)i, s, c, sf, d); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, this, str, color, fontfamily, fontsize);
        }

        public void AppendLine(string str, string color = null, string fontfamily = null, double fontsize = 0)
        {
            Action<TabItem, string, string, string, double> del = (i, s, c, sf, d) => { ((MainWindow)WinIOAPP.Instance.MainWindow).AppendLine((System.Windows.Controls.TabItem)i, s, c, sf, d); };
            WinIOAPP.Instance.Dispatcher.Invoke(del, this, str, color, fontfamily, fontsize);
        }

        public static implicit operator TabItem(System.Windows.Controls.TabItem item)
        {
            var temp = new TabItem();
            temp.fontFamily = item.FontFamily.ToString();
            temp.fontSize = item.FontSize;
            temp.tabItem = item;
            return temp;
        }

        public static explicit operator System.Windows.Controls.TabItem(TabItem item)
        {
            return item.tabItem;
        }
    }
}
