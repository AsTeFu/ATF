using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DNF {
    static class Program {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
            
            
        }
    }

    class Function {
        private bool[] function;
        private int[] functionInt;

        private int countVariables;
        private int rows {
            get {
               return (int)Math.Pow(2, countVariables);
            }
        }

        private bool[,] truthTable;

        private char overline = '̅';

        public bool ClassT0 {
            get {
                return !function[0];
            }
        }
        public bool ClassT1 {
            get {
                return function[rows - 1];
            }
        }
        public bool ClassS {
            get {
                return this == GetDualFunction();
            }
        }
        public bool ClassM {
            get {
                bool isComparable(int first, int second) {
                    for (int i = 0; i < countVariables; i++) {
                        if ((truthTable[first, i] ? 1 : 0) > (truthTable[second, i] ? 1 : 0))
                            return false;
                    }
                    return true;
                }

                for (int i = 0; i < rows; i++) {
                    if (function[i]) {
                        for (int j = i + 1; j < rows; j++) {
                            if (isComparable(i, j) && !function[j])
                                return false;
                        }
                    }
                }
                
                return true;
            }
        }
        public bool ClassL {
            get {
                bool isConjunction(int index) {
                    int tmp = 0;
                    for (int i = 0; i < countVariables; i++) {
                        if (truthTable[index, i])
                            tmp++;
                    }

                    return tmp > 1;
                }

                bool[] linear = new bool[rows];
                bool[] lineartwo = new bool[rows];

                List<bool> polynomial = new List<bool>();

                polynomial.Add(function[0]);
                for (int i = 0; i < rows; i++) {
                    linear[i] = function[i];
                }
                
                for (int i = 1; i < rows; i++) {
                    if (i % 2 == 1) {
                        lineartwo = new bool[rows - i];
                        polynomial.Add(linear[0] != linear[1]);

                        for (int j = 0; j < rows - i; j++) {
                            lineartwo[j] = linear[j] != linear[j+1];
                        }
                    } else {
                        linear = new bool[rows - i];
                        polynomial.Add(lineartwo[0] != lineartwo[1]);

                        for (int j = 0; j < rows - i; j++) {
                            linear[j] = lineartwo[j] != lineartwo[j + 1];
                        }
                    }
                    
                }
                
                for (int i = 0; i < polynomial.Count; i++) {
                    if (polynomial[i] && isConjunction(i))
                        return false;
                }

                return true;
            }
        }
        
        public static Function conjuncture = new Function(new int[] { 0, 0, 0, 1 });
        public static Function disjunction = new Function(new int[] { 0, 1, 1, 1 });
        public static Function implication = new Function(new int[] { 1, 1, 0, 1 });


        private string PDNF {
            get {
                return GetPDNF();
            }
        }
        List<char> namesVariables = new List<char>();
                
        private Function (int countVariables) {
            this.countVariables = countVariables;

            function = new bool[(int)Math.Pow(2, countVariables)];
            functionInt = new int[(int)Math.Pow(2, countVariables)];

            truthTable = GetTableTruthBinary(countVariables);
        }
        private Function(int[] func) : this((int)Math.Log(func.Length, 2)) {
            functionInt = func;

            bool[] f = new bool[func.Length];
            for (int i = 0; i < f.Length; i++) {
                f[i] = func[i] == 1;
            }
            function = f;
            
            for (int i = 0; i < countVariables; i++) {
                namesVariables.Add((char)('x' + i));
            }

        }
        public Function(int[] func, char[] names) : this(func) {
            namesVariables = new List<char>(names);
        }

        private static string GetBinaryNumber(int num, int countVariables) {
            string ConvertNumber(string number, int count) {
                string tmp = "";
                for (int i = number.Length; i < count; i++) {
                    tmp += "0";
                }
                return tmp + number;
            }

            string result = "";
            do {
                result = (num % 2) + result;
                num = num / 2;
            } while (num > 0);

            return ConvertNumber(result, (int)Math.Pow(2, countVariables));
        }
        private bool[,] GetTableTruth(int countVariables) {

            //int rows = (int)Math.Pow(2, countVariables);
            bool[,] table = new bool[rows, countVariables];

            bool tmp;
            int keyIndex;
            for (int j = 1; j <= countVariables; j++) {
                tmp = false;
                keyIndex = (int)Math.Pow(2, j - 1) + 1;

                for (int i = 0; i < rows; i++) {

                    if ((i + 1) == keyIndex) {
                        tmp = !tmp;
                        keyIndex += (int)Math.Pow(2, j - 1);
                    }

                    truthTable[i, countVariables - j] = tmp;

                }
            }

            return table;

        }
        private bool[,] GetTableTruthBinary(int countVariables) {

            //int rows = (int)Math.Pow(2, countVariables);
            bool[,] table = new bool[rows, countVariables];
            
            for (int i = 0; i < rows; i++) {
                string binaryNumber = GetBinaryNumber(i, countVariables);

                for (int j = 0; j < countVariables; j++) {
                    table[i, j] = int.Parse(binaryNumber[j] + "") == 1;
                }
            }

            return table;
        }

        public string TruthTable() {
            int IsTrue(bool varialbe) {
                return varialbe ? 1 : 0;
            }

            string table = "";

            for (int j = 0; j < countVariables; j++) {
                table += "|\t" + namesVariables[j] + "\t";
            }
            table += "|\t" + "F" + "\t|" + Environment.NewLine + Environment.NewLine;
            
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < countVariables; j++) {
                    table += "|\t" + IsTrue(truthTable[i, j]) + "\t";
                }
                table += "|\t" + IsTrue(function[i]) + "\t|" + Environment.NewLine;
            }

            return table;
        }
        private string GetPDNF() {
            string pdnf = "";

            string Implicant(int index) {
                string implicant = "";
                for (int i = 0; i < countVariables; i++) {
                    implicant += namesVariables[i] + (truthTable[index, i] ? "" : overline.ToString());
                }
                return implicant;
            }

            for (int i = 0; i < function.Length; i++) {
                if (function[i]) {
                    pdnf += Implicant(i) + (i == function.Length - 1 ? "" : " ∨ ");
                }
            }

            return pdnf;
        }

        public Function GetDualFunction() {
            int[] func = new int[rows];
            for (int i = 0; i < rows; i++) {
                func[i] = function[rows - 1 - i] ? 0 : 1;
            }
            
            return new Function(func);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override bool Equals(object obj) {
            if (countVariables != ((Function)obj).countVariables)
                return false;

            for (int i = 0; i < countVariables; i++) {
                if (function[i] != ((Function)obj).function[i])
                    return false;
            }

            return true;
        }
        public static bool operator ==(Function func1, Function func2) {
            return func1.Equals(func2);
        }
        public static bool operator !=(Function func1, Function func2) {
            return !func1.Equals(func2);
        }

        public static bool Implication(bool x, bool y) {
            return !x || y;
        }
        public static bool ArrowPier(bool x, bool y) {
            return !(x || y);
        }
        public static bool ShefferStroke(bool x, bool y) {
            return !(x && y);
        }

        public static bool[,] AllFunction(int countVariables) {
            bool[,] allFunc = new bool[(int)Math.Pow(2, 2 * countVariables), (int)Math.Pow(2, countVariables)];

            for (int i = 0; i < (int)Math.Pow(2, 2 * countVariables); i++) {
                string binaryNumber = GetBinaryNumber(i, countVariables);

                for (int j = 0; j < (int)Math.Pow(2, countVariables); j++) {
                    allFunc[i, j] = int.Parse(binaryNumber[j] + "") == 1;
                }
            }

            return allFunc;
        }
    }
}
