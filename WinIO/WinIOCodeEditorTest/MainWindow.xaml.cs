
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


namespace WinIOCodeEditorTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            textEditor.TextArea.TextView.Height = 100;
            textEditor.TextArea.TextView.Width = 100;

            Console.WriteLine(textEditor.TextArea.TextView);
        }
    }
}
