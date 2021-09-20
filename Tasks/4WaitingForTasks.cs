using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPL.Tasks
{
    public class WaitingForTasks
    {
        public static void Main1()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var task1 = new Task(() =>
            {
                Console.WriteLine("Task1: I take 5 seconds");
                for (var i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }
                Console.WriteLine("Task1: I'm done after 5 seconds");
            }, token);
            task1.Start();

            var task2 = new Task(() =>
            {
                Console.WriteLine("Task2: I take 3 seconds");
                Thread.Sleep(3000);
                Console.WriteLine("Task2: I'm done after 3 seconds");
            }, token);
            task2.Start();

            // cancelling a token will throw exception if we are using task.WaitAll
            // Console.ReadKey();
            // cts.Cancel();

            // only wait for 4 seconds and then continue executing the main thread
            // if we use Task.WaitAny or Task.WaitAll then cancelling on a token actually fires an exception. it will throw aggregate exception
            Task.WaitAny(new[] { task1, task2 }, 4000, token);
            //Task.WaitAll(new[] {task1, task2}, 4000, token);

            Console.WriteLine($"Task1 status: {task1.Status}");
            Console.WriteLine($"Task2 status: {task2.Status}");

            Console.WriteLine("Main Program");
            Console.ReadKey();
            // cts.Cancel();
        }
    }
}