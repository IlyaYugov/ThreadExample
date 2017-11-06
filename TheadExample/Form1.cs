using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace TheadExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            worker.RunWorkerAsync();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            worker.CancelAsync();
            buttonStart.Enabled = true;
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage + 1;
            progressBar.Value = e.ProgressPercentage;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string message = e.Error != null 
                ? $"Error: {e.Error.Message}"
                : e.Cancelled
                    ? "Процесс отменён"
                    : "Процесс завершен!";
            MessageBox.Show(message);
            buttonStart.Enabled = true;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                if(i == 30)
                {
                    throw new Exception("Ooooops");
                }

                worker.ReportProgress(i);
                Thread.Sleep(15);
            }
        }
    }
}
