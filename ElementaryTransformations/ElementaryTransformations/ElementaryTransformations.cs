using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RationalNumbers;
using MatrixATF;

namespace ElementaryTransformations
{
    class Program {
        static void Main(string[] args)
        {
            ElementaryTransformations matrix = new ElementaryTransformations(new Matrix());
            ElementaryTransformations lastMatrix = new ElementaryTransformations(new Matrix());


            while (true) {
                Console.Write("command ");
                string command = Console.ReadLine();

                if (command.StartsWith("show")) {
                    Console.Write("\n" + matrix);
                }
                else if (command.StartsWith("new")) {
                    Console.WriteLine("\tНовая матрица: ");
                    Console.WriteLine("\tДроби вводятся через '/'");

                    int lines = 0, columns = 0;
                    string[] dim;
                    do {
                        Console.Write("\tВведите размеры матрицы (3x4) or (3 4): ");
                        dim = Console.ReadLine().Split(new string[] { " ", "x" }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    while (dim.Length != 2 || !(int.TryParse(dim[0], out lines) && int.TryParse(dim[1], out columns) && lines > 0 && columns > 0));

                    Rational[,] data = new Rational[lines, columns];

                    for (int i = 0; i < lines; i++) {
                        for (int j = 0; j < columns; j++) {
                            string[] str;
                            int num = 1, den = 1;

                            do {
                                Console.Write("\t\tВведие число" + $" [{i}, {j}]: ");
                                str = Console.ReadLine().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                            } while ((str.Length == 1 && int.TryParse(str[0], out num))
                                     == (str.Length == 2 && int.TryParse(str[0], out num) && int.TryParse(str[1], out den) && den != 0));

                            data[i, j] = new Rational(num, den);
                        }
                        Console.WriteLine();
                    }

                    matrix = new ElementaryTransformations(new Matrix(data));
                }
                else if (command.StartsWith("subtract")) {
                    lastMatrix = CopyMatrix(matrix);

                    string[] lines = command.Substring("subtract".Length).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int line1, line2, num, den = 1;
                    Rational[] constant = new Rational[2];
                    constant[0] = new Rational(1);
                    constant[1] = new Rational(1);

                    if (lines.Length >= 2 && int.TryParse(lines[0], out line1) && int.TryParse(lines[1], out line2) &&
                        line1 > 0 && line2 > 0 && line1 <= matrix.data.Lines && line2 <= matrix.data.Lines) {
                        if (lines.Length > 2) {
                            for (int i = 0; i < lines.Length - 2; i++) {
                                string[] rational = lines[i + 2].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                                if ((rational.Length == 1 && int.TryParse(rational[0], out num)) ||
                                    (rational.Length == 2 && int.TryParse(rational[0], out num) && int.TryParse(rational[1], out den) && den != 0)) {
                                    constant[i] = new Rational(num, den);
                                }
                                else {
                                    Console.WriteLine("\tНе правильные параметры");
                                }
                            }
                        }

                        matrix.Subtract(line1, line2, constant[1], constant[0]);
                    }
                    else {
                        Console.WriteLine("\tНе правильные параметры");
                    }

                }
                else if (command.StartsWith("multiply")) {
                    lastMatrix = CopyMatrix(matrix);

                    string[] lines = command.Substring("multiply".Length).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int line, num, den = 1;

                    if (lines.Length == 2 && int.TryParse(lines[0], out line) && line > 0 && line <= matrix.data.Lines) {
                        lines = lines[1].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                        if ((lines.Length == 1 && int.TryParse(lines[0], out num)) || (lines.Length == 2 && int.TryParse(lines[0], out num) && int.TryParse(lines[1], out den) && den != 0)) {
                            matrix.MultiplyConst(line, new Rational(num, den));
                        }
                        else {
                            Console.WriteLine("\tНе правильные параметры");
                        }
                    }
                    else {
                        Console.WriteLine("\tНе правильные параметры");
                    }

                }
                else if (command.StartsWith("swap")) {
                    lastMatrix = CopyMatrix(matrix);

                    string[] lines = command.Substring("swap".Length).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int line1, line2;

                    if (lines.Length == 2 && int.TryParse(lines[0], out line1) && int.TryParse(lines[1], out line2)
                        && line1 > 0 && line2 > 0 && line1 <= matrix.data.Lines && line2 <= matrix.data.Lines) {
                        matrix.SwapLines(line1, line2);
                    }
                    else {
                        Console.WriteLine("\tНе правильные параметры");
                    }

                }
                else if (command.StartsWith("back")) {
                    matrix = CopyMatrix(lastMatrix);
                    Console.WriteLine("\tВосстановлено прошлое значение матрицы");
                }
                else if (command.StartsWith("exit")) {
                    return;
                }
                else {
                    Console.WriteLine("\tДоступный набор команд: ");
                    Console.WriteLine("\t\tshow");
                    Console.WriteLine("\t\tnew");
                    Console.WriteLine("\t\tsubtract {line 1} {line 2} {const 2 = 1} {const 1 = 1}");
                    Console.WriteLine("\t\tmultiply {line} {const}");
                    Console.WriteLine("\t\tswap {line 1} {line 2}");
                    Console.WriteLine("\t\tback");
                    Console.WriteLine("\t\texit\n");
                }
            }
        }

        private static ElementaryTransformations CopyMatrix(ElementaryTransformations from) {
            Rational[,] newData = new Rational[from.data.Lines, from.data.Columns];

            for (int i = 0; i < from.data.Lines; i++) {
                for (int j = 0; j < from.data.Lines; j++) {
                    newData[i, j] = from.data[i, j];
                }
            }

            return new ElementaryTransformations(new Matrix(newData));
        }
    }

    class ElementaryTransformations {
        public Matrix data {
            get;
        }

        public ElementaryTransformations(Matrix matrix)
        {
            data = matrix;
        }

        public void Subtract(int line1, int line2)
        {
            Console.WriteLine($"\tВычетаем из {line1 + 1} строку {line2 + 1}");

            for (int j = 0; j < data.Columns; j++) {
                data[line1 - 1, j] -= data[line2 - 1, j];
            }
        }

        public void Subtract(int line1, int line2, Rational constant2)
        {
            line1 -= 1;
            line2 -= 1;

            Console.WriteLine($"\tВычетаем из {line1 + 1} строку {line2 + 1} умноженную на {constant2}");

            for (int j = 0; j < data.Columns; j++) {
                data[line1, j] -= data[line2, j] * constant2;
            }
        }

        public void Subtract(int line1, int line2, Rational constant1, Rational constant2)
        {
            line1 -= 1;
            line2 -= 1;

            Console.WriteLine($"\tВычетаем из строки {line1 + 1} {(constant1 == 1 ? "" : "умноженной на " + constant1.ToString())} строку {line2 + 1} {(constant2 == 1 ? "" : "умноженную на " + constant2.ToString())}");

            for (int j = 0; j < data.Columns; j++) {
                data[line1, j] = data[line1, j] * constant1 - data[line2, j] * constant2;
            }
        }

        public void MultiplyConst(int line, Rational constant)
        {
            line -= 1;
            Console.WriteLine($"\tУмножаем строку {line + 1} на {constant}");

            for (int j = 0; j < data.Columns; j++) {
                data[line, j] *= constant;
            }
        }

        public void SwapLines(int line1, int line2)
        {
            line1 -= 1;
            line2 -= 1;
            Console.WriteLine($"\tМеняем строки местами {line1 + 1} и {line2 + 1}");

            for (int j = 0; j < data.Columns; j++) {
                Rational tmp = data[line1, j];
                data[line1, j] = data[line2, j];
                data[line2, j] = tmp;
            }
        }


        public override string ToString()
        {
            string tmp = "\t";
            for (int i = 0; i < data.Lines; i++) {
                for (int j = 0; j < data.Columns; j++) {
                    tmp += data[i, j] + "\t";
                }
                tmp += "\n\t";
            }
            tmp += "\n";
            return tmp;
        }
    }
}
