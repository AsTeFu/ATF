using System;
using System.Collections.Generic;
using System.Text;

namespace ATF {
    namespace NumeralSystem {
        public sealed class NumeralSystem {
            public readonly decimal DecimalNumber;
            public int Accuracy {
                get {
                    return accuracy;
                }
                set {
                    if (value > minAccuracy && value <= maxAccuracy) {
                        accuracy = value;
                    }
                }
            }
            private int accuracy;
            private readonly int minAccuracy = 0;
            private readonly int maxAccuracy = 200;


            private NumeralSystem(decimal decimalNumber) {
                Accuracy = 25;
                DecimalNumber = Math.Abs(decimalNumber);
            }
            public NumeralSystem(string number, int system) {
                Accuracy = 25;
                DecimalNumber = Math.Abs(ToDecimalNumber(number, system));
            }
            public string TranslateSystem(int system) {
                if (system < 1)
                    return null;

                string number = "";

                decimal decimalInteger = Math.Floor(DecimalNumber);
                decimal decimalFractional = DecimalNumber - decimalInteger;

                while (decimalInteger > 0) {
                    number = GetSymbolFromNum(decimalInteger % system) + number;
                    decimalInteger = Math.Floor(decimalInteger / system);
                }

                if (decimalFractional > 0) {
                    number += ".";

                    for (int i = 0; i < Accuracy; i++) {
                        number += GetSymbolFromNum(Math.Floor(decimalFractional * system));
                        decimalFractional = (decimalFractional * system) - Math.Floor(decimalFractional * system);
                    }
                }

                return number;
            }

            private decimal ToDecimalNumber(string number, int system) {
                string[] str = number.Trim('_', ' ').ToUpper().Split(new string[] { ".", "," }, StringSplitOptions.RemoveEmptyEntries);

                string integer = GetValidNumber(str[0], system);
                string fractional = "";
                if (str.Length == 2)
                    fractional = GetValidNumber(str[1], system);

                return GetDecimal(system, integer, fractional);
            }
            private decimal GetDecimal(int system, string integer, string fractional) {
                decimal decimalInteger = 0;

                for (int i = 0; i < integer.Length; i++) {
                    decimalInteger += GetNumFromSymbol(integer[i]) * (decimal)Math.Pow(system, integer.Length - 1 - i);
                }

                for (int i = 0; i < fractional.Length; i++) {
                    decimalInteger += GetNumFromSymbol(fractional[i]) * (decimal)Math.Pow(system, -(i + 1));
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
            private string GetSymbolFromNum(decimal num) {
                if (num < 10)
                    return num.ToString();
                else return ((char)(num - 10 + 'A')).ToString();
            }

            public static implicit operator NumeralSystem(decimal num) {
                return new NumeralSystem(num);
            }
            public static implicit operator NumeralSystem((string, int) num) {
                return new NumeralSystem(num.Item1, num.Item2);
            }

            public static NumeralSystem operator +(NumeralSystem left, NumeralSystem right) {
                return new NumeralSystem(left.DecimalNumber + right.DecimalNumber);
            }
            public static NumeralSystem operator *(NumeralSystem left, NumeralSystem right) {
                return new NumeralSystem(left.DecimalNumber * right.DecimalNumber);
            }
            public static NumeralSystem operator -(NumeralSystem left, NumeralSystem right) {
                return left + (-1 * right);
            }
            public static NumeralSystem operator /(NumeralSystem left, NumeralSystem right) {
                return new NumeralSystem(left.DecimalNumber / right.DecimalNumber);
            }
        }
    }
}
