using System;
using System.IO;
using Microsoft.Win32.TaskScheduler;

namespace BuildTask
{
    internal static class Program
    {
        private static void Main()
        {
            //https://stackoverflow.com/questions/7394806/creating-scheduled-tasks
            using (var ts = new TaskService())
            {
                var td = ts.NewTask();
                td.Actions.Add(new ExecAction(Path.Combine(Environment.CurrentDirectory, "Switcher")));
                //https://github.com/dahall/TaskScheduler/wiki/TaskSecurity
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Principal.RunLevel = TaskRunLevel.Highest;
                ts.RootFolder.RegisterTaskDefinition("SQLSwitcher", td);
            }
        }
    }
}
