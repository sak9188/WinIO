﻿using ICSharpCode.AvalonEdit;
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
                var sty = (Style)Control.Resources[style];
                if(!sty.IsSealed)
                {
                    sty.BasedOn = textEditor.Style;
                }
                textEditor.Style = sty;
                textEditor.KeyDown += TextEditor_KeyDown;
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
        }

        public void AppendInputString(string s, string color = null, string fonfamily = null, double fontsize = 0)
        {
            this.textEditor.AppendText(s);
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
    }
}
