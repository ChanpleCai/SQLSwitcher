using SQLSwitcher.Properties;
using System;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Diagnostics;
using System.IO;

namespace SQLSwitcher
{
    public class IconTray
    {
        private NotifyIcon notifyIcon;
        private ServiceController sc;


        public IconTray()
        {
            sc = new ServiceController("MSSQLSERVER");
            notifyIcon = new NotifyIcon
            {
                Icon = sc.Status.Equals(ServiceControllerStatus.Stopped) ? Resources.server_stop : Resources.server_run,
                Visible = true
            };

            notifyIcon.MouseDoubleClick += (s, e) =>
            {
                Process.Start(Path.Combine(Environment.CurrentDirectory, "Switcher"));
                if (sc.Status.Equals(ServiceControllerStatus.Stopped))
                {
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                    notifyIcon.Icon = Resources.server_run;
                }
                else
                {
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    notifyIcon.Icon = Resources.server_stop;
                }
            };
        }
    }
}
