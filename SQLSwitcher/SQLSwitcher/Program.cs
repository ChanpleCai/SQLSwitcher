using System;
using System.Threading;
using System.Windows.Forms;

namespace SQLSwitcher
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            using (new Mutex(true, "{b3de16e0-0a95-4a4f-8869-e45eca845c2a}", out var createNew))
            {
                if (!createNew)
                    return;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new IconTray());
            }
        }
    }
}
