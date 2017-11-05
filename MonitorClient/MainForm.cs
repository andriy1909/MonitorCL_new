using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonitorLibrary;

namespace MonitorClient
{
    public partial class MainForm : Form
    {
        ClientObject client = new ClientObject();
        public MainForm()
        {
            InitializeComponent();
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            client.Connect("127.0.0.1", 11000);
        }
    }
}
