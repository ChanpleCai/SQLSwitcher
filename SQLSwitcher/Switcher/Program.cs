using System.ServiceProcess;

namespace Switcher
{
    static class Program
    {
        static void Main()
        {
            using (ServiceController sc = new ServiceController("OracleServiceORCL"))
                if (sc.Status.Equals(ServiceControllerStatus.Stopped))
                    sc.Start();
                else
                    sc.Stop();
        }
    }
}
