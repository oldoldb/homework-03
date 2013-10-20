using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using MaxSumLibrary;

namespace homework_03
{
    public partial class Form1 : Form
    {
        private const int INF = 0x3f3f3f3f;
        private TabControl _tabs;
        private int cas = 0;
        Button[,] bt = null;
        int[,] map = null;
        int[,] sum = null;
        int[,] mark = null;
        int ans = -INF;
        int row;
        int col;
        int mode;
        public Form1()
        {
            InitializeComponent();
        }

        public Form1(string[] args)
        {
            this.Width = 800;
            this.Height = 600;
            InitializeComponent();
            _tabs = new TabControl();
            _tabs.Dock = DockStyle.Fill;
            Controls.Add(_tabs);

            var page = new TabPage("File0");
            page.Width = _tabs.Width;
            page.Height = Convert.ToInt32(_tabs.Height * 0.9);

            string path = calc(args);
            input(path);
            sum = new int[row, col];
            mark = new int[row, col];
            ans = -INF;

            MaxSum thismaxsum=new MaxSum(map,row,col,ref ans,ref mark,mode);
            DrawMaxSum(page);
            ShowMaxSum(page);
            _tabs.SelectedTab = page;
            _tabs.TabPages.Add(page);
        }

        public void DrawMaxSum(TabPage page)
        {
            int X = page.Width;
            int Y = page.Height;
            bt = new Button[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    bt[i, j] = new Button();
                    bt[i, j].AutoSize = false;
                    bt[i, j].Text = Convert.ToString(map[i, j]);
                    bt[i, j].SetBounds(j * X / col, i * Y / row, X / col, Y / row);
                    bt[i, j].Visible = true;
                    page.Controls.Add(bt[i, j]);
                }
            }
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (mark[i, j] == 1)
                    {
                        bt[i, j].BackColor = Color.Yellow;
                    }
                }
            }
        }

        public void ShowMaxSum(TabPage page)
        {
            StatusStrip sb = new StatusStrip();
            ToolStripLabel tsl = new ToolStripLabel();
            tsl.Text = "MaxSum is " + Convert.ToString(ans);
            ToolStripItem[] tsi = new ToolStripItem[1];
            tsi[0] = tsl;
            sb.Items.AddRange(tsi);
            page.Controls.Add(sb);
        }

        private void input(string path)
        {
            string[] stringArray;
            string line;
            char[] separator = new char[] { ',' };
            try
            {
                StreamReader sr = new StreamReader(path);
                line = sr.ReadLine();
                stringArray = line.Split(separator);
                row = Convert.ToInt32(stringArray[0]);
                line = sr.ReadLine();
                stringArray = line.Split(separator);
                col = Convert.ToInt32(stringArray[0]);
                map = new int[row, col];
                for (int i = 0; i < row; i++)
                {
                    line = sr.ReadLine();
                    stringArray = line.Split(separator);
                    for (int j = 0; j < col; j++)
                    {
                        map[i, j] = Convert.ToInt32(stringArray[j]);
                    }
                }
                sr.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("An IO exception has been thrown!");
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }

        private string calc(string[] args)
        {
            string path = null;
            int len = args.Length;
            if (len == 1)
            {
                mode = 1;
                path = args[0];
            }
            else if (len == 2)
            {
                if (args[0] == "/v")
                {
                    mode = 3;
                }
                else if (args[0] == "/h")
                {
                    mode = 2;
                }
                else if (args[0] == "/a")
                {
                    mode = 5;
                }
                path = args[1];
            }
            else if (len == 3)
            {
                if ((args[0] == "/v" && args[1] == "/h") || (args[0] == "/h" && args[1] == "/v"))
                {
                    mode = 4;
                    path = args[2];
                }
            }
            else if (len == 4)
            {
                if (((args[0] == "/v" && args[1] == "/h") || (args[0] == "/h" && args[1] == "/v")) && args[2] == "/a")
                {
                    mode = 6;
                    path = args[3];
                }
            }
            return path;
        }

        private string calc2(string str)
        {
            string path = null;
            int begin = 0;
            for (int i = str.Length - 2; i >= 0; i--)
            {
                if (str[i] == ' ')
                {
                    begin = i;
                    break;
                }
            }
            for (int i = begin; i < str.Length; i++)
            {
                path += str[i];
            }
            bool flagv = false, flagh = false, flaga = false;
            for (int i = 0; i < begin; i++)
            {
                if (str[i] == '/' && str[i + 1] == 'v')
                {
                    flagv = true; ;
                }
                if (str[i] == '/' && str[i + 1] == 'h')
                {
                    flagh = true;
                }
                if (str[i] == '/' && str[i + 1] == 'a')
                {
                    flaga = true;
                }
            }
            if (flagv && flagh && flaga)
            {
                mode = 6;
            }
            else if (flagv && flagh)
            {
                mode = 4;
            }
            else if (flagv)
            {
                mode = 3;
            }
            else if (flagh)
            {
                mode = 2;
            }
            else if (flaga)
            {
                mode = 5;
            }
            else
            {
                mode = 1;
            }
            return path;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Program.WM_COPYDATA && (uint)m.WParam == 101)
            {
                var cd = (Program.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(Program.COPYDATASTRUCT));
                string prevpath = cd.lpData;
                string path=calc2(prevpath);
                input(path);
                var page = new TabPage("File" + (++cas));
                page.Width = _tabs.Width;
                page.Height = Convert.ToInt32(_tabs.Height * 0.9);
                ans = -INF;
                MaxSum thismaxsum = new MaxSum(map, row, col, ref ans, ref mark,mode);
                DrawMaxSum(page);
                ShowMaxSum(page);
                _tabs.TabPages.Add(page);
                _tabs.SelectedTab = page;
                return;
            }
            base.WndProc(ref m);
        }
    }
}
