using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Highlighting;
using Python.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using WinIO.WPF.Command;

namespace WinIO.WPF.Control
{
    public class IOTabItem : System.Windows.Controls.TabItem
    {
        private IORichTextBox richTextbox;
        private TextEditor textEditor;
        private Grid grid;
        private WrapPanel wrapPanel;
        private StatusBar statusBar;
        private bool isOutput;
        private string orignalHeader;

        public int TabPanelID { set; get; }

        private PyObject pyComDataList;
        public PyObject PyComDataList
        {
            set
            {
                if (PyList.IsListType(value))
                {
                    pyComDataList = value;
                }
            }
            get
            {
                return pyComDataList;
            }
        }

        public PyObject PyMouseDown { set; get; }
        public PyObject PyTextEntered { set; get; }
        public PyObject PyOnSelected { set; get; }
        private PyObject PyInputObject;
        private PyObject PyInputObjectKeyUp;

        CompletionWindow completionWindow;

        private Dictionary<string, PyObject> clicks = new Dictionary<string, PyObject>();

        public IOTabItem(string name, string style, bool isOutput = true)
        {
            this.MinWidth = 20;

            this.isOutput = isOutput;

            // GRID
            Grid grid = new Grid();
            var topRow = new RowDefinition();
            topRow.Height = GridLength.Auto;
            grid.RowDefinitions.Add(topRow);
            grid.RowDefinitions.Add(new RowDefinition());
            var secRow = new RowDefinition();
            secRow.Height = GridLength.Auto;
            grid.RowDefinitions.Add(secRow);

            this.grid = grid;

            if (isOutput)
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
                temp.ShowEndOfLine = true;
                temp.ShowSpaces = true;
                temp.ShowTabs = true;
                temp.EnableEmailHyperlinks = false;
                temp.EnableHyperlinks = false;
                temp.HighlightCurrentLine = true;
                textEditor.TextArea.SelectionCornerRadius = 0;
                textEditor.Options = temp;
                textEditor.KeyUp += TextEditor_KeyUp;
                textEditor.PreviewKeyDown += TextEditor_KeyDown;
                textEditor.PreviewMouseLeftButtonUp += TextEditor_MouseUp;
                textEditor.TextArea.TextEntered += TextArea_TextEntered;
                //DataObject.AddPastingHandler(textEditor, TextArea_TextPasted);
                textEditor.ShowLineNumbers = true;
                this.textEditor = textEditor;
                grid.Children.Add(textEditor);
                Grid.SetRow(textEditor, 1);

            }

            this.ClickCloseBtnCommand = new RelayCommand(collapseItem);
            DataContext = this;

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

        private Button btnCloseButton;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var tem = this.Template;
            btnCloseButton = (Button)tem.FindName("btnCloseButton", this);
            //ShowCloseButton(false);
        }

