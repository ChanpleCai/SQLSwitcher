using System;
using System.Threading;
using System.Windows.Forms;

namespace SQLSwitcher
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(true, "{b3de16e0-0a95-4a4f-8869-e45eca845c2a}", out bool createNew))
            {
                if (!createNew)
                    return;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                new IconTray();
                Application.Run();
            }
        }
    }
}
