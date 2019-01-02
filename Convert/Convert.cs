using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeralSystem {
    class Convert {
        public double DecimalNumber {
            get; private set;
        }

        private Convert(double decimalNumber) {
            DecimalNumber = decimalNumber;
        }
        public Convert(string number, int system) {
            DecimalNumber = ToDecimalNumber(number, system);
        }
        public string TranslateSystem(int system) {
            string number = "";

            long decimalInteger = (long)DecimalNumber;
            double decimalFractional = DecimalNumber - decimalInteger;

            while (decimalInteger > 0) {
                number = GetSymbolFromNum(decimalInteger % system) + number;
                decimalInteger /= system;
            }

            if (decimalFractional > 0.00000001d) {
                number += ".";

                for (int i = 0; i < 25; i++) {
                    number += GetSymbolFromNum((long)(decimalFractional * system));
                    decimalFractional = (decimalFractional * system) - (long)(decimalFractional * system);
                }
            }

            return number;
        }
        
        private double ToDecimalNumber(string number, int system) {
            string[] str = number.Trim('_', ' ').ToUpper().Split(new string[] { ".", "," }, StringSplitOptions.RemoveEmptyEntries);

            string integer = GetValidNumber(str[0], system);
            string fractional = "";
            if (str.Length == 2)
                fractional = GetValidNumber(str[1], system);

            return GetDecimal(system, integer, fractional);
        }
        private double GetDecimal(int system, string integer, string fractional) {
            double decimalInteger = 0;

            for (int i = 0; i < integer.Length; i++) {
                decimalInteger += GetNumFromSymbol(integer[i]) * Math.Pow(system, integer.Length - 1 - i);
            }

            for (int i = 0; i < fractional.Length; i++) {
                decimalInteger += GetNumFromSymbol(fractional[i]) * Math.Pow(system, -(i + 1));
            }

            return decimalInteger;
        }
        
        private string GetValidNumber(string original, int system) {
            char[] validCharacter = GetValidCharacter(system);
            string clearNumber = "";
            for (int i = 0; i < original.Length; i++) {
                if (IsValidCharacter(validCharacter, original[i])) {
                    clearNumber += original[i];
                }
                else throw new Exception("Невалидный символ");
            }

            return clearNumber;
        }
        private char[] GetValidCharacter(int system) {
            char[] validCharacter = new char[system];

            for (int i = 0; i < system; i++) {
                if (i < 10)
                    validCharacter[i] = (char)('0' + i);
                else validCharacter[i] = (char)('A' + i - 10);
            }

            return validCharacter;
        }
        private bool IsValidCharacter(char[] validCharacter, char characher) {
            for (int i = 0; i < validCharacter.Length; i++) {
                if (characher == validCharacter[i])
                    return true;
            }

            return false;
        }
        private int GetNumFromSymbol(char symbol) {
            if (int.TryParse(symbol.ToString(), out int num))
                return num;
            else return symbol - 'A' + 10;
        }
        private string GetSymbolFromNum(long num) {
            if (num < 10)
                return num.ToString();
            else return ((char)(num - 10 + 'A')).ToString();
        }

        public static implicit operator Convert(double num) {
            return new Convert(num);
        }
        public static implicit operator Convert((string, int) num) {
            return new Convert(num.Item1, num.Item2);
        }

        public static Convert operator +(Convert left, Convert right) {
            return new Convert(left.DecimalNumber + right.DecimalNumber);
        }
        public static Convert operator *(Convert left, Convert right) {
            return new Convert(left.DecimalNumber * right.DecimalNumber);
        }
        public static Convert operator -(Convert left, Convert right) {
            return left + (-1 * right);
        }
        public static Convert operator /(Convert left, Convert right) {
            return new Convert(left.DecimalNumber / right.DecimalNumber);
        }
    }
}
