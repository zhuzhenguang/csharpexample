using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace csharpexample
{
    class Program
    {
        private static readonly object lockObject = new object();
        private static int account;
        private static int usingResource = 0;

        static async Task Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // Task task1 = Task.Run(Add10KWithCompareExchnage);
            // Task task2 = Task.Run(Add10KWithCompareExchnage);
            // Task task1 = Task.Run(Add10KWithLock);
            // Task task2 = Task.Run(Add10KWithLock);
            List<Task> tasks = Enumerable.Range(0, 20000).ToList().Select(i => Task.Run(Add1WithCompareExchnage)).ToList();
            
            foreach (Task task in tasks)
            {
                await task;
            }

            // await task1;
            // await task2;

            Console.WriteLine(account);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        static void Add10KWithLock()
        {
            lock (lockObject)
            {
                Enumerable.Range(0, 10000).ToList().ForEach(i => account += 1);
            }
        }

        static void Add1WithCompareExchnage()
        {
            while (true)
            {
                int currentAccount = account;
                int newAccount = account + 1;
                int replaced = Interlocked.CompareExchange(ref account, newAccount, currentAccount);
                if (replaced == currentAccount)
                {
                    break;
                }
            }
        }
    }
}