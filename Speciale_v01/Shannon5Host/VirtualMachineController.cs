using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shannon5Host
{
    class VirtualMachineController
    {
        //Creates a process for the commandopromt
        private static Process cmd = new Process();

        //A function to power off the virtual machine
        public void poweroffVirtualMachine(string machineName)
        {
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"""C:\Program Files\Oracle\VirtualBox\VBoxManage.exe"" controlvm " + machineName + " poweroff");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
        }

        //A function to restore the virtual machine
        public void restoreVirtualMachine(string machineName, string snapshotName)
        {
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"""C:\Program Files\Oracle\VirtualBox\VBoxManage.exe"" snapshot " + machineName + " restore " + snapshotName);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
        }

        //A function to start the virtual machine
        public void startVirtualMachine(string machineName)
        {
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"""C:\Program Files\Oracle\VirtualBox\VBoxManage.exe"" startvm " + machineName);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
        }
    }
}
