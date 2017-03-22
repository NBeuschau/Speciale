using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speciale_v01.TestEnvironmentLogger
{
    class VirtualMachineController
    {
        public static void poweroffVirtualMashine(string mashineName)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"C:\Program Files\Oracles\VirtualBox\VBoxManage.exe controlvm" + mashineName +  " poweroff");
            cmd.StandardInput.Flush();
        }

        public static void restoreVirtualMashine(string mashineName, string snapshotName)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"C:\Program Files\Oracles\VirtualBox\VBoxManage.exe snapshot" + mashineName + " restore " + snapshotName);
            cmd.StandardInput.Flush();
        }

        public static void startVirtualMashine(string mashineName)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"C:\Program Files\Oracles\VirtualBox\VBoxManage.exe startvm" + mashineName);
            cmd.StandardInput.Flush();
        }
    }
}
