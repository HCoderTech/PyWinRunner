using Runner.Helpers;
using System;
using System.Configuration;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows.Forms;

namespace RemoteRunner
{
    public partial class RemoteForm : Form
    {
        public RemoteForm()
        {
            InitializeComponent();
        }

        private void RemoteForm_Load(object sender, EventArgs e)
        {
            TcpChannel channel = new TcpChannel(21000);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
               typeof(ProcessExecutor), "ProcessExecutor",
               WellKnownObjectMode.SingleCall);
            RunHelper.PythonPath = ConfigurationManager.AppSettings["PythonPath"];
            this.WindowState = FormWindowState.Minimized;
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }

        private void RemoteForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(3000);
                this.ShowInTaskbar = false;
            }
        }
    }
}
