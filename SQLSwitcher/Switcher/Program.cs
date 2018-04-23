using System.ServiceProcess;
using System.Threading;

namespace Switcher
{
    static class Program
    {
        static void Main()
        {
            using (Mutex mutex = new Mutex(true, "{24c7fcb7-ec4b-4e75-ac5e-3a6dc931bb8f}", out bool createNew))
                using (ServiceController sc = new ServiceController("MSSQLSERVER"))
                    if (sc.Status.Equals(ServiceControllerStatus.Stopped))
                        sc.Start();
                    else
                        sc.Stop();
        }
    }
}
