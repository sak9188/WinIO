using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
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
using System.Windows.Input;
using System.Windows.Media;

namespace WinIO.WPF.Control
{
    public class IOTabItem : System.Windows.Controls.TabItem
    {
        private IORichTextBox richTextbox;
        private TextEditor textEditor;
        private Grid grid;
        private WrapPanel wrapPanel;
        private bool isOutput;
        private string orignalHeader;

        public int TabPanelID { set; get; }

        public PyObject PyOnSelected { set; get; }

        private PyObject PyInputObject;

        // private TextBlock wrapPanelName;

        private Dictionary<Button, PyObject> clicks = new Dictionary<Button, PyObject>();

        public IOTabItem(string name, string style, bool isOutput = true)
        {
            this.isOutput = isOutput;

            // GRID
            Grid grid = new Grid();
            var topRow = new RowDefinition();
            topRow.Height = GridLength.Auto;
            grid.RowDefinitions.Add(topRow);
            grid.RowDefinitions.Add(new RowDefinition());
            this.grid = grid;

            if(isOutput)
            {
                // RICHBOX
                IORichTextBox textBox = new IORichTextBox();
                textBox.Style = (Style)Control.Resources[style];
                textBox.Document = new FlowDocument(new Paragraph());
                textBox.Document.Style = (Style)Control.Resources["BoxFlowDocument"];
                this.richTextbox = textBox;

                grid.Children.Add(textBox);
                Grid.SetRow(textBox, 1);
            }
            else
            {
                TextEditor textEditor = new TextEditor();
                textEditor.Style = (Style)Control.Resources[style];
                var temp = new TextEditorOptions();
                temp.ShowSpaces = true;
                temp.ShowTabs = true;
                temp.EnableEmailHyperlinks = false;
                temp.EnableHyperlinks = false;
                textEditor.Options = temp;
                textEditor.KeyDown += TextEditor_KeyDown;
                textEditor.DragOver += TextEditor_DragOver;
                this.textEditor = textEditor;
                grid.Children.Add(textEditor);
                Grid.SetRow(textEditor, 1);
            }

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

        private void TextEditor_DragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void TextEditor_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (PyInputObject != null)
            {
                using (Py.GIL())
                {
                    PyInputObject.Invoke(e.Key.ToPython(), e.Key.ToString().ToPython());
                }
            }
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
            if (isOutput)
            {
                IORichTextBox temp = richTextbox;
                if (temp != null)
                {
                    temp.FontSize = d;
                    return true;
                }
                return false;
            }else
            {
                var temp = textEditor;
                if (temp != null)
                {
                    temp.FontSize = d;
                    return true;
                }
                return false;
            }
        }

        public bool SetFontFamily(string s)
        {
            if(isOutput)
            {
                IORichTextBox temp = richTextbox;
                if (temp != null)
                {
                    temp.FontFamily = new FontFamily(s);
                    return true;
                }
                return false;
            }
            else
            {
                var temp = textEditor;
                if (temp != null)
                {
                    temp.FontFamily = new FontFamily(s);
                    return true;
                }
                return false;
            }
        }

        public void AppendOutputString(string s, string color = null, string fonfamily = null, double fontsize = 0)
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

            // 无论输出端干了啥，必须得滚到最下面
            richTextbox.ScrollToEnd();
        }

        public void AppendInputString(string s, string color = null, string fonfamily = null, double fontsize = 0)
        {
            this.textEditor.Text = s;
        }

        public void AppendString(string s, string color = null, string fonfamily = null, double fontsize = 0)
        {
            if(this.isOutput)
            {
                this.AppendOutputString(s, color, fonfamily, fontsize);
            }else
            {
                this.AppendInputString(s);
            }
        }

        public static double GetStringActuallyWidth(Run r)
        {
            return new FormattedText(r.Text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                                    new Typeface(r.FontFamily, r.FontStyle, r.FontWeight, r.FontStretch),
                                    r.FontSize, Brushes.Black).Width;
        }

        public void AppendLine(string s, string color = null, string fonfamily = null, double fontsize = 0)
        {
            if (this.isOutput)
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

                // 无论输出端干了啥，必须得滚到最下面
                richTextbox.ScrollToEnd();
            }
        }

        public string GetText()
        {
            if(this.isOutput)
            {
                TextRange textRange = new TextRange(richTextbox.Document.ContentStart, richTextbox.Document.ContentEnd);
                return textRange.Text;
            }else
            {
                return textEditor.Text;
            }
        }

        public void Clear()
        {
            if(this.isOutput)
            {
                IORichTextBox box = richTextbox;
                Paragraph p = (Paragraph)box.Document.Blocks.LastBlock;
                p.Inlines.Clear();
            }else
            {
                textEditor.Clear();
            }
        }

        public bool IsCurrent()
        {
            return this.IsSelected;
        }

        public void SetRichBoxKeyDownEvent(PyObject pyKeyDown)
        {
            if(this.isOutput)
            {
                richTextbox.PyKeyDown = pyKeyDown;
            }else
            {
                PyInputObject = pyKeyDown;
            }
        }

        public void Selected()
        {
            if(PyOnSelected != null)
            {
                using(Py.GIL())
                {
                    PyOnSelected.Invoke();
                }
            }
        }

        public void SetSyntaxHighlighting(string format)
        {
            if(textEditor != null)
            {
                var typeConverter = new HighlightingDefinitionTypeConverter();
                textEditor.SyntaxHighlighting = (IHighlightingDefinition)typeConverter.ConvertFrom(format);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if(e.Source == this)
            {
                base.OnMouseMove(e);

                // 判断左键是否按下
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    //声明DataObject,并打包圆控件的图像绘制方式(包含颜色)、高度及其副本。
                    DataObject data = new DataObject();
                    data.SetData(this.GetType().Name, this);

                    //使用DragDrop的DoDragDrop方法开启拖动功能。拖动方式为拖动复制或移动
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
            }
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);

            //Effects获取拖动类型
            if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                Mouse.SetCursor(Cursors.Hand);
            }
            else
            {
                Mouse.SetCursor(Cursors.No);
            }
            e.Handled = true;
        }
    }
}
