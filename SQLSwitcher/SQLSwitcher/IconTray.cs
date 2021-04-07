using System.Diagnostics;
using System.Drawing;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQLSwitcher.Properties;

namespace SQLSwitcher
{
    public class IconTray : ApplicationContext
    {
        private readonly bool _isChinese = Thread.CurrentThread.CurrentUICulture.Name.StartsWith("zh");
        private readonly NotifyIcon _notifyIcon;
        private readonly ServiceController _sc;


        public IconTray()
        {
            _sc = new ServiceController("MSSQLSERVER");
            _notifyIcon = new NotifyIcon
            {
                Icon = GetIcon(),
                Text = Application.ProductName,
                Visible = true
            };

            _notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
            _notifyIcon.MouseMove += (s, e) => { _notifyIcon.Icon = GetIcon(); };

            var exit = new ToolStripMenuItem
            {
                Font = new Font("Segoe UI", 9F),
                Text = _isChinese ? "退出" : "Exit"
            };

            exit.Click += (s, e) =>
            {
                _notifyIcon.Dispose();
                _sc.Dispose();
                Application.Exit();
            };

            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(exit);
            _notifyIcon.ContextMenuStrip = contextMenuStrip;
        }

        private Icon GetIcon()
            => _sc.Status.Equals(ServiceControllerStatus.Stopped) ? Resources.server_stop : Resources.server_run;

        private async void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _notifyIcon.MouseDoubleClick -= NotifyIcon_MouseDoubleClick;
            Process.Start("schtasks", "/run /TN \"SQLSwitcher\"");
            if (_sc.Status.Equals(ServiceControllerStatus.Stopped))
            {
                _notifyIcon.Icon = Resources.server_wait_32;
                _notifyIcon.Text = _notifyIcon.BalloonTipText =
                    _isChinese ? "SQL 服务正在启动..." : "SQL Server is starting...";
                _notifyIcon.ShowBalloonTip(5000);
                await Task.Run(() => _sc.WaitForStatus(ServiceControllerStatus.Running));
                _notifyIcon.Icon = Resources.server_run;
                _notifyIcon.Text = _notifyIcon.BalloonTipText = _isChinese ? "SQL 服务正在运行" : "SQL Server is running!";
            }
            else
            {
                _notifyIcon.Icon = Resources.server_wait_32;
                _notifyIcon.Text = _notifyIcon.BalloonTipText =
                    _isChinese ? "SQL 服务正在关闭..." : "SQL Server is shutting down...";
                _notifyIcon.ShowBalloonTip(5000);
                await Task.Run(() => _sc.WaitForStatus(ServiceControllerStatus.Stopped));
                _notifyIcon.Icon = Resources.server_stop;
                _notifyIcon.Text = _notifyIcon.BalloonTipText = _isChinese ? "SQL 服务已停止" : "SQL Server has stopped!";
            }

            _notifyIcon.ShowBalloonTip(3000);
            _notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        }
    }
}