        public void ShowCloseButton(bool show=true)
        {
            if (btnCloseButton != null)
            {
                if (show)
                {
                    btnCloseButton.Visibility = Visibility.Visible;
                }
                else
                {
                    btnCloseButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        private TextBlock statusBarPrefixText;
        private TextBlock statusBarSuffixText;
        private ProgressBar progressBar;

        public void ShowProgressBar()
        {
            if (this.isOutput)
            {
                if (statusBar == null)
                {
                    statusBar = new StatusBar();
                    statusBar.Background = new SolidColorBrush(Color.FromRgb(40, 44, 52));
                    statusBar.Foreground = new SolidColorBrush(Colors.White);

                    var descTextBlock = new StatusBarItem();
                    statusBarPrefixText = new TextBlock();
                    descTextBlock.Content = statusBarPrefixText;

                    progressBar = new ProgressBar();
                    progressBar.IsIndeterminate = false;
                    progressBar.Orientation = Orientation.Horizontal;
                    progressBar.Height = 20;
                    progressBar.Width = 200;
                    progressBar.Value = 100;

                    var suffixDescTextBlock = new StatusBarItem();
                    statusBarSuffixText = new TextBlock();
                    suffixDescTextBlock.Content = statusBarSuffixText;

                    statusBar.Items.Add(descTextBlock);
                    statusBar.Items.Add(progressBar);
                    statusBar.Items.Add(new Separator());
                    statusBar.Items.Add(suffixDescTextBlock);

                    grid.Children.Add(statusBar);
                    Grid.SetRow(statusBar, 2);
                }
                else
                {
                    statusBar.Visibility = Visibility.Visible;
                }
            }
        }

        public void HideProgressBar()
        {
            if (this.isOutput)
            {
                if (statusBar != null)
                {
                    statusBar.Visibility = Visibility.Collapsed;
                }
            }
        }

        public void SetProgressBarValue(double value)
        {
            if (progressBar != null)
            {
                if (value > 100)
                {
                    value = 100;
                }
                else if (value < 0)
                {
                    value = 0;
                }

                Duration duration = new Duration(TimeSpan.FromSeconds(0.5));
                progressBar.BeginAnimation(ProgressBar.ValueProperty, new DoubleAnimation(value, duration));
            }
        }

        public void SetStatusText(string text)
        {
            if (statusBarPrefixText != null)
            {
                statusBarPrefixText.Text = text;
            }
        }

        public void SetStatusSuffixText(string text)
        {
            if (statusBarSuffixText != null)
            {
                statusBarSuffixText.Text = text;
            }
        }

        private void TextEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (PyInputObjectKeyUp != null)
            {
                using (Py.GIL())
                {
                    PyInputObjectKeyUp.Invoke(e.Key.ToPython(), e.Key.ToString().ToPython());
                }
            }
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
            clicks.Add(name, OnClick);
            wrapPanel.Children.Add(button);
        }

        public void SetButtonClick(string name, PyObject OnClick)
        {
            clicks[name] = OnClick;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bu = (Button)sender;
            PyObject click = null;
            if (clicks.TryGetValue((string)bu.Content, out click))
            {
                using (Py.GIL())
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
            }
            else
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
            if (isOutput)
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
                    throw;
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

            //double sum = 0;
            //foreach (Run i in p.Inlines)
            //{
            //    sum += GetStringActuallyWidth(i);
            //}

            richTextbox.MinPageWidth = GetStringActuallyWidth(r) + fontsize;

            // 无论输出端干了啥，必须得滚到最下面
            // richTextbox.ScrollToEnd();
        }

        public void AppendInputString(string s, string color = null, string fonfamily = null, double fontsize = 0)
        {
            this.textEditor.Text = s;
        }

        private List<Tuple<int, string, string, string, double>> tuples = new List<Tuple<int, string, string, string, double>>(maxLines * maxBuffer);
        public void AppendString(string s, string color = null, string fonfamily = null, double fontsize = 0)
        {
            if (this.isOutput)
            {
                // this.AppendOutputString(s, color, fonfamily, fontsize);
                if (this.isOutput)
                {
                    if (timer == null)
                    {
                        timer = new DispatcherTimer();
                        // 16.6 ms 驱动一次 可以包装60帧
                        timer.Interval = new TimeSpan(166000);
                        timer.Tick += new EventHandler(TimerDriveOutput);
                        timer.Start();
                    }
                    tuples.Add(new Tuple<int, string, string, string, double>(0, s, color, fonfamily, fontsize));
                }
            }
            else
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

        private int countLines = 0;

        readonly static int maxLines = 300;
        readonly static int maxBuffer = 4;

        private DispatcherTimer timer;
        public void AppendLine(string s, string color = null, string fonfamily = null, double fontsize = 0)
        {
            if (this.isOutput)
            {
                if (timer == null)
                {
                    timer = new DispatcherTimer();
                    // 1 ms 驱动一次 可以100帧
                    timer.Tag = new Action<string, string, string, double>(_AppendLine);
                    timer.Interval = new TimeSpan(100000);
                    timer.Tick += new EventHandler(TimerDriveOutput);
                    timer.Start();
                }
                tuples.Add(new Tuple<int, string, string, string, double>(1, s, color, fonfamily, fontsize));
            }
        }

        private void TimerDriveOutput(object sender, EventArgs e)
        {
            if (tuples.Count == 0)
            {
                return;
            }
            else if (tuples.Count > maxLines * (maxBuffer - 1))
            {
                tuples.RemoveRange(0, tuples.Count - maxLines * (maxBuffer - 1));
            }
            int count = 0;
            foreach (var item in tuples)
            {
                if (count >= 10)
                {
                    break;
                }
                if (item.Item1 == 0)
                {
                    AppendOutputString(item.Item2, item.Item3, item.Item4, item.Item5);
                }
                else
                {
                    _AppendLine(item.Item2, item.Item3, item.Item4, item.Item5);
                }
                count += 1;
            }

            tuples.RemoveRange(0, count);
        }

        public void _AppendLine(string s, string color = null, string fonfamily = null, double fontsize = 0)
        {
            IORichTextBox richTextbox = this.richTextbox;
            Paragraph lastBlock = (Paragraph)this.richTextbox.Document.Blocks.LastBlock;
            Run item = new Run(s);
            if (color != null)
            {
                try
                {
                    item.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                }
                catch (FormatException)
                {
                    Console.WriteLine("错误的颜色字符串");
                }
            }
            if (fontsize == 0.0)
            {
                fontsize = richTextbox.FontSize;
            }
            item.FontSize = fontsize;
            if (fonfamily != null)
            {
                item.FontFamily = new FontFamily(fonfamily);
            }
            lastBlock.Inlines.Add(item);
            this.countLines++;
            if (this.countLines == maxLines)
            {
                this.richTextbox.Document.Blocks.Add(new Paragraph());
                this.countLines = 0;
                if (this.richTextbox.Document.Blocks.Count >= maxBuffer)
                {
                    this.richTextbox.Document.Blocks.Remove(this.richTextbox.Document.Blocks.FirstBlock);
                }
            }
            richTextbox.MinPageWidth = GetStringActuallyWidth(item) + fontsize;
            richTextbox.ScrollToEnd();
        }


        public void QuickAnnotation()
        {
            string text = textEditor.SelectedText;
            string anotText = "";
            StringReader strReader = new StringReader(text);
            while (true)
            {
                var aLine = strReader.ReadLine();
                if (aLine != null)
                {
                    anotText += ("#" + aLine);
                    anotText += "\n";
                }
                else
                {
                    break;
                }
            }
            textEditor.SelectedText = anotText.TrimEnd('\n');
        }

        public void QuickAntiAnnotation()
        {
            string text = textEditor.SelectedText;
            string anotText = "";
            StringReader strReader = new StringReader(text);
            while (true)
            {
                var aLine = strReader.ReadLine();
                if (aLine != null && aLine.Trim().StartsWith("#"))
                {
                    anotText += aLine.Trim().Substring(1);
                    anotText += "\n";
                }
                else
                {
                    break;
                }
            }
            anotText = anotText.TrimEnd('\n');
            if (anotText != "")
            {
                textEditor.SelectedText = anotText;
            }
        }

        public string GetText()
        {
            if (this.isOutput)
            {
                TextRange textRange = new TextRange(richTextbox.Document.ContentStart, richTextbox.Document.ContentEnd);
                return textRange.Text;
            }
            else
            {
                return textEditor.Text;
            }
        }

        public void Clear()
        {
            if (this.isOutput)
            {
                IORichTextBox box = richTextbox;
                box.Document.Blocks.Clear();
                box.Document.Blocks.Add(new Paragraph());
                box.MinPageWidth = 0.0f;
            }
            else
            {
                textEditor.Clear();
            }
        }

        public bool IsCurrent()
        {
            var control = this.Parent as IOTabControl;
            if (control.SelectedIndex == -1)
                return false;
            return this.IsSelected;
        }

        public void SetRichBoxKeyDownEvent(PyObject pyKeyDown)
        {
            if (this.isOutput)
            {
                richTextbox.PyKeyDown = pyKeyDown;
            }
            else
            {
                PyInputObject = pyKeyDown;
            }
        }

        public void SetRichBoxKeyUpEvent(PyObject pyKeyUp)
        {
            if (this.isOutput)
            {
                // richTextbox.PyKeyDown = pyKeyDown;
            }
            else
            {
                PyInputObjectKeyUp = pyKeyUp;
            }
        }

        public void Selected()
        {
            if (PyOnSelected != null)
            {
                using (Py.GIL())
                {
                    PyOnSelected.Invoke();
                }
            }
        }

        public void SetSyntaxHighlighting(string format)
        {
            if (textEditor != null)
            {
                var typeConverter = new HighlightingDefinitionTypeConverter();
                textEditor.SyntaxHighlighting = (IHighlightingDefinition)typeConverter.ConvertFrom(format);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Source == this)
            {
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

        public int GetLineNumber()
        {
            if (textEditor != null)
            {
                return textEditor.TextArea.Caret.Line;
            }
            return -1;
        }

        public string GetAboveText()
        {
            int offset = textEditor.Document.GetOffset(GetLineNumber(), 0);
            return textEditor.TextArea.Document.GetText(0, offset);
        }

        public string GetCurrentLineBefore()
        {
            int start = textEditor.Document.GetOffset(GetLineNumber(), 0);
            int end = textEditor.TextArea.Caret.Column == 0 ? 0 : textEditor.TextArea.Caret.Column - 1;
            return textEditor.Document.GetText(start, end);
        }

        public string GetLineString(int line)
        {
            if (line > 0 && textEditor.Document.LineCount >= line)
            {
                int start = textEditor.Document.GetOffset(line, 0);
                int end = textEditor.Document.GetLineByNumber(line).Length;
                return textEditor.Document.GetText(start, end);
            }
            return "";
        }

        public int GetLineCusorPos()
        {
            return textEditor.TextArea.Caret.Column;
        }

        private void TextEditor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (PyMouseDown != null)
            {
                using (Py.GIL())
                {
                    PyMouseDown.Invoke();
                }
            }
        }

        private void TextArea_TextPasted(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText) return;
            var text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;
            TextPythonEntered(text);
        }

        private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            TextPythonEntered(e.Text);
        }

        void TextPythonEntered(string text)
        {
            if (PyTextEntered != null)
            {
                List<string> list = new List<string>();
                using (Py.GIL())
                {
                    PyTextEntered.Invoke(text.ToPython());
                    if (PyComDataList != null)
                    {
                        foreach (var item in PyComDataList)
                        {
                            list.Add(item.ToString());
                        }
                    }
                }

                completionWindow = new CompletionWindow(textEditor.TextArea);
                completionWindow.ResizeMode = ResizeMode.NoResize;
                var data = completionWindow.CompletionList.CompletionData;
                int len;
                if (list.Count > 0 && int.TryParse(list[0], out len))
                {
                    list.RemoveAt(0);
                    foreach (var item in list)
                    {
                        data.Add(new IOCompletionData(item, len));
                    }
                    if (data.Count > 0)
                    {
                        completionWindow.Show();
                        completionWindow.Closed += delegate
                        {
                            completionWindow = null;
                        };
                    }
                }
            }
        }

        public RelayCommand ClickCloseBtnCommand { get; set; }
        private IOMenuItem showIoMenuItem;

        private void collapseItem()
        {
            IOTabControl control = this.Parent as IOTabControl;
            this.Visibility = Visibility.Collapsed;
            var content = this.Content as UIElement;
            content.Visibility = Visibility.Collapsed;

            if (this.IsSelected)
            {
                control.SelectedIndex = -1;
                foreach (IOTabItem item in control.Items)
                {
                    if (item.Visibility == Visibility.Visible)
                    {
                        control.SelectedItem = item;
                        break;
                    }
                }

            }
            var window = WinIOAPP.Current.MainWindow as WinIO.WPF.MainWindow; 
            showIoMenuItem = window.CreatePanelMenuItem(this.Header, this);
        }

        public void AfterSetHeader()
        {
            if (showIoMenuItem != null)
            {
                showIoMenuItem.Header = this.Header;
            }
        }

        public void AfterClose()
        {
            if (showIoMenuItem != null)
            {
                var menu = showIoMenuItem.Parent as Menu;
                menu.Items.Remove(showIoMenuItem);
            }
        }
    }

   
}
