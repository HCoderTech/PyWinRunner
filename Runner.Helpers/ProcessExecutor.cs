using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Runner.Helpers
{
    public class ProcessExecutor : MarshalByRefObject,IRunner
    {
        public void Execute(string runon,string filename, string parameters)
        {
            Configuration runnerConfig = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
            string PythonPath = runnerConfig.AppSettings.Settings["PythonPath"].Value;
            string ScriptPath = Path.Combine(runnerConfig.AppSettings.Settings["ScriptPath"].Value,filename);
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.Arguments = "/C "+ PythonPath + " "+ ScriptPath + " "+parameters;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit(1000 * 600 * 5);
        }
    }
}
