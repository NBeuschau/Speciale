using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickHostControl
{
    class VirtualMachineController
    {
        public static void poweroffVirtualMachine(string machineName)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"""C:\Program Files\Oracle\VirtualBox\VBoxManage.exe"" controlvm " + machineName + " poweroff");
            cmd.StandardInput.Flush();
        }

        public static void restoreVirtualMachine(string machineName, string snapshotName)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"""C:\Program Files\Oracle\VirtualBox\VBoxManage.exe"" snapshot " + machineName + " restore " + snapshotName);
            cmd.StandardInput.Flush();
        }

        public static void startVirtualMachine(string machineName)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"""C:\Program Files\Oracle\VirtualBox\VBoxManage.exe"" startvm " + machineName);
            cmd.StandardInput.Flush();
        }
    }
}
