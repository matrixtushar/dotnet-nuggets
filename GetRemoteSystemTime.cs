using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GetRemoteSystemTimeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string strMachineName = args[0];
                string strStdOut = "";
                Process pTask = new Process();
                pTask.StartInfo.UseShellExecute = false;
                pTask.StartInfo.RedirectStandardOutput = true;
                pTask.StartInfo.FileName = "net";
                pTask.StartInfo.Arguments = @"time \\" + strMachineName;

                pTask.Start();
                pTask.WaitForExit();

                strStdOut = pTask.StandardOutput.ReadLine();
                Console.WriteLine(strStdOut);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
