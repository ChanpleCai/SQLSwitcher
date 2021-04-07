using Microsoft.Win32.TaskScheduler;

namespace DeleteTask
{
    internal static class Program
    {
        private const string name = "SQLSwitcher";

        private static void Main()
        {
            //https://stackoverflow.com/questions/7394806/creating-scheduled-tasks
            using (var ts = new TaskService())
            {
                if (ts.GetTask(name) != null) { ts.RootFolder.DeleteTask(name); }
            }
        }
    }
}
