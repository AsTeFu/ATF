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

        private static ElementaryTransformations matrix = new ElementaryTransformations(new Matrix());
        private static ElementaryTransformations lastMatrix = new ElementaryTransformations(new Matrix());
        private static string command;

        static void Main(string[] args) {
            while (true) {
                Console.Write("command ");
                command = Console.ReadLine();

                if (command.StartsWith("show"))
                    Console.Write("\n" + matrix.Show("\t"));
                else if (command.StartsWith("stepedd"))
                    matrix.SteppedForm();
                else if (command.StartsWith("solve"))
                    matrix.Solve();
                else if (command.StartsWith("new")) 
                    NewMatrix();
                else if (command.StartsWith("subtract"))
                    Subtract();
                else if (command.StartsWith("multiply"))
                    Multiply();
                else if (command.StartsWith("swap"))
                    Swap();
                else if (command.StartsWith("back"))
                    LastMatrix();
                else if (command.StartsWith("exit"))
                    return;
                else UnknownCommand();
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
        private static void NewMatrix() {
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
        private static void Subtract() {
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

                matrix.Subtract(line1 - 1, line2 - 1, constant[1], constant[0]);
            }
            else {
                Console.WriteLine("\tНе правильные параметры");
            }
        }
        private static void Swap() {
            lastMatrix = CopyMatrix(matrix);

            string[] lines = command.Substring("swap".Length).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int line1, line2;

            if (lines.Length == 2 && int.TryParse(lines[0], out line1) && int.TryParse(lines[1], out line2)
                && line1 > 0 && line2 > 0 && line1 <= matrix.data.Lines && line2 <= matrix.data.Lines) {
                matrix.SwapLines(line1 - 1, line2 - 1);
            }
            else {
                Console.WriteLine("\tНе правильные параметры");
            }
        }
        private static void Multiply() {
            lastMatrix = CopyMatrix(matrix);

            string[] lines = command.Substring("multiply".Length).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int line, num, den = 1;

            if (lines.Length == 2 && int.TryParse(lines[0], out line) && line > 0 && line <= matrix.data.Lines) {
                lines = lines[1].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                if ((lines.Length == 1 && int.TryParse(lines[0], out num)) || (lines.Length == 2 && int.TryParse(lines[0], out num) && int.TryParse(lines[1], out den) && den != 0)) {
                    matrix.MultiplyConst(line - 1, new Rational(num, den));
                }
                else {
                    Console.WriteLine("\tНе правильные параметры");
                }
            }
            else {
                Console.WriteLine("\tНе правильные параметры");
            }
        }
        private static void LastMatrix() {
            Console.WriteLine("\tВосстановлено прошлое значение матрицы");
            matrix = CopyMatrix(lastMatrix);
        }
        private static void UnknownCommand() {
            Console.WriteLine("\tДоступный набор команд: ");
            Console.WriteLine("\t\tshow");
            Console.WriteLine("\t\tstepedd");
            Console.WriteLine("\t\tsolve");
            Console.WriteLine("\t\tnew");
            Console.WriteLine("\t\tsubtract {line 1} {line 2} {const 2 = 1} {const 1 = 1}");
            Console.WriteLine("\t\tmultiply {line} {const}");
            Console.WriteLine("\t\tswap {line 1} {line 2}");
            Console.WriteLine("\t\tback");
            Console.WriteLine("\t\texit\n");
        }
    }

    class ElementaryTransformations {
        public Matrix data {
            get; private set;
        }

        public ElementaryTransformations(Matrix matrix)
        {
            data = matrix;
        }

        public void Subtract(int line1, int line2) {
            Console.WriteLine($"\tВычетаем из {line1 + 1} строку {line2 + 1}");

            for (int j = 0; j < data.Columns; j++) {
                data[line1 - 1, j] -= data[line2 - 1, j];
            }
        }
        public void Subtract(int line1, int line2, Rational constant2) {
            if (constant2 > 0)
                Console.Write($"\tИз строки ({line1 + 1}) вычетаем строку ({line2 + 1}) умноженную на {MathRational.Abs(constant2)}: ");
            else Console.Write($"\tК строке ({line1 + 1}) добавляем строку ({line2 + 1}) умноженную на {MathRational.Abs(constant2)}: ");
            Console.WriteLine($"({line1 + 1}) {(constant2 > 0 ? "-" : "+")} {MathRational.Abs(constant2)} * ({line2 + 1})\n");
            
            for (int j = 0; j < data.Columns; j++) {
                data[line1, j] -= data[line2, j] * constant2;
            }

            Console.Write(Show("\t\t"));
        }
        public void Subtract(int line1, int line2, Rational constant1, Rational constant2) {
            Console.WriteLine($"\tВычетаем из строки {line1 + 1} {(constant1 == 1 ? "" : "умноженной на " + constant1.ToString())} строку {line2 + 1} {(constant2 == 1 ? "" : "умноженную на " + constant2.ToString())}");

            for (int j = 0; j < data.Columns; j++) {
                data[line1, j] = data[line1, j] * constant1 - data[line2, j] * constant2;
            }
        }
        public void MultiplyConst(int line, Rational constant) {
            Console.Write($"\tУмножаем строку ({line + 1}) на {constant}: ");
            Console.WriteLine($"\t{constant} * ({line + 1}):\n");

            for (int j = 0; j < data.Columns; j++) {
                data[line, j] *= constant;
            }
            
            Console.Write(Show("\t\t"));
        }
        public void SwapLines(int line1, int line2) {
            Console.Write($"\tМеняем строки ({line1 + 1}) и ({line2 + 1}) местами: ");
            Console.WriteLine($"({line1 + 1}) ↔ ({line2 + 1}):\n");

            for (int j = 0; j < data.Columns; j++) {
                Rational tmp = data[line1, j];
                data[line1, j] = data[line2, j];
                data[line2, j] = tmp;
            }
            
            Console.Write(Show("\t\t"));
        }
        public void DeleteZeroLines() {
            int tmpAmount = 0;
            while (zeroLines() != -1) {
                int zeroLine = zeroLines();
                deleteZeroLine(zeroLine);
                Console.WriteLine($"\tУдаляем пустую строку ({zeroLine + tmpAmount++})\n");
            }

            int zeroLines() {
                for (int i = 0; i < data.Lines; i++) {
                    for (int j = 0; j < data.Columns; j++) {
                        if (data[i, j] != 0)
                            break;

                        if (j == data.Columns - 1) {
                            return i;
                        }
                    }
                }
                return -1;
            }
            void deleteZeroLine(int line) {
                int offset = 0;
                Rational[,] rationals = new Rational[data.Lines - 1, data.Columns];
                for (int i = 0; i < data.Lines; i++) {
                    if (line != i)
                        for (int j = 0; j < data.Columns; j++) {
                            rationals[i - offset, j] = data[i, j];
                        }
                    else {
                        offset++;

                    }
                }
                data = new Matrix(rationals);
            }
        }
        
        public override string ToString() {
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
        public string Show(string tabl) {
            string tmp = tabl;
            for (int i = 0; i < data.Lines; i++) {
                for (int j = 0; j < data.Columns; j++) {
                    tmp += data[i, j] + "\t" + tabl;
                }
                tmp += "\n" + tabl;
            }
            tmp += "\n";
            return tmp;
        }

        public void SteppedForm() {
            data = GetSteppedForm().data;
        }
        public ElementaryTransformations GetSteppedForm() {
            ElementaryTransformations newMatrix = CopyMatrix();
            for (int i = 0; i < newMatrix.data.Lines; i++) {
                var keyElement = newMatrix.FindKeyElemnt(i);
                if (keyElement.Item1 != -1 && keyElement.Item2 != -1) {
                    if (i != keyElement.Item1)
                        newMatrix.SwapLines(i, keyElement.Item1);
                    if (newMatrix.data[i, keyElement.Item2] != 1)
                        newMatrix.MultiplyConst(i, 1 / newMatrix.data[i, keyElement.Item2]);

                    for (int k = i + 1; k < newMatrix.data.Lines; k++) {
                        if (newMatrix.data[k, keyElement.Item2] != 0)
                            newMatrix.Subtract(k, i, newMatrix.data[k, keyElement.Item2]);
                    }
                }
            }

            newMatrix.DeleteZeroLines();

            return newMatrix;
        }

        public void Solve() {
            data = GetSolve().data;
        }
        public ElementaryTransformations GetSolve() {
            ElementaryTransformations newMatrix = GetSteppedForm();

            int currentCol = newMatrix.data.Lines - 1;
            for (int i = currentCol; i > 0; i--) {
                for (int k = i - 1; k >= 0; k--) {
                    newMatrix.Subtract(k, i, newMatrix.data[k, currentCol]);
                }
                currentCol--;
            }

            return newMatrix;
        }
        
        private (int, int) FindKeyElemnt(int startIndex) {
            for (int j = startIndex; j < data.Columns; j++) {
                for (int i = startIndex; i < data.Lines; i++) {
                    if (data[i, j] != 0) {
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }
        private ElementaryTransformations CopyMatrix() {
            Rational[,] newData = new Rational[data.Lines, data.Columns];

            for (int i = 0; i < data.Lines; i++) {
                for (int j = 0; j < data.Columns; j++) {
                    newData[i, j] = data[i, j];
                }
            }

            return new ElementaryTransformations(new Matrix(newData));
        }
    }
}