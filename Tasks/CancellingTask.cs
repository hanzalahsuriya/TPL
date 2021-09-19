using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL.Tasks
{
    public class CancellingTask
    {
        public static void Main1()
        {
            // SimpleCancellationTokenExample();
            CompositeCancellationExample();
        }

        public static void SimpleCancellationTokenExample()
        {
            // create a cancellation source
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            // register an event handler if want to be notified if a task is cancelled
            token.Register(() => 
            {
                Console.WriteLine("Task has been cancelled");
            });

            // pass cancellation token to task 
            Task task = new Task(() =>
            {
                int i = 0;
                while(true)
                {
                    token.ThrowIfCancellationRequested();
                    Console.WriteLine(i++);
                }
            }, token);
            
            task.Start();
            Console.ReadKey();
            cts.Cancel(); 
        }

        public static void CompositeCancellationExample()
        {
            var planned = new CancellationTokenSource();
            var preventive = new CancellationTokenSource();
            var emergency = new CancellationTokenSource();

            var paranoid = CancellationTokenSource.CreateLinkedTokenSource(planned.Token,preventive.Token,emergency.Token);

            var task = Task.Factory.StartNew(() => 
            {
                int i = 0;
                while(true)
                {
                    paranoid.Token.ThrowIfCancellationRequested();
                    Console.WriteLine(i++);
                    Thread.Sleep(1000);
                }
            }, paranoid.Token);

            Console.ReadKey();
            emergency.Cancel(); // as this or any above cancellation token source will break above task
        }
    }
}