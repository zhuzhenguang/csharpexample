using System;
using System.Linq;
using System.Threading.Tasks;

namespace csharpexample
{
    class Program
    {
        private static readonly object lockObject = new object();
        private static int account;

        static async Task Main(string[] args)
        {
            Task task1 = Task.Run(Add10K);
            Task task2 = Task.Run(Add10K);

            await task1;
            await task2;

            Console.WriteLine(account);
        }

        static void Add10K()
        {
            lock (lockObject)
            {
                Enumerable.Range(0, 10000).ToList().ForEach(i => account += 1);
            }
        }
    }
}