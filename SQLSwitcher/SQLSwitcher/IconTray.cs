using SQLSwitcher.Properties;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SQLSwitcher
{
    public class IconTray : ApplicationContext
    {
        private NotifyIcon _notifyIcon;
        private ServiceController sc;
        private readonly bool _isChinese = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.StartsWith("zh");


        public IconTray()
        {
            sc = new ServiceController("OracleServiceORCL");
            _notifyIcon = new NotifyIcon
            {
                Icon = sc.Status.Equals(ServiceControllerStatus.Stopped) ? Resources.server_stop : Resources.server_run,
                Text = Application.ProductName,
                Visible = true
            };

            _notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;

            var exit = new ToolStripMenuItem
            {
                Font = new System.Drawing.Font("Segoe UI", 9F),
                Text = _isChinese ? "退出" : "Exit"
            };

            exit.Click += (s, e) =>
            {
                _notifyIcon.Dispose();
                sc.Dispose();
                Application.Exit();
            };

            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(exit);
            _notifyIcon.ContextMenuStrip = contextMenuStrip;
        }

        private async void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _notifyIcon.MouseDoubleClick -= NotifyIcon_MouseDoubleClick;
            Process.Start("schtasks", "/run /TN \"SQLSwitcher\"");
            _notifyIcon.Icon = Resources.server_wait_32;
            _notifyIcon.Text = _notifyIcon.BalloonTipText = _isChinese ? "SQL 服务正在启动..." : "SQL Server is starting...";
            _notifyIcon.ShowBalloonTip(1000);
            if (sc.Status.Equals(ServiceControllerStatus.Stopped))
            {
                await Task.Run(() => sc.WaitForStatus(ServiceControllerStatus.Running));
                _notifyIcon.Icon = Resources.server_run;
                _notifyIcon.Text = _notifyIcon.BalloonTipText = _isChinese ? "SQL 服务正在运行" : "SQL Server is running!";
            }
            else
            {
                await Task.Run(() => sc.WaitForStatus(ServiceControllerStatus.Stopped));
                _notifyIcon.Icon = Resources.server_stop;
                _notifyIcon.Text = _notifyIcon.BalloonTipText = _isChinese ? "SQL 服务已停止" : "SQL Server has stopped!";
            }

            _notifyIcon.ShowBalloonTip(3000);
            _notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        }
    }
}
