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
        private SynchronizationContext _context;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            _worker = new Worker();
            _worker.ProcessChanged += Worker_PreocessChanged;
            _worker.WorkCompleted += WorkCompoleted;

            buttonStart.Enabled = false;

            //_worker.Work();
            Thread thread = new Thread(_worker.Work);
            thread.Start(_context);
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
            progressBar.Value = progress + 1;// Fix progress freeze
            progressBar.Value = progress;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _context = SynchronizationContext.Current;
        }
    }
}
