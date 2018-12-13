using System;
using System.Diagnostics;

namespace Sqrt {
    class Program {
        static void Main(string[] args) {

            Stopwatch time1 = new Stopwatch();
            Stopwatch time2 = new Stopwatch();
            Stopwatch time3 = new Stopwatch();

            double tmp;

            Console.WriteLine("Оригинальный sqrt: ");
            time1.Start();
            for (int i = 0; i < 10000000; i++) {
                tmp = Math.Sqrt(i);
            }
            time1.Stop();
            Console.WriteLine($"Time: {time1.ElapsedMilliseconds} ms; {time1.ElapsedTicks} ticks\n");

            Console.WriteLine("Древняя магия Вавилона: ");
            time2.Start();
            for (int i = 0; i < 10000000; i++) {
                tmp = sqrtBabylon(i);
            }
            time2.Stop();
            Console.WriteLine($"Time: {time2.ElapsedMilliseconds} ms; {time2.ElapsedTicks} ticks\n");

            Console.WriteLine("Через экспоненту: ");
            time3.Start();
            for (int i = 0; i < 10000000; i++) {
                tmp = sqrtExp(i);
            }
            time3.Stop();
            Console.WriteLine($"Time: {time3.ElapsedMilliseconds} ms; {time3.ElapsedTicks} ticks");

            Console.ReadLine();

        }

        private static double sqrtBabylon(double num) {
            double result = 1;

            for (int i = 0; i < 9; i++) {
                result = (result + num / result) * 0.5;
            }

            return result;
        }

        private static double sqrtExp(double num) {
            return Math.Exp(Math.Log(num) * 0.5);
            
        }

    }
}
