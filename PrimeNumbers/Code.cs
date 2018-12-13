using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace PrimeList {
    class Program {
        static void Main(string[] args) {

            PrimeNumbers primes = new PrimeNumbers();
            
            Console.Write("Введите предел поиска простых чисел = ");

            ulong maxNumber = ulong.Parse(Console.ReadLine());

            List<uint> p = primes.EratosthenesTable(maxNumber);

            string appRoot = AppDomain.CurrentDomain.BaseDirectory;
            Directory.CreateDirectory(Path.Combine(appRoot, "data"));
            using (StreamWriter sw = new StreamWriter(Path.Combine(appRoot, "data", "primes_" + p.Count.ToString() + ".txt"), false, Encoding.Default)) {

                sw.WriteLine($"Time: {primes.TimeWork} ms");
                sw.WriteLine($"Ticks: {primes.TickWork}");
                sw.WriteLine($"All numbers count: {p.Count}");
                sw.WriteLine($"Max numbers count: {maxNumber}\n");

                for (int i = 0; i < p.Count(); i++) {
                    if (i % 10 == 0) sw.Write("\n");
                    sw.Write(p[i] + "\t");
                }
            }
            
            Console.WriteLine($"Time: {primes.TimeWork} ms\n" 
                            + $"Ticks: {primes.TickWork}\n"
                            + $"All numbers count: {p.Count}\n"
                            + $"Max numbers count: {maxNumber}\n");


            Console.Write("Вывести числа? ");
            string show = "";
            do {
                Console.WriteLine("(yes/no)");
                show = Console.ReadLine();
            }
            while (!(show == "yes" || show == "no" || show == ""));

            if (show == "yes") {
                Console.WriteLine("Простые числа до " + maxNumber + ": ");
                for (int i = 0; i < p.Count(); i++) {
                    Console.Write(p[i] + "\t");
                }

                Console.Read();
            }
        }
    }

    class PrimeNumbers {
        
        private Stopwatch timer = new Stopwatch();

        public long TimeWork {
            get {
                return timer.ElapsedMilliseconds;
            }
        }
        public long TickWork {
            get {
                return timer.ElapsedTicks;
            }
        }

        public List<uint> primeNumbers = new List<uint>();

        public List<uint> EratosthenesTable(ulong n) {
            return GetPrimeOptimize(n);
        }

        private List<uint> GetPrimes(ulong n) {
            byte[] allNumbers = new byte[n + 1UL];
            List<uint> primeNumbers = new List<uint>();

            primeNumbers.Add(2);

            timer.Start();
            for (uint i = 3; i <= n; i += 2) {
                if (allNumbers[i] == 0) {
                    primeNumbers.Add(i);

                    for (ulong j = i * i; j <= n; j += i) {
                        allNumbers[j] = 1;
                    }
                }
            }
            timer.Stop();

            return primeNumbers;
        }
        private List<uint> GetPrimeOptimize(ulong n) {
            byte[] allNumbers = new byte[(n % 2 == 1 ? n : n + 1) / 2];
            List<uint> primeNumbers = new List<uint>();

            primeNumbers.Add(2);

            timer.Start();
            for (uint i = 3; i <= n; i += 2) {
                if (allNumbers[(i - 3) / 2] == 0) {
                    primeNumbers.Add(i);

                    for (ulong j = i * i; j <= n; j += 2 * i) {
                        allNumbers[(j - 3) / 2] = 1;
                    }
                }
            }
            timer.Stop();

            return primeNumbers;
        }
    }
}
