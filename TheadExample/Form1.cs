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
        private TaskScheduler _sheduler;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            _worker = new Worker();
            _worker.ProcessChanged += Worker_PreocessChanged;

            buttonStart.Enabled = false;

            var task =  Task.Factory.StartNew(_worker.Work);
            task.ContinueWith((t, o) => WorkCompoleted(t.Result), null, _sheduler);


        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            _worker.Cancel();
            buttonStart.Enabled = true;
        }

        private void WorkCompoleted(bool cancelled)
        {
            string message = cancelled
                ? "Процесс отменён"
                : "Процесс завершен!";
            MessageBox.Show(message);
            buttonStart.Enabled = true;
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
            _sheduler = TaskScheduler.FromCurrentSynchronizationContext();

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
