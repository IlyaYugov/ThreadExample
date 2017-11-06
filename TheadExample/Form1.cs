using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheadExample
{
    public partial class Form1 : Form
    {
        private Worker _worker;
        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            _worker = new Worker();
            _worker.ProcessChanged += Worker_PreocessChanged;
            _worker.WorkCompleted += WorkCompoleted;

            buttonStart.Enabled = false;

            bool cancelled = await Task.Factory.StartNew(_worker.Work);

            string message = cancelled
                ? "Процесс отменён"
                : "Процесс завершен!";
            MessageBox.Show(message);
            buttonStart.Enabled = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            _worker.Cancel();
            buttonStart.Enabled = true;
        }

        private void WorkCompoleted(bool cancelled)
        {
        }
        private void Worker_PreocessChanged(int progress)
        {
            Action action = () =>
            {
                progressBar.Value = progress;
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Action action = () =>
            {
                while (true)
                {
                    Invoke((Action)(() => lblTime.Text = DateTime.Now.ToLongTimeString()));
                    Thread.Sleep(1000);
                }
            };
            //var task = new Task(action);
            //task.Start();
            Task.Factory.StartNew(action);
        }
    }
}
