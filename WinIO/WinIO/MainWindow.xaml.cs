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

namespace WinIO
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private List<TabControl> tabs = new List<TabControl>();

        private TabItem CreateTabItem(string name)
        {
            TextBox textBox = new TextBox();
            textBox.Style = (Style)Resources["OuputTextBox"];
            Grid grid = new Grid();
            grid.Children.Add(textBox);
            TabItem tabItem = new TabItem();
            tabItem.Content = grid;
            tabItem.Header = name;
            return tabItem;
        }

        public void CreateOutputTabPanel(uint tabPanel, string name)
        {
            if(tabs.Count > tabPanel)
            {
                tabs[(int)tabPanel].Items.Add(CreateTabItem(name));
            }
        }

        public void CloseOutputTabPanel(uint tabPanel)
        {
            if (tabs.Count > tabPanel)
            {
                tabs[(int)tabPanel].Items.Clear();
            }
        }

        public void CloseOutputTermminal(uint tabPanel, string name)
        {
            if (tabs.Count > tabPanel)
            {
                var tab = tabs[(int)tabPanel];
                var item = tab.Items.Cast<TabItem>().First(n => n.Name == name);
                tab.Items.Remove(item);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Init();
            
            CreateOutputTabPanel(1, "test");
        }

        private void Init()
        {
            tabs.Add(TabControl0);
            tabs.Add(TabControl1);
            tabs.Add(TabControl2);
        }
    }
}
