using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace homework_03
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Process current = Process.GetCurrentProcess();
            var runningProcess = Process.GetProcessesByName(current.ProcessName).FirstOrDefault(p => p.Id != current.Id);
            if (runningProcess == null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new homework_03.Form1(args));
            }
            else
            {
                var hwnd = runningProcess.MainWindowHandle;
                SetForegroundWindow(hwnd);
                var cd = new COPYDATASTRUCT();
                for (int i = 0; i < args.Length; i++)
                {
                    cd.lpData += args[i] + " ";
                }
                cd.cbData = cd.lpData.Length + 1;
                SendMessage(hwnd, WM_COPYDATA, 101, ref cd);
            }
        }
        public struct COPYDATASTRUCT
        {
            public uint dwData;
            public int cbData;
            public string lpData;
        }
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, uint wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, uint wParam, ref COPYDATASTRUCT lParam);
        public const int WM_COMMAND = 0x0111;
        public const int WM_COPYDATA = 74;
    }
}
