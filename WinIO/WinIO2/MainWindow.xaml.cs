using Cosine.Windows.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinIO2
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            control.ItemsSource = item_list;
            item_list.Add("1123");
            item_list.Add("2342");
            item_list.Add("+");
        }

        public List<string> item_list = new List<string>();

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var newItem = new TabItem();
            control.Items.Insert(control.Items.Count - 1, newItem);
            control.SelectedIndex = control.Items.Count - 2;
            e.Handled = true;
        }
    }
}
