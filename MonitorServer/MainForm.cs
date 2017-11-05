using MonitorLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonitorServer
{
    public partial class MainForm : Form
    {
        ServerObject server = new ServerObject();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            server.setReceiveOut(ReceiveMessage);
            server.StartServer("127.0.0.1", 8998);
        }

        public void ReceiveMessage(string message)
        {
            var settextAction = new Action(() => { richTextBox1.AppendText(Environment.NewLine + "Обновляем данные"); });
            if (richTextBox1.InvokeRequired)
                richTextBox1.Invoke(settextAction);
            else
                settextAction();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Dispose();
        }
    }
}
