using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = Task.Factory.StartNew((Func<bool>)Work);
            task.ContinueWith(t => Console.WriteLine($"Task completed Result: {task.Result}"));

            Console.WriteLine("Main Thread completed");
            Console.ReadKey();
        }

        private static bool Work()
        {
            Console.WriteLine("working.... working.....");

            Thread.Sleep(3000);
            return true;
        }
    }
}
