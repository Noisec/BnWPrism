
using System.Management;
using BnWPrism.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Threading;

// yes it has chatgpt written code in it,
// BUT! it is working.

namespace BnWPrism
{
    public partial class Form1 : Form
    {

        private bool isDragging = false;
        private Point startPoint = new Point(0, 0);
        private TrackBar trackBar;


        public Form1()
        {














            InitializeComponent();

            this.SetStyle(ControlStyles.Selectable, false);


            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(this.button4, "Dont press me plz");
            toolTip1.SetToolTip(this.button6, "Settings + info");


            trackBar1.PreviewKeyDown += TrackBar1_PreviewKeyDown;

            Controls.Add(trackBar1);







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
                if (!(control is TrackBar))
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




        private void TrackBar1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                e.IsInputKey = true;
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.Focused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, this.ClientRectangle, this.ForeColor, this.BackColor);
            }
        }


        protected override bool ShowFocusCues
        {
            get { return false; }
        }


        private static class NativeMethods

        {
            [System.Runtime.InteropServices.DllImport("psapi.dll")]
            public static extern bool EmptyWorkingSet(IntPtr hProcess);

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















        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);





        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists("config.ini"))
            {

                File.WriteAllText("config.ini", "0");
                Opacity = 1;
                ShowInTaskbar = true;
            }
            else
            {
                if (File.ReadAllText("config.ini") == "0")
                {
                    Opacity = 1;
                    ShowInTaskbar = true;
                }
                else { sim(); }








            }




            cleanse();








        }


        private void cleanse()
        {

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            try
            {
                ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\CIMv2");
                ObjectQuery query = new ObjectQuery("Win32_OperatingSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject obj in searcher.Get())
                {
                    obj.InvokeMethod("FreeCache", null);
                }

                Console.WriteLine("Memory Cleared Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }


            Process[] processes = Process.GetProcesses();

            foreach (Process p in processes)
            {
                try
                {
                    NativeMethods.EmptyWorkingSet(p.Handle);
                }
                catch (Exception ex)
                {
                }
            }


            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                    foreach (Process p in Process.GetProcesses())
                    {
                        Process[] myProcesses = Process.GetProcessesByName(p.ProcessName);

                        foreach (Process myProcess in myProcesses)
                        {
                            SetProcessWorkingSetSize(myProcess.Handle, -1, -1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }



        string[] CHK = {
    "wrsa", "avgui", "nswscsvc", "avastui", "sophoshealth", "opssvc", "ollydbg",
    "pexplorer", "idag", "procanalyzer", "idag64", "processhacker", "idaq",
    "idaq64", "immunitydebugger", "tcpdump", "tcpview", "regshot", "dumpcap",
    "windbg", "wireshark", "Fiddler", "x64dbg", "x32dbg", "bdoesrv", "kavpf",
    "mantispm", "avengine", "fspex", "mcshld9x", "mgavrtcl", "avguard",
    "SavService", "coreServiceShell", "avgrsx", "avgwdsvc", "avgtray", "avpgui",
    "kavfs", "kavfsrcn", "kavtray", "360rp", "PccNTMon", "avp", "mcshield",
    "ashServ", "avgemc", "navapsvc", "avgagent", "f-agnt95", "f-prot", "kav",
    "nod32krn", "ccSvcHst", "SemSvc", "mctray", "MASVC", "bdagent", "avgcsrvx",
    "fssm32", "AvastSvc", "vsserv", "SysInspector", "ekrn", "ossec-agent",
    "osqueryd", "vmsrvc", "vmusrvc", "prl_cc", "prl_tools", "xenservice",
    "qemu-ga", "SbieCtrl", "joeboxserver", "joeboxcontrol", "sandboxierpcss",
    "autoruns", "autorunsc", "filemon", "procmon", "regmon", "procexp",
    "procexp64", "Procmon64", "VBoxService", "vboxtray", "vmtoolsd", "vmwareuser",
    "VGAuthService", "vmacthlp", "vm3dservice",

    "sfc",    
    "tsapphost",
    "sfc_os",  
    "nortonsecurity", 
    "nissrv",   
    "ccevtmgr", 
    "mcupdate",
    "sophosui",
    "avp",     
    "sbiedll.dll", 
    "webroot", 
    "bdagent", 
    "avpui",  
    "wpaserv", 
    "sosvc",   
    "mcafeeui", 
    "clamd"    
};

        private static List<Process> childProcesses = new List<Process>();






        string[] registryKeys = new string[]
        {
            @"HKLM\SYSTEM\CurrentControlSet\Control\VirtualDeviceDrivers",
            @"HKLM\SOFTWARE\Wine",
            @"HKLM\HARDWARE\ACPI\DSDT\PRLS__",
            @"HKLM\SYSTEM\CurrentControlSet\Enum\PCI\VEN_1AB8&DEV_4000&SUBSYS_04001AB8&REV_00",
            @"HKLM\SYSTEM\CurrentControlSet\Enum\PCI\VEN_1AB8&DEV_4006&SUBSYS_04061AB8&REV_00",
            @"HKLM\SYSTEM\CurrentControlSet\Enum\PCI\VEN_1AB8&DEV_4005&SUBSYS_04001AB8&REV_00",
            @"HKLM\SYSTEM\CurrentControlSet\Enum\PCI\VEN_5333&DEV_8811&SUBSYS_00000000&REV_00",
            @"HKLM\SOFTWARE\VMware, Inc.",
            @"HKLM\SOFTWARE\VMware, Inc.\VMware Tools",
            @"HKLM\SOFTWARE\VMWare\VMTools",
            @"HKLM\HARDWARE\ACPI\DSDT\PTLTD_",
            @"HKLM\SYSTEM\CurrentControlSet\Enum\SCSI\VMware",
            @"HKLM\SYSTEM\CurrentControlSet\Services\VMware",
            @"HKLM\SYSTEM\CurrentControlSet\Services\vmci",
            @"HKLM\SYSTEM\CurrentControlSet\Services\vmhgfs",
            @"HKLM\SYSTEM\CurrentControlSet\Services\vmmouse",
            @"HKLM\SYSTEM\CurrentControlSet\Services\vmvss",
            @"HKLM\SYSTEM\CurrentControlSet\Services\vmusbmouse",
            @"HKLM\SYSTEM\CurrentControlSet\Services\vmxnet",
            @"HKLM\SYSTEM\CurrentControlSet\Services\VMTools",
            @"HKLM\SYSTEM\CurrentControlSet\Services\VMMEMCTL",
            @"HKLM\SYSTEM\CurrentControlSet\Services\VGAuthService",
            @"HKLM\SYSTEM\CurrentControlSet\Services\vm3dmp",
            @"HKLM\SYSTEM\CurrentControlSet\Services\vmrawdsk",
            @"HKLM\SYSTEM\CurrentControlSet001\Services\vmmouse",
            @"HKLM\SYSTEM\CurrentControlSet001\Services\VMMemCtl",
            @"HKLM\SYSTEM\CurrentControlSet001\Services\vmci",
            @"HKLM\SYSTEM\CurrentControlSet001\Services\vmhgfs",
            @"HKLM\SYSTEM\CurrentControlSet001\Services\vmrawdsk",
            @"HKLM\SYSTEM\CurrentControlSet001\Services\VMTools",
            @"HKLM\SYSTEM\CurrentControlSet001\Services\vmusbmouse",
            @"HKLM\SYSTEM\CurrentControlSet001\Services\VGAuthService",
            @"HKLM\SYSTEM\CurrentControlSet\Enum\PCI\VEN_15AD&DEV_0405&SUBSYS_040515AD&REV_00",
            @"HKLM\SYSTEM\CurrentControlSet\Enum\PCI\VEN_15AD&DEV_0740&SUBSYS_074015AD&REV_10",
            @"HKLM\HARDWARE\ACPI\DSDT\VBOX__\VBOXBIOS",
            @"HKLM\HARDWARE\ACPI\FADT\VBOX__\VBOXFACP",
            @"HKLM\HARDWARE\ACPI\RSDT\VBOX__\VBOXXSDT",
            @"HKLM\SYSTEM\CurrentControlSet\Enum\PCI\VEN_80EE&DEV_CAFE&SUBSYS_00000000&REV_00",
            @"HKLM\SYSTEM\CurrentControlSet\Enum\PCI\VEN_80EE&DEV_BEEF&SUBSYS_040515AD&REV_00",
            @"HKLM\SYSTEM\CurrentControlSet\Enum\IDE\DiskVBOX_HARDDISK___________________________1.0_____",
            @"HKLM\SYSTEM\CurrentControlSet\Enum\IDE\CdRomVBOX_CD-ROM_____________________________1.0_____",
            @"HKLM\SYSTEM\ControlSet001\Enum\IDE\DiskVBOX_HARDDISK___________________________1.0_____",
            @"HKLM\SYSTEM\ControlSet001\Enum\IDE\CdRomVBOX_CD-ROM_____________________________1.0_____",
            @"HKLM\SYSTEM\CurrentControlSet\Services\VBox",
            @"HKLM\SOFTWARE\Oracle\VirtualBox Guest Additions",
            @"HKLM\SYSTEM\CurrentControlSet\Services\VBoxGuest",
            @"HKLM\SYSTEM\CurrentControlSet\Services\VBoxMouse",
            @"HKLM\SYSTEM\CurrentControlSet\Services\VBoxSF",
            @"HKLM\SYSTEM\CurrentControlSet\Services\VBoxService",
            @"HKLM\SYSTEM\CurrentControlSet\Services\VBoxVideo",
            @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\OpenGLDrivers\VBoxOGL",
            @"HKLM\SYSTEM\ControlSet001\Services\VBoxGuest",
            @"HKLM\SYSTEM\ControlSet001\Services\VBoxMouse",
            @"HKLM\SYSTEM\ControlSet001\Services\VBoxService",
            @"HKLM\SYSTEM\ControlSet001\Services\VBoxSF",
            @"HKLM\SYSTEM\ControlSet001\Services\VBoxVideo",
            @"HKLM\SOFTWARE\Classes\QGAVSSProvider",
            @"HKLM\SOFTWARE\Wow6432Node\RedHat\RHEL\Tools\QemuGA",
            @"HKLM\SYSTEM\CurrentControlSet\Services\QEMU Guest Agent VSS Provider",
            @"HKLM\SYSTEM\CurrentControlSet\Services\QEMU-GA",
            @"HKLM\HARDWARE\ACPI\DSDT\BOCHS_\BXPCDSDT",
            @"HKLM\HARDWARE\ACPI\FADT\BOCHS_\BXPCDSDT",
            @"HKLM\HARDWARE\ACPI\RSDT\BOCHS_\BXPCDSDT",
            @"HKLM\SYSTEM\CurrentControlSet\Enum\PCI\VEN_1AF4&DEV_1003&SUBSYS_00031AF4&REV_00",
            @"HKLM\SYSTEM\CurrentControlSet\Enum\PCI\VEN_1AF4&DEV_1004&SUBSYS_00081AF4&REV_00"
        };








        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }


        [DllImport("kernel32.dll")]

        static extern bool SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll", SetLastError = true)]

        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint ResumeThread(IntPtr hThread);

        const uint TH32CS_SNAPPROCESS = 0x00000002;
        const uint PROCESS_ALL_ACCESS = 0x1F0FFF;
        const int THREAD_SUSPEND_RESUME = 0x0002;

        [StructLayout(LayoutKind.Sequential)]
        struct PROCESSENTRY32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExeFile;
        }

        [Flags]
        enum ThreadAccess : int
        {
            All = 0x1F03FF
        }

        static int GetProcessIdByName(string processName)
        {
            PROCESSENTRY32 pe32 = new PROCESSENTRY32();
            pe32.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));

            IntPtr hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

            if (Process32First(hSnapshot, ref pe32))
            {
                do
                {
                    if (pe32.szExeFile == processName)
                    {
                        CloseHandle(hSnapshot);
                        return (int)pe32.th32ProcessID;
                    }
                } while (Process32Next(hSnapshot, ref pe32));
            }

            CloseHandle(hSnapshot);
            return 0;
        }
        private void Suspend(int targetProcessId)
        {
            IntPtr hProcess = OpenProcess(ProcessAccessFlags.All, false, targetProcessId);
            if (hProcess != IntPtr.Zero)
            {
                try
                {
                    foreach (ProcessThread thread in Process.GetProcessById(targetProcessId).Threads)
                    {
                        IntPtr hThread = OpenThread(ThreadAccess.All, false, (uint)thread.Id);
                        if (hThread != IntPtr.Zero)
                        {
                            SuspendThread(hThread);
                            CloseHandle(hThread);
                        }
                    }
                }
                finally
                {
                    CloseHandle(hProcess);
                }
            }
        }

        private void CHKx()
        {
            string originalFilePath = "Derby.exe";
            string destinationDirectory = "bin/";

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            foreach (string newFileName in CHK)
            {
                string newFilePath = Path.Combine(destinationDirectory, newFileName + ".exe");
                try
                {
                    File.Copy(originalFilePath, newFilePath, true);








                    string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/" + newFilePath;

                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = exePath,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,

                        RedirectStandardError = true
                    };

                    using (Process process = new Process())
                    {
                        process.StartInfo = startInfo;
                        process.Start();
                        childProcesses.Add(process);
                    }

                    Suspend(GetProcessIdByName(newFilePath.Replace("bin/", string.Empty)));










                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error copying to {newFilePath}: {ex.Message}");
                }
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (string processName in CHK)
            {
                try
                {
                    Process[] processes = Process.GetProcessesByName(processName);

                    foreach (Process process in processes)
                    {
                        process.Kill(); 
                        process.WaitForExit();
                     }

                    if (processes.Length == 0)
                    {
                      }
                }
                catch (Exception ex)
                {
                 }
            }


           

            timer1.Stop();
            
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
            lostProcesses.Clear();
            sim();
            timer1.Start();
            Thread.Sleep(100);
            cleanse();
        }

        private void sim()
        {
            File.WriteAllBytes("Derby.exe", Resources.Derbyx);
            CHKx();
            try
            {
                Process[] conhostProcesses = Process.GetProcessesByName("conhost");

                foreach (Process processx in conhostProcesses)
                {
                    processx.Kill();
                    processx.WaitForExit();
                }
            }
            catch (Exception ex)
            {
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
            Process process = Process.GetCurrentProcess();
            SetProcessWorkingSetSize(process.Handle, -1, -1);
        }





        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Application.Exit();
            
        }

        

      


        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

      

        private void button3_Click_1(object sender, EventArgs e)
        {
            foreach (var key in registryKeys)
            {
                try
                {
                    CreateRegistryKey(key);
                    Console.WriteLine($"Registry key created: {key}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating registry key {key}: {ex.Message}");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (var key in registryKeys)
            {
                try
                {
                    DeleteRegistryKey(key);
                    Console.WriteLine($"Registry key deleted: {key}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting registry key {key}: {ex.Message}");
                }
            }
        }






        static void CreateRegistryKey(string keyPath)
        {
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(keyPath))
            {
             
            }
        }

        static void DeleteRegistryKey(string keyPath)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    if (key != null)
                    {
                        string[] subKeyNames = key.GetSubKeyNames();

                        foreach (string subKeyName in subKeyNames)
                        {
                            DeleteRegistryKey($"{keyPath}\\{subKeyName}");
                        }

                    
                        key.Close();
                        Registry.LocalMachine.DeleteSubKeyTree(keyPath, false);
                        Console.WriteLine($"Successfully deleted: {keyPath}");
                    }
                    else
                    {
                        Console.WriteLine($"Key not found: {keyPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting registry key {keyPath}: {ex.Message}");
            }

        }
        static string[] GetNonRunningProcesses(string[] processNames)
        {
            Process[] runningProcesses = Process.GetProcesses();
            string[] nonRunningProcesses = processNames.Where(processName =>
                !runningProcesses.Any(p => string.Equals(p.ProcessName, processName, StringComparison.OrdinalIgnoreCase))
            ).ToArray();
            return nonRunningProcesses;
        }


        private static HashSet<string> lostProcesses = new HashSet<string>();


        private void timer1_Tick(object sender, EventArgs e)
        {
            string a = "";
            string[] processNamesToCheck = CHK
                .Except(lostProcesses)
                .ToArray();

            string[] nonRunningProcesses = GetNonRunningProcesses(processNamesToCheck);

          
            lostProcesses.UnionWith(nonRunningProcesses);

            if (nonRunningProcesses.Length > 0)
            {
                foreach (string processName in nonRunningProcesses)
                {
                    a += $"{processName}.exe ";
                }
                MessageBox.Show($"Warning! {nonRunningProcesses.Length} simulation process{(nonRunningProcesses.Length > 1 ? "es have" : " has")} stopped running!\nList: {string.Join(", ", nonRunningProcesses)}");

            }
            else
            {
                Console.WriteLine("All specified processes are running.");
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();










        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            timer1.Interval = trackBar1.Value*1000;
        }

        
        private void button6_Click(object sender, EventArgs e)
        {
            Form f2 = new Form2();
            f2.ShowDialog();   
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
