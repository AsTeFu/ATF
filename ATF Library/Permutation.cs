using System;
using System.Collections.Generic;
using System.Linq;

namespace Permutation {
    class Permutation {
        private int[] data;
        public int Degree {
            get {
                return data.Length;
            }
        }

        private int[][] cycles;
        private int[][] transposition;
        public readonly int order;
        public readonly int sign;
        public readonly int decrement;

        public Permutation(params int[] permutation) {
            if (permutation.Distinct().Count() != permutation.Length)
                throw new Exception("Перестановка содержит повторяющиеся элементы");

            for (int i = 0; i < permutation.Length; i++)
                if (permutation[i] > permutation.Length)
                    throw new Exception($"{i}-ый элемент больше или меньше степени перестановки!");

            data = permutation;

            cycles = GetCycles();
            transposition = GetTranspostions();

            order = GetOrder();
            sign = GetSign();
            decrement = Degree - cycles.Length;
        }

        int this[int index] {
            get {
                return data[index - 1];
            }
        }

        public static Permutation Identical(int len) {
            int[] result = new int[len];
            for (int i = 0; i < len; i++)
                result[i] = i + 1;

            return new Permutation(result);
        }

        private int[][] GetCycles() {
            var result = new List<int[]>();
            var used = new List<int>();

            var currentCycle = new List<int>();
            for (int i = 0; i < data.Length; i++) {
                if (!used.Contains(i + 1)) {
                    currentCycle = new List<int>();
                    currentCycle.Add(i + 1);

                    for (int j = i + 1; !currentCycle.Contains(data[j - 1]); j = data[j - 1]) {
                        currentCycle.Add(data[j - 1]);
                        used.Add(data[j - 1]);
                    }

                    result.Add(currentCycle.ToArray());
                }
            }

            return result.ToArray();
        }
        private int[][] GetTranspostions() {
            int amount = 0;
            for (int i = 0; i < cycles.Length; i++) {
                if (cycles[i].Length > 1)
                    amount += cycles[i].Length - 1;
            }

            int current = 0;
            int[][] trans = new int[amount][];
            for (int i = 0; i < cycles.Length; i++)
                if (cycles[i].Length > 1)
                    for (int j = cycles[i].Length - 1; j > 0; j--) {
                        trans[current] = new int[2];

                        trans[current][0] = cycles[i][0];
                        trans[current++][1] = cycles[i][j];
                    }

            return trans;
        }
        private int NOD(int a, int b) {
            if (b == 0)
                return a;
            return NOD(b, a % b);
        }
        private int NOK(int a, int b) {
            return a * b / NOD(a, b);
        }
        private int GetOrder() {
            List<int> lens = new List<int>();
            for (int i = 0; i < cycles.Length; i++)
                lens.Add(cycles[i].Length);

            if (lens.Count == 1)
                return lens[0];

            int m = NOK(lens[0], lens[1]);
            for (int i = 2; i < lens.Count; i++) {
                m = NOK(m, lens[i]);
            }

            return m;
        }
        private int GetSign() {
            return (int)Math.Pow(-1, transposition.Length);
        }

        private int[] ReverseData() {
            int[] newData = new int[data.Length];

            for (int i = 0; i < newData.Length; i++) {
                newData[data[i] - 1] = i + 1;
            }

            return newData;
        }
        public Permutation Reverse() {
            return new Permutation(ReverseData());
        }
        public Permutation Pow(int pow) {
            if (pow == 0)
                return Identical(data.Length);

            int[] result = new int[data.Length];
            int a = Math.Abs(pow);

            int IndexOfCycle(int item) {
                for (int i = 0; i < cycles.Length; i++)
                    if (cycles[i].Contains(item))
                        return i;
                return -1;
            }
            int IndexOfItem(int cycle, int item) {
                for (int i = 0; i < cycles[cycle].Length; i++)
                    if (cycles[cycle][i] == item)
                        return i;
                return -1;
            }

            for (int i = 0; i < data.Length; i++) {
                int tmp = IndexOfCycle(i + 1);
                result[i] = cycles[tmp][(IndexOfItem(tmp, i + 1) + a) % cycles[tmp].Length];
            }

            return pow > 0 ? new Permutation(result) : new Permutation(result).Reverse();
        }

        public static Permutation operator *(Permutation left, Permutation right) {
            if (left.Degree != right.Degree)
                throw new Exception("Разные степени перестановки!");

            int[] result = new int[left.Degree];
            for (int i = 0; i < result.Length; i++)
                result[i] = left[right[i + 1]];

            return new Permutation(result);
        }

        public override string ToString() {
            string result = "";

            for (int i = 0; i < data.Length; i++)
                result += (i + 1) + " ";
            result += "\n";

            for (int i = 0; i < data.Length; i++)
                result += data[i] + " ";
            result += "\n";

            return result;
        }
        public string PrintCycle() {
            return Print(cycles);
        }
        public string PrintTransposition() {
            return Print(transposition);
        }
        private string Print(int[][] arr) {
            string result = string.Empty;

            for (int i = 0; i < arr.Length; i++) {
                result += "(";

                for (int j = 0; j < arr[i].Length; j++) {
                    result += arr[i][j];

                    if (j != arr[i].Length - 1)
                        result += ", ";
                }

                result += ")";
            }

            return result;
        }
    }
}
