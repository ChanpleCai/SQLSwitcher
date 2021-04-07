using System.ServiceProcess;

namespace Switcher
{
    internal static class Program
    {
        private static void Main()
        {
            using var sc = new ServiceController("MSSQLSERVER");
            if (sc.Status.Equals(ServiceControllerStatus.Stopped))
                sc.Start();
            else
                sc.Stop();
        }
    }
}
