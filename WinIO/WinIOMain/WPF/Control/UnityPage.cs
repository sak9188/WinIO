//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Interop;

//namespace WinIO.WPF.Control
//{
//    public class UnityPage : Page
//    {
//        [DllImport("User32.dll")]
//        static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

//        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
//        [DllImport("user32.dll")]
//        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

//        [DllImport("user32.dll")]
//        static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

//        private Process process;
//        private IntPtr unityHWND = IntPtr.Zero;

//        private const int WM_ACTIVATE = 0x0006;
//        private readonly IntPtr WA_ACTIVE = new IntPtr(1);
//        private readonly IntPtr WA_INACTIVE = new IntPtr(0);

//        bool initialized = false;

//        MainWindow mainWindow;

//        public UnityPage(MainWindow pwindow)
//        {
//            mainWindow = pwindow;
//            mainWindow.Closed += Application_Exit;
//            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
//            dispatcherTimer.Tick += attemptInit;
//            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
//            dispatcherTimer.Start();

//        }

//        void attemptInit(object sender, EventArgs e)
//        {

//            if (initialized)
//                return;

//            //HwndSource source = (HwndSource)HwndSource.FromVisual(mainWindow);

//            //Console.WriteLine("attempting to get handle...");

//            //if (source == null)
//            //{
//            //    Console.WriteLine("Failed to get handle source");
//            //    return;
//            //}

//            //IntPtr hWnd = source.Handle;

//            try
//            {
//                process = new Process();
//                process.StartInfo.FileName = @"D:\Project\Dev\ZTool\FairyGUI-Editor\FairyGUI-Editor.exe";
//                process.StartInfo.Arguments = "-parentHWND " + mainWindow.Handle.ToInt32() + " " + Environment.CommandLine;
//                process.StartInfo.UseShellExecute = true;
//                process.StartInfo.CreateNoWindow = true;

//                process.Start();

//                process.WaitForInputIdle();
//                // Doesn't work for some reason ?!
//                //unityHWND = process.MainWindowHandle;
//                EnumChildWindows(mainWindow.Handle.ToInt32(), WindowEnum, IntPtr.Zero);

//                //unityHWNDLabel.Text = "Unity HWND: 0x" + unityHWND.ToString("X8");
//                Console.WriteLine("Unity HWND: 0x" + unityHWND.ToString("X8"));

//                panel1_Resize(this, EventArgs.Empty);

//                initialized = true;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message + ".\nCheck if Container.exe is placed next to UnityGame.exe.");
//            }
//        }

//        private void ActivateUnityWindow()
//        {
//            SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
//        }

//        private void DeactivateUnityWindow()
//        {
//            SendMessage(unityHWND, WM_ACTIVATE, WA_INACTIVE, IntPtr.Zero);
//        }

//        private int WindowEnum(IntPtr hwnd, IntPtr lparam)
//        {
//            unityHWND = hwnd;
//            ActivateUnityWindow();
//            return 0;
//        }

//        private void panel1_Resize(object sender, EventArgs e)
//        {
//            MoveWindow(unityHWND, 0, 0, (int)mainWindow.Width - 100, (int)mainWindow.Height - 100, true);
//            Console.WriteLine("RESIZED UNITY WINDOW TO: " + (int)(mainWindow.Width) + "x" + (int)(mainWindow.Height));
//            ActivateUnityWindow();
//        }

//        // Close Unity application
//        private void Application_Exit(object sender, EventArgs e)
//        {
//            try
//            {
//                process.CloseMainWindow();

//                Thread.Sleep(1000);
//                while (!process.HasExited)
//                    process.Kill();
//            }
//            catch (Exception)
//            {

//            }
//        }

//        private void Form1_Activated(object sender, EventArgs e)
//        {
//            ActivateUnityWindow();
//        }

//        private void Form1_Deactivate(object sender, EventArgs e)
//        {
//            DeactivateUnityWindow();
//        }
//    }
//}
