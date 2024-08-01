using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace BnWPrism
{
    public partial class Form2 : Form
    {

        private bool isDragging = false;
        private Point startPoint = new Point(0, 0);
        private RichTextBox richTextBox;

        public Form2()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.Selectable, false);
           

            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(this.pictureBox1, "For suggestions and questions");


            Cursor customCursor = new Cursor(new MemoryStream(Properties.Resources.Windows_XP_3D_Black_Normal));

            this.Cursor = customCursor;

            SetCursorForAllControls(this, customCursor);


            AttachMouseEvents(this);
        }


        private void SetCursorForAllControls(Control parent, Cursor cursor)
        {
            foreach (Control control in parent.Controls)
            {
                control.Cursor = cursor;
                SetCursorForAllControls(control, cursor);
            }
        }
        private void AttachMouseEvents(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (!(control is RichTextBox))
                {
                    control.MouseDown += Control_MouseDown;
                    control.MouseMove += Control_MouseMove;
                    control.MouseUp += Control_MouseUp;
                }
                AttachMouseEvents(control);
            }
        }
        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                startPoint = new Point(e.X, e.Y);
            }
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - startPoint.X, p.Y - startPoint.Y);
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }



       
        private static class NativeMethods
        {
            public const int GWL_WNDPROC = -4;
            public const uint WM_SETFOCUS = 0x0007;
            public const uint WM_KILLFOCUS = 0x0008;

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll")]
            public static extern IntPtr DefWindowProc(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

            public delegate IntPtr WindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        }







        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
            Process.Start("https://discord.gg/8wpcbmkM");
            
        }


        public void cmdx(string command)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = startInfo })
                {
                    process.Start();




                }
            }
            catch (Exception ex)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try {

                File.Delete("Derby.exe");

                Directory.Delete("bin", true);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) { File.WriteAllText("config.ini", "1"); }
            else { File.WriteAllText("config.ini", "0"); }
        }

    

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_DoubleClick(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Noisec/BnWPrism");
        }

        private void richTextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Process.Start("https://github.com/Noisec/BnWPrism");
        }
    }
}
