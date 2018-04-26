using Microsoft.Win32.TaskScheduler;
using System;
using System.IO;

namespace BuildTask
{
    static class Program
    {
        static void Main()
        {
            //https://stackoverflow.com/questions/7394806/creating-scheduled-tasks
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.Actions.Add(new ExecAction(Path.Combine(Environment.CurrentDirectory, "Switcher")));
                //https://github.com/dahall/TaskScheduler/wiki/TaskSecurity
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Principal.RunLevel = TaskRunLevel.Highest;
                ts.RootFolder.RegisterTaskDefinition("SQLSwitcher", td);
            }
        }
    }
}
