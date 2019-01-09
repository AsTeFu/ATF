using System;

namespace ATF {
    namespace Matrix {
        namespace ElementaryTransformations {
            using RationalNumbers;

            public class ElementaryTransformations {
                public MatrixRational data {
                    get; private set;
                }

                public ElementaryTransformations(MatrixRational matrix) {
                    data = matrix;
                }
                
                public void Subtract(int line1, int line2, Rational constant2, Rational constant1) {
                    if (constant2 > 0) {
                        Console.Write($"\tИз строки ({line1 + 1}){(constant1 == 1 ? "" : "умноженной на " + MathRational.Abs(constant1))} вычетаем строку ({line2 + 1}){(constant2 == 1 ? "" : " умноженную на " + MathRational.Abs(constant2))}: ");
                    }
                    else Console.Write($"\tК строке ({line1 + 1}){(constant1 == 1 ? "" : "умноженной на " + MathRational.Abs(constant1))} добавляем строку ({line2 + 1}){(constant2 == 1 ? "" : " умноженную на " + MathRational.Abs(constant2))}: ");
                    Console.WriteLine($"{(constant1 == 1 ? "" : constant1 + " * ") } ({line1 + 1}) {(constant1 > 0 ? "-" : "+")} {(constant2 == 1 ? "" : MathRational.Abs(constant2) + " * ")}({line2 + 1})\n");
                    
                    for (int j = 0; j < data.Columns; j++) {
                        data[line1, j] = data[line1, j] * constant1 - data[line2, j] * constant2;
                    }

                    Console.WriteLine(Show("\t\t"));
                }
                public void Subtract(int line1, int line2, Rational constant2) {
                    Subtract(line1, line2, constant2, 1);
                }
                public void MultiplyConst(int line, Rational constant) {
                    Console.Write($"\tУмножаем строку ({line + 1}) на {constant}: ");
                    Console.WriteLine($"\t{constant} * ({line + 1}):\n");

                    for (int j = 0; j < data.Columns; j++) {
                        data[line, j] *= constant;
                    }

                    Console.WriteLine(Show("\t\t"));
                }
                public void SwapLines(int line1, int line2) {
                    Console.Write($"\tМеняем строки ({line1 + 1}) и ({line2 + 1}) местами: ");
                    Console.WriteLine($"({line1 + 1}) <-> ({line2 + 1}):\n");

                    for (int j = 0; j < data.Columns; j++) {
                        Rational tmp = data[line1, j];
                        data[line1, j] = data[line2, j];
                        data[line2, j] = tmp;
                    }

                    Console.WriteLine(Show("\t\t"));
                }
                public void DeleteZeroLines() {
                    while (zeroLines() != -1) {
                        int zeroLine = zeroLines();
                        deleteZeroLine(zeroLine);
                        Console.WriteLine("\tУдаляем пустую строку");
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
                        data = new MatrixRational(rationals);
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
                                    newMatrix.Subtract(k, i, newMatrix.data[k, keyElement.Item2], 1);
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

                    return new ElementaryTransformations(new MatrixRational(newData));
                }
            }
        }
    }
}
