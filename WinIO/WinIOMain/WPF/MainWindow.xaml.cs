using Python.Runtime;
using SourceChord.FluentWPF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
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
        private Thread thread;

        private List<Window> windows = new List<Window>();

        private List<TabControl> tabs = new List<TabControl>();

        public IOMenuItem CreateTopMenuItem(string header)
        {
            IOMenuItem item = new IOMenuItem();
            item.Header = header;
            MainMenu.Items.Add(item);
            item.Click += MenuItem_Click;
            return item;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            var menuitem = (IOMenuItem)sender;
            if (menuitem.PyOnClick != null)
            {
                using(Py.GIL())
                {
                    menuitem.PyOnClick.Invoke();
                }
            }
        }

        public IOMenuItem CreateMenuItem(string header)
        {
            IOMenuItem item = new IOMenuItem();
            item.Header = header;
            item.Click += MenuItem_Click;
            return item;
        }


        private IOTabItem CreateOuputTextBox(string name)
        {
            return new IOTabItem(name, "OutputTextBox");
        }

        private IOTabItem CreateInputTextBox(string name)
        {
            return new IOTabItem(name, "InputTextBox", false);
        }

        public ExportPython.TabItem CreateOutputTab(uint tabPanel, string name)
        {
            if (tabs.Count > tabPanel)
            {
                var panel = tabs[(int)tabPanel];
                var item = CreateOuputTextBox(name);
                item.TabPanelID = (int)tabPanel;
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
                item.TabPanelID = (int)tabPanel;
                panel.Items.Add(item);
                if (panel.Items.Count == 1)
                {
                    panel.SelectedIndex = 0;
                }
                return item;
            }
            return null;
        }

        public void CloseTab(IOTabItem item)
        {
            var tab = tabs[item.TabPanelID];
            tab.Items.Remove(item);
        }

        public void CloseTabPanel(uint tabPanel)
        {
            if (tabs.Count > tabPanel)
            {
                tabs[(int)tabPanel].Items.Clear();
            }
        }

        public MainWindow(Thread thread)
        {
            this.thread = thread;
            Init();
        }

        public MainWindow(MainWindow mainWindow)
        {
            mainWindow.windows.Add(this);
            Init();
        }

        private ResourceDictionary GetResourceDictionaryByURI(string suri)
        {
            Uri uri = new Uri(suri);
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = uri;
            return rd;
        }

        private void Init()
        {
            InitializeComponent();

            tabs.Add(TabControl0);
            tabs.Add(TabControl1);
            tabs.Add(TabControl2);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tab = (TabControl)sender;
            var item = (IOTabItem)tab.Items[tab.SelectedIndex];
            item.Selected();
        }
    }
}
