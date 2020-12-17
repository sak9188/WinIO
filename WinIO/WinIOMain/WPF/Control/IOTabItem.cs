using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WinIO.WPF.Control
{
    public class IOTabItem : System.Windows.Controls.TabItem
    {
        private IORichTextBox richTextbox;
        private Grid grid;
        private WrapPanel wrapPanel;
        private bool isOutput;
        private string orignalHeader;

        public int TabPanelID { set; get; }
        // private TextBlock wrapPanelName;

        private Dictionary<Button, PyObject> clicks = new Dictionary<Button, PyObject>();

        public IOTabItem(string name, string style, bool isOutput = true)
        {
            this.isOutput = isOutput;

            // RICHBOX
            IORichTextBox textBox = new IORichTextBox();
            textBox.Style = (Style)Control.Resources[style];
            textBox.Document = new FlowDocument(new Paragraph());
            textBox.Document.Style = (Style)Control.Resources["BoxFlowDocument"];
            this.richTextbox = textBox;

            // GRID
            Grid grid = new Grid();
            var topRow = new RowDefinition();
            topRow.Height = GridLength.Auto;
            grid.RowDefinitions.Add(topRow);
            grid.RowDefinitions.Add(new RowDefinition());
            grid.Children.Add(textBox);
            Grid.SetRow(textBox, 1);
            this.grid = grid;

            // WrapPanel
            WrapPanel panel = new WrapPanel();
            grid.Children.Add(panel);
            Grid.SetRow(panel, 0);
            this.wrapPanel = panel;

            // Tabitem
            this.Content = grid;
            this.Header = name;
            orignalHeader = name;
        }

        public void AddButton(string name, PyObject OnClick)
        {
            // WrapPanel - button
            Button button = new Button();
            button.Content = name;
            button.Style = (Style)Control.Resources["TabPanelButton"];
            button.Click += Button_Click;
            clicks.Add(button, OnClick);
            wrapPanel.Children.Add(button);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bu = (Button)sender;
            PyObject click = null;
            if (clicks.TryGetValue(bu, out click))
            {
                using(Py.GIL())
                {
                    click.Invoke();
                }
            }
        }

        public bool SetFontSize(double d)
        {
            IORichTextBox temp = richTextbox;
            if (temp != null)
            {
                temp.FontSize = d;
                return true;
            }
            return false;
        }

        public bool SetFontFamily(string s)
        {
            IORichTextBox temp = richTextbox;
            if (temp != null)
            {
                temp.FontFamily = new FontFamily(s);
                return true;
            }
            return false;
        }

        public void AppendString(string s, string color = null, string fonfamily = null, double fontsize = 0)
        {
            Paragraph p = (Paragraph)richTextbox.Document.Blocks.LastBlock;
            Run r = new Run(s);
            if (color != null)
            {
                try
                {
                    r.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("错误的颜色字符串");
                }
            }
            if (fontsize == 0)
            {
                fontsize = richTextbox.FontSize;
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

            richTextbox.MinPageWidth = sum + 10;
        }

        public static double GetStringActuallyWidth(Run r)
        {
            return new FormattedText(r.Text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                                    new Typeface(r.FontFamily, r.FontStyle, r.FontWeight, r.FontStretch),
                                    r.FontSize, Brushes.Black).Width;
        }

        public void AppendLine(string s, string color = null, string fonfamily = null, double fontsize = 0)
        {
            IORichTextBox box = richTextbox;
            Paragraph p = (Paragraph)richTextbox.Document.Blocks.LastBlock;
            Run r = new Run(s);
            if (color != null)
            {
                try
                {
                    r.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("错误的颜色字符串");
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
            p.Inlines.Add(new LineBreak());
            // box.Document.Blocks.Add(p);
            box.MinPageWidth = GetStringActuallyWidth(r);
        }

        public string GetText()
        {
            TextRange textRange = new TextRange(richTextbox.Document.ContentStart, richTextbox.Document.ContentEnd);
            return textRange.Text;
        }

        public void Clear()
        {
            IORichTextBox box = richTextbox;
            Paragraph p = (Paragraph)box.Document.Blocks.LastBlock;
            p.Inlines.Clear();
        }

        public bool IsCurrent()
        {
            return this.IsSelected;
        }

        public void SetRichBoxKeyDownEvent(PyObject pyKeyDown)
        {
            richTextbox.PyKeyDown = pyKeyDown;
        }
    }
}
