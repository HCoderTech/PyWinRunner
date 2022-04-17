using Runner.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace MainRunner
{
    public partial class MainForm : Form
    {
        IRunner RemoteRunner;
        IRunner MainRunner;
        Dictionary<string, IRunner> Runners = new Dictionary<string, IRunner>();
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MainRunner = new ProcessExecutor();
            RemoteRunner = (IRunner)Activator.GetObject(typeof(IRunner),
                  "tcp://localhost:21000/ProcessExecutor");
            Runners.Add("Main",MainRunner);
            Runners.Add("Remote",RemoteRunner);
            RunHelper.PythonPath = ConfigurationManager.AppSettings["PythonPath"];
            RunHelper.TestSessionFile = ConfigurationManager.AppSettings["TestSession"];
            RunHelper.ScriptPath = ConfigurationManager.AppSettings["ScriptPath"];
            this.WindowState = FormWindowState.Minimized;

            //Run the Test Session
            RunSession();

        }

        private void RunSession()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(RunHelper.TestSessionFile);
            XmlNodeList stepList = doc.DocumentElement.SelectNodes("//Step");
            foreach(XmlNode step in stepList)
            {
                Runners[step.Attributes["RunOn"].Value].Execute(
                    step.Attributes["RunOn"].Value,
                    step.Attributes["Filename"].Value, 
                    step.Attributes["Arguments"].Value
                    );
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(3000);
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }
    }
}
