using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheadExample
{
    public class Worker
    {
        private bool _cancelled = false;

        public void Cancel()
        {
            _cancelled = true;
        }

        public void Work()
        {
            for (int i = 0; i < 1000; i++)
            {
                if (_cancelled)
                    break;

                Thread.Sleep(3);
                ProcessChanged(i);
            }
            WorkCompleted(_cancelled);
        }

        public event Action<int> ProcessChanged;
        public event Action<bool> WorkCompleted;

    }
}
