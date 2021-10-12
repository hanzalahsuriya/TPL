using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TPL.DataSharingAndSync
{
    public class InterLockAccount
    {
        private int _balance;
        public int Balance 
        { 
            get => _balance; 
            set { _balance = value; }
        }

        public void Deposit(int amount)
        {
            Interlocked.Add(ref _balance, amount);
            // it's not thread safe as output won't be consistent so make it thread safe we add a lock
            // Balance += amount;
        }

        public void Withdraw(int amount)
        {
            Interlocked.Add(ref _balance, -amount);
            // Balance -= amount;
        }
    }

    public class InterlockDataSharingSyncUsingLock
    {
        public static void Main1()
        {
            var account = new InterLockAccount();
            var tasks = new List<Task>();
            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 1000; j++)
                    {
                        account.Deposit(1000);
                    }

                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 1000; j++)
                    {
                        account.Withdraw(1000);
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"current balance: {account.Balance}");
        }
    }
}