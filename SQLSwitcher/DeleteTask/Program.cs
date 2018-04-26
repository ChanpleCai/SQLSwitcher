using Microsoft.Win32.TaskScheduler;

namespace DeleteTask
{
    static class Program
    {
        private const string name = "SQLSwitcher";

        static void Main()
        {
            //https://stackoverflow.com/questions/7394806/creating-scheduled-tasks
            using (TaskService ts = new TaskService())
            {
                if (ts.GetTask(name) != null)
                {
                    ts.RootFolder.DeleteTask(name);
                    return;
                }
            }
        }

    }
}
