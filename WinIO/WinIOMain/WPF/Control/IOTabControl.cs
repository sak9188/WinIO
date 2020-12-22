using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WinIO.WPF.Control
{
    public class IOTabControl : TabControl
    {
        public IOTabControl() : base()
        {
            this.SelectionChanged += IOTabControl_SelectionChanged;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            //检查数据是否包含制定字符串类型数据
            if (e.Data.GetDataPresent("IOTabItem"))
            {
                //获取字符串
                IOTabItem item = (IOTabItem)e.Data.GetData("IOTabItem");
                
                IOTabControl parent = (IOTabControl)item.Parent;
                if(parent != this)
                {
                    parent.Items.Remove(item);
                    this.Items.Add(item);
                    this.SelectedItem = item;
                    item.TabPanelID = ((MainWindow)WinIOAPP.Instance.MainWindow).GetTabIndex(this);
                }
            }
            e.Handled = true;
        }

        private void IOTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tab = (TabControl)sender;
            if (tab.SelectedIndex != -1)
            {
                var item = (IOTabItem)tab.Items[tab.SelectedIndex];
                item.Selected();
            }
        }
    }
}
