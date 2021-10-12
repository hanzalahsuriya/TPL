using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TPL.DataSharingAndSync
{
    public class MutexLockAccount
    {
        private int _balance;
        public int Balance 
        { 
            get => _balance; 
            set { _balance = value; }
        }

        public void Deposit(int amount)
        {
            Balance += amount;
        }

        public void Withdraw(int amount)
        {
            Balance -= amount;
        }
    }

    public class MutexlockDataSharingSyncUsingLock
    {
        public static void Main1()
        {
            var account = new MutexLockAccount();
            var tasks = new List<Task>();
            Mutex mutex = new Mutex();

            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 1000; j++)
                    {
                        bool lockTaken = mutex.WaitOne();
                        try
                        {
                            Console.WriteLine($"{i}, {j} - deposit");
                            account.Deposit(1000);
                        }
                        finally
                        {
                            if(lockTaken) mutex.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 1000; j++)
                    {
                        bool lockTaken = mutex.WaitOne();
                        try
                        {                            
                            Console.WriteLine($"{i}, {j} - withdraw");
                            account.Withdraw(1000);
                        }
                        finally
                        {
                            if(lockTaken) mutex.ReleaseMutex();
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"current balance: {account.Balance}");
        }
    }
}