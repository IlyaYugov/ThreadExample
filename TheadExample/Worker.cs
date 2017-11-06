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

        public void Work(object param)
        {
            SynchronizationContext context = (SynchronizationContext)param;

            for (int i = 0; i < 1000; i++)
            {
                if (_cancelled)
                    break;

                Thread.Sleep(3);
                context.Send(OnProcessChanged, i);
            }
            context.Send(OnWorkCompleted, _cancelled);
        }

        public void OnProcessChanged(object i)
        {
            ProcessChanged?.Invoke((int)i);
        }

        public void OnWorkCompleted(object cancelled)
        {
            WorkCompleted?.Invoke((bool)cancelled);
        }

        public event Action<int> ProcessChanged;
        public event Action<bool> WorkCompleted;

    }
}
