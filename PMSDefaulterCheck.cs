using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;

namespace PMSDefaulterCheck
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern IntPtr GetShellWindow();
        static void Main(string[] args)
        {
            IntPtr h = Process.GetCurrentProcess().MainWindowHandle;
            ShowWindow(h, 0);
            IntPtr shellWin = GetShellWindow();
            SetParent(h, shellWin);
            Microsoft.Win32.SystemEvents.SessionSwitch += new Microsoft.Win32.SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            Console.ReadLine();
        }

        static void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                //Console.WriteLine("I left my desk");
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                //Console.WriteLine("I returned to my desk");
                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                userName = userName.Substring(userName.IndexOf("\\")+1).ToLower();
                using (WebClient wc = new WebClient())
                {
                    var strUrlOutput = wc.DownloadString("https://shortbookingpms.mindeservices.com/api/ShortBooking?wnUser="+userName);
                    int startdelimeter = strUrlOutput.IndexOf('{');
                    int enddelimeter = strUrlOutput.IndexOf('}');
                    string strDefhrs = strUrlOutput.Substring(startdelimeter +1, enddelimeter - startdelimeter -1 ).Trim();
                    double defHours = Double.Parse(strDefhrs);
                    if (defHours >0)
                        {
                        DialogResult dlgResponse;
                        dlgResponse = MessageBox.Show("Your PMS booking is short of "+strDefhrs+ " hours. Kindly go to PMS and complete it first.", "PMS Defaulter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        System.Diagnostics.Process.Start("https://pms2019.mindeservices.com/");
                    }                   
                }
                
                //MessageBox.Show("You have been defaulting on PMS. \nPlease fill your PMS entries to stop seeing this message repeatedly.", "PMS Default", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
}
