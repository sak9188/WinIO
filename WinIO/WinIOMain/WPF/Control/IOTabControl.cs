using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WinIO.WPF.Control
{
    public class IOTabControl : TabControl
    {
        public IOTabControl() : base()
        {
            this.SelectionChanged += IOTabControl_SelectionChanged;
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {

            base.OnGiveFeedback(e);

            
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
                }else
                {
                    // Point point = item.TranslatePoint(new Point(), parent);
                    // 获得鼠标下面的控件
                    Point dragPoint = e.GetPosition(parent);
                    IOTabItem hit = null;
                    VisualTreeHelper.HitTest(parent, null, new HitTestResultCallback(
                        (HitTestResult result) => {
                            if(result.VisualHit is Border && result.VisualHit.GetValue(NameProperty).ToString() == "TabItemBorder")
                            {
                                hit = (result.VisualHit as Border).TemplatedParent as IOTabItem;
                                return HitTestResultBehavior.Stop;
                            }
                            return HitTestResultBehavior.Continue; 
                        }),
                        new PointHitTestParameters(dragPoint));
                    if(hit != null)
                    {
                        int insertIndex = parent.Items.IndexOf(hit);
                        parent.Items.Remove(item);
                        parent.Items.Insert(insertIndex, item);
                        this.SelectedItem = item;
                    }
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
