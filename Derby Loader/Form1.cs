using Derby_Loader.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Derby_Loader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

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
        }
        string[] CHK = { "wrsa", "avgui", "nswscsvc", "avastui", "sophoshealth", "opssvc", "ollydbg", "pexplorer", "idag", "procanalyzer", "idag64", "processhacker", "idaq", "idaq64", "immunitydebugger", "tcpdump", "tcpview", "regshot", "dumpcap", "windbg", "wireshark", "Fiddler", "x64dbg", "x32dbg", "bdoesrv", "kavpf", "mantispm", "avengine", "fspex", "mcshld9x", "mgavrtcl", "avguard", "SavService", "coreServiceShell", "avgrsx", "avgwdsvc", "avgtray", "avpgui", "kavfs", "kavfsrcn", "kavtray", "360rp", "PccNTMon", "avp", "mcshield", "ashServ", "avgemc", "navapsvc", "avgagent", "f-agnt95", "f-prot", "kav", "nod32krn", "ccSvcHst", "SemSvc", "mctray", "MASVC", "bdagent", "avgcsrvx", "fssm32", "AvastSvc", "vsserv", "SysInspector", "ekrn", "ossec-agent", "osqueryd", "vmsrvc", "vmusrvc", "prl_cc", "prl_tools", "xenservice", "qemu-ga", "SbieCtrl", "joeboxserver", "joeboxcontrol", "sandboxierpcss", "tcpview", "autoruns", "autorunsc", "filemon", "procmon", "regmon", "procexp", "procexp64", "Procmon64", "VBoxService", "vboxtray", "vmtoolsd", "VBoxTray", "vmwareuser", "VGAuthService", "vmacthlp", "vm3dservice", "VBoxService" };

        private static List<Process> childProcesses = new List<Process>();















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


        // Import the required native Windows APIs
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


            foreach (var item in CHK)
            {
                cmdx($"TASKKILL /F /IM {item}.exe");
            }

            Application.Exit();
        }



        static void cmdx(string command)
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


        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        private void button1_Click(object sender, EventArgs e)
        {

            sim();
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) { File.WriteAllText("config.ini", "1"); }
            else { File.WriteAllText("config.ini", "0"); }

        }

      


        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Github page: https://github.com/Noisec/Derby <copied to clipboard>\nIf you check that one button, the next time you launch the app, it will not be visible and will auto start the simulation. To uncheck it, just delete the config.ini file.");
            Clipboard.SetText("https://github.com/Noisec/Derby");
        
    }
    }
}
