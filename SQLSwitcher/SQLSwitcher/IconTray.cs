using SQLSwitcher.Properties;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Diagnostics;

namespace SQLSwitcher
{
    public class IconTray : ApplicationContext
    {
        private NotifyIcon notifyIcon;
        private ServiceController sc;
        private readonly bool IsChinese = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.StartsWith("zh");


        public IconTray()
        {
            sc = new ServiceController("OracleServiceORCL");
            notifyIcon = new NotifyIcon
            {
                Icon = sc.Status.Equals(ServiceControllerStatus.Stopped) ? Resources.server_stop : Resources.server_run,
                Text = Application.ProductName,
                Visible = true
            };

            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;

            var exit = new ToolStripMenuItem
            {
                Font = new System.Drawing.Font("Segoe UI", 9F),
                Text = IsChinese ? "退出" : "Exit"
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

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon.MouseDoubleClick -= NotifyIcon_MouseDoubleClick;
            Process.Start("schtasks", "/run /TN \"SQLSwitcher\"");

            if (sc.Status.Equals(ServiceControllerStatus.Stopped))
            {
                sc.WaitForStatus(ServiceControllerStatus.Running);
                notifyIcon.Icon = Resources.server_run;
                notifyIcon.Text = notifyIcon.BalloonTipText = IsChinese ? "SQL 服务正在运行" : "SQL Server is running!";
            }
            else
            {
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                notifyIcon.Icon = Resources.server_stop;
                notifyIcon.Text = notifyIcon.BalloonTipText = IsChinese ? "SQL 服务已停止" : "SQL Server has stopped!";
            }

            notifyIcon.ShowBalloonTip(3000);
            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        }
    }
}
