using System;
using ATF.RationalNumbers;
using ATF.Matrix;
using ATF.Matrix.ElementaryTransformations;

class Program {
    private static ElementaryTransformations matrix = new ElementaryTransformations(new MatrixRational(new Rational[0, 0]));
    private static ElementaryTransformations lastMatrix = new ElementaryTransformations(new MatrixRational(new Rational[0, 0]));
    private static string command;
    
    static void Main(string[] args) {
        while (true) {
            Console.Write("command ");
            command = Console.ReadLine().TrimStart(' ');

            if (command.StartsWith("show"))
                Console.Write("\n" + matrix.ToString().Replace("\t", "\t\t"));
            else if (command.StartsWith("stepedd")) {
                lastMatrix = CopyMatrix(matrix);
                matrix.SteppedForm();
            }
            else if (command.StartsWith("solve")) {
                lastMatrix = CopyMatrix(matrix);
                matrix.Solve();
            }
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

            return new ElementaryTransformations(new MatrixRational(newData));
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
                string str;
                Rational rational;
                do {
                    Console.Write("\t\tВведие число" + $" [{i}, {j}]: ");
                    str = Console.ReadLine();
                } while (!Rational.TryParse(str, out rational));

                data[i, j] = rational;
            }
            Console.WriteLine();
        }

        matrix = new ElementaryTransformations(new MatrixRational(data));
        }
    private static void Subtract() {
        lastMatrix = CopyMatrix(matrix);
    
        string[] lines = command.Substring("subtract".Length).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    
        int line1, line2;
        Rational[] constants = new Rational[2];
        constants[0] = 1;
        constants[1] = 1;
    
        if (lines.Length >= 2 && int.TryParse(lines[0], out line1) && int.TryParse(lines[1], out line2) &&
            line1 > 0 && line2 > 0 && line1 <= matrix.data.Lines && line2 <= matrix.data.Lines) {
    
            if (lines.Length > 2) {
                for (int i = 0; i < lines.Length - 2; i++) {
                    if (Rational.TryParse(lines[i + 2], out Rational constant)) {
                        constants[i] = constant;
                    } else {
                        Console.WriteLine("\tНе правильные параметры");
                    }
                }
            }
            
            matrix.Subtract(line1 - 1, line2 - 1, constants[0], constants[1]);
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
        int line;
        
        if (lines.Length == 2 && int.TryParse(lines[0], out line) && line > 0 && line <= matrix.data.Lines) {
            if (Rational.TryParse(lines[1], out Rational rational)) {

                matrix.MultiplyConst(line - 1, rational);
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
    
   

    

