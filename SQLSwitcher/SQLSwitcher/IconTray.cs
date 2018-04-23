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
                    notifyIcon.BalloonTipText = "Sql Server is running!";
                    notifyIcon.ShowBalloonTip(5000);
                }
                else
                {
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    notifyIcon.Icon = Resources.server_stop;
                    notifyIcon.BalloonTipText = "SQL Server has stopped!";
                    notifyIcon.ShowBalloonTip(5000);
                }
            };

            var exit = new ToolStripMenuItem
            {
                Font = new System.Drawing.Font("Segoe UI", 9F),
                Text = "Exit"
            };

            exit.Click += (s, e) =>
            {
                notifyIcon.Dispose();
                sc.Dispose();
                Application.Exit();
            };

            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(exit);
            notifyIcon.ContextMenuStrip = contextMenuStrip;
        }
    }
}
