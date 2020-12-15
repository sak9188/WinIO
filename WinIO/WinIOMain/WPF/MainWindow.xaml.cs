using SourceChord.FluentWPF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using WinIO.WPF.Control;

namespace WinIO.WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private List<Window> windows = new List<Window>();

        private List<TabControl> tabs = new List<TabControl>();

        private Dictionary<TabItem, IORichTextBox> richboxs = new Dictionary<TabItem, IORichTextBox>();

        private TabItem CreateTextBox(string name, string style)
        {
            IORichTextBox textBox = new IORichTextBox();
            textBox.Style = (Style)Resources[style];
            textBox.Document = new FlowDocument(new Paragraph());
            textBox.Document.Style = (Style)Resources["BoxFlowDocument"];
            Grid grid = new Grid();
            grid.Children.Add(textBox);
            TabItem tabItem = new TabItem();
            tabItem.Content = grid;
            tabItem.Header = name;
            richboxs.Add(tabItem, textBox);
            return tabItem;
        }

        private IORichTextBox GetRichTextBox(TabItem item)
        {
            IORichTextBox temp = null;
            richboxs.TryGetValue(item, out temp);
            return temp;
        }

        public bool SetFontSize(TabItem item, double d)
        {
            IORichTextBox temp = GetRichTextBox(item);
            if(temp != null)
            {
                temp.FontSize = d;
                return true;
            }
            return false;
        }

        public bool SetFontFamily(TabItem item, string s)
        {
            IORichTextBox temp = GetRichTextBox(item);
            if (temp != null)
            {
                temp.FontFamily = new System.Windows.Media.FontFamily(s);
                return true;
            }
            return false;
        }

        private TabItem CreateOuputTextBox(string name)
        {
            return CreateTextBox(name, "OutputTextBox");
        }

        private TabItem CreateInputTextBox(string name)
        {
            return CreateTextBox(name, "InputTextBox");
        }

        public void AppendString(TabItem item, string s, string color = null, string fonfamily = null, double fontsize = 0)
        {
            IORichTextBox box = null;
            if(richboxs.TryGetValue(item, out box))
            {
                Paragraph p = (Paragraph)box.Document.Blocks.LastBlock;
                Run r = new Run(s);
                if (color != null)
                { 
                    try
                    {
                        r.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                    }
                    catch (System.FormatException e)
                    {
                        Console.WriteLine("错误的颜色字符串");
                    }
                }
                if (fontsize == 0)
                {
                    fontsize = box.FontSize;
                }
                r.FontSize = fontsize;
                if (fonfamily != null)
                {
                    r.FontFamily = new System.Windows.Media.FontFamily(fonfamily);
                }
                p.Inlines.Add(r);

                double sum = 0;
                foreach (Run i in p.Inlines)
                {
                    sum += GetStringActuallyWidth(i);
                }

                box.MinPageWidth = sum+10;
            }
        }

        public static double GetStringActuallyWidth(Run r)
        {
            return new FormattedText(r.Text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                                    new Typeface(r.FontFamily, r.FontStyle, r.FontWeight, r.FontStretch),
                                    r.FontSize, Brushes.Black).Width;
        }

        public void AppendLine(TabItem item, string s, string color = null, string fonfamily = null, double fontsize = 0)
        {
            IORichTextBox box = null;
            if (richboxs.TryGetValue(item, out box))
            {
                Paragraph p = new Paragraph();
                Run r = new Run(s);
                if (color != null)
                {
                    var cc = (Color)ColorConverter.ConvertFromString(color);
                    if (cc != null)
                    {
                        r.Foreground = new SolidColorBrush(cc);
                    }
                }
                if (fontsize == 0)
                {
                    fontsize = box.FontSize;
                }
                if (fonfamily != null)
                {
                    r.FontFamily = new System.Windows.Media.FontFamily(fonfamily);
                }
                p.Inlines.Add(r);
                box.Document.Blocks.Add(p);

                box.MinPageWidth = GetStringActuallyWidth(r);
            }
        }

        public ExportPython.TabItem CreateOutputTab(uint tabPanel, string name)
        {
            if (tabs.Count > tabPanel)
            {
                var panel = tabs[(int)tabPanel];
                var item = CreateOuputTextBox(name);
                panel.Items.Add(item);
                if(panel.Items.Count == 1)
                {
                    panel.SelectedIndex = 0;
                }
                return item;
            }
            return null;
        }

        public ExportPython.TabItem CreateInputTab(uint tabPanel, string name)
        {
            if (tabs.Count > tabPanel)
            {
                var panel = tabs[(int)tabPanel];
                var item = CreateInputTextBox(name);
                panel.Items.Add(CreateInputTextBox(name));
                if (panel.Items.Count == 1)
                {
                    panel.SelectedIndex = 0;
                }
                return item;
            }
            return null;
        }

        public void CloseTab(uint tabPanel, ExportPython.TabItem item)
        {
            if (tabs.Count > tabPanel)
            {
                var tab = tabs[(int)tabPanel];
                tab.Items.Remove((TabItem)item);
            }
        }

        public void CloseTabPanel(uint tabPanel)
        {
            if (tabs.Count > tabPanel)
            {
                tabs[(int)tabPanel].Items.Clear();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            tabs.Add(TabControl0);
            tabs.Add(TabControl1);
            tabs.Add(TabControl2);
        }
    }
}
