using System;
using System.Collections.Generic;

namespace Stupeni {
    class Program {
        static void Main(string[] args) {
            Matrix matrix = new Matrix(new double[,] { { 1, 5, -4 }, 
                                                       { -7, 1, 2 }, 
                                                       { -1, 0, 2 }, 
                                                       { -5, 1, 7 } }, 4, 3);
            
            Console.WriteLine(matrix);

            matrix.DiffLines(2, 1, -7);
            matrix.DiffLines(3, 1, -1);
            matrix.DiffLines(4, 1, -5);

            matrix.DiffLines(4, 2);
            

            Console.Read();
        }
    }


    class Matrix {

        double[,] data;
        int column;
        int row;

        public Matrix(double[,] data, int row, int column) {
            this.data = data;
            this.column = column;
            this.row = row;
        }

        public void DiffLines(int line1, int line2) {
            line1 -= 1;
            line2 -= 1;

            Console.WriteLine($"Вычетаем из {line1 + 1} строку {line2 + 1}");

            for (int j = 0; j < column; j++) {
                data[line1, j] -= data[line2, j];
            }

            Console.WriteLine(this);
        }

        public void DiffLines(int line1, int line2, double constant2) {
            line1 -= 1;
            line2 -= 1;
            Console.WriteLine($"Вычетаем из {line1 + 1} строку {line2 + 1} умноженную на {constant2}");
            for (int j = 0; j < column; j++) {
                data[line1, j] -= data[line2, j] * constant2;
            }

            Console.WriteLine(this);
        }

        public void DiffLines(int line1, int line2, double constant1, double constant2) {
            line1 -= 1;
            line2 -= 1;

            Console.WriteLine($"Вычетаем из {line1 + 1} умноженной на {constant1} строку {line2 + 1} умноженную на {constant2}");

            for (int j = 0; j < column; j++) {
                data[line1, j] = data[line1, j] * constant1 - data[line2, j] * constant2;
            }

            Console.WriteLine(this);
        }

        public void MultiplyConst(int line, double constant) {
            line -= 1;
            Console.WriteLine($"Умножаем строку {line + 1} на {constant}");
            for (int j = 0; j < column; j++) {
                data[line, j] *= constant;
            }

            Console.WriteLine(this);
        }

        public void SwapLines(int line1, int line2) {
            line1 -= 1;
            line2 -= 1;
            Console.WriteLine($"Меняем строки местами {line1 + 1} и {line2 + 1}");

            for (int j = 0; j < column; j++) {
                double tmp = data[line1, j];
                data[line1, j] = data[line2, j];
                data[line2, j] = tmp;
            }

            Console.WriteLine(this);
        }


        //public void Solve() {

        //    bool common = true;
        //    //int keyElements = 0;
        //    List<int> indexKeyElements = new List<int>();
        //    for (int j = 0, i = 0; j < column && i < row; j++, i++) {
        //        if (data[i, j] != 0) {
        //            MultiplyConst(i + 1, 1 / data[i, j]);

        //            indexKeyElements.Add(j);

        //            if (j == column - 1)
        //                common = false;

        //            for (int k = i + 1; k < row; k++) {
        //                DiffLines(k + 1, i + 1, data[k, j]);
        //            }
        //        } else {
        //            i--;
        //            continue;
        //        }
        //    }

        //    if (common) {

        //        if (indexKeyElements.Count == column - 1) {
        //            Console.WriteLine("СЛУ совместная определенная");

        //        } else {
        //            Console.WriteLine("СЛУ совместная неопределенная");
        //        }

        //        for (int j = column - 1; j >= 0; j--) {
        //            if (indexKeyElements.Contains(j)) {
        //                for (int i = )
        //            }
        //        }


        //    } else {
        //        Console.WriteLine("СЛУ несовместна");
        //    }

        //}



        public override  string ToString() {
            string tmp = "";
            for (int i = 0; i < row; i++) {
                for (int j = 0; j < column; j++) {
                    tmp += data[i, j] + "\t";
                }
                tmp += "\n";
            }
            tmp += "\n";
            return tmp;
        }
    }
}
