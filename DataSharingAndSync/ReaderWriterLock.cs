using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TPL.DataSharingAndSync
{
    public class ReaderWriterLock
    {
        static Random  random = new Random();
        public static void Main1()
        {
            int x = 0;

            ReaderWriterLockSlim readWriteSlim = new ReaderWriterLockSlim();
            var tasks = new List<Task>();
            for(var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => 
                {
                    readWriteSlim.EnterReadLock();
                    Console.WriteLine($"entered read lock {x}");
                    Thread.Sleep(3000);
                    readWriteSlim.ExitReadLock();
                    Console.WriteLine($"exited read lock {x}");
                }));
            }
            try
            {
                Task.WaitAll(tasks.ToArray());
            } 
            catch(AggregateException err)
            {
                err.Handle(e => {
                    Console.WriteLine(e);
                    return true;
                });
            }

            while(true)
            {
                Console.ReadKey();
                Console.WriteLine($"");

                readWriteSlim.EnterWriteLock();
                Console.WriteLine($"entered write lock {x}");

                x = random.Next(10);
                Console.WriteLine($"set value of x: {x}");
                readWriteSlim.ExitWriteLock();
                Console.WriteLine($"exited write lock {x}");


            }
        }
    } 
}