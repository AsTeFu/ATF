namespace ATF {
    namespace Matrix {
        using RationalNumbers;
        using System;

        public class MatrixRational {
            public int Lines {
                get {
                    return matrix.GetLength(0);
                }
            }
            public int Columns {
                get {
                    return matrix.GetLength(1);
                }
            }

            private readonly Rational[,] matrix;

            private MatrixRational(int lines, int columns) {
                matrix = new Rational[lines, columns];
            }
            public MatrixRational(Rational[,] data) {
                matrix = data;
            }

            public Rational this[int i, int j] {
                get {
                    return matrix[i, j];
                }
                set {
                    matrix[i, j] = value;
                }
            }
            
            public static MatrixRational operator +(MatrixRational left, MatrixRational right) {
                if (left.Lines == right.Lines && left.Columns == right.Columns) {
                    MatrixRational newMatrix = new MatrixRational(left.Lines, left.Columns);

                    for (int i = 0; i < newMatrix.Lines; i++) {
                        for (int j = 0; j < newMatrix.Columns; j++) {
                            newMatrix.matrix[i, j] = left.matrix[i, j] + right.matrix[i, j];
                        }
                    }

                    return newMatrix;
                }
                else return null;
            }
            public static MatrixRational operator -(MatrixRational left, MatrixRational right) {
                return left + (right * -1);
            }
            public static MatrixRational operator -(MatrixRational operand) {
                return operand * (-1);
            }

            public static MatrixRational operator *(Rational scalar, MatrixRational matrix) {
                MatrixRational newMatrix = new MatrixRational(matrix.Lines, matrix.Columns);
                for (int i = 0; i < newMatrix.Lines; i++) {
                    for (int j = 0; j < newMatrix.Columns; j++) {
                        newMatrix.matrix[i, j] = matrix.matrix[i, j] * scalar;
                    }
                }
                return newMatrix;
            }
            public static MatrixRational operator *(MatrixRational matrix, Rational scalar) {
                return scalar * matrix;
            }

            public static MatrixRational operator /(Rational scalar, MatrixRational matrix) {
                return (1 / scalar) * matrix;
            }
            public static MatrixRational operator /(MatrixRational matrix, Rational scalar) {
                return scalar / matrix;
            }

            public MatrixRational Transpose() {
                Rational[,] newMatrix = new Rational[Columns, Lines];

                int iMax = Lines, jMax = Columns;
                for (int i = 0; i < iMax; i++) {
                    for (int j = 0; j < jMax; j++) {
                        newMatrix[j, i] = matrix[i, j];
                    }
                }

                return new MatrixRational(newMatrix);
            }

            public static MatrixRational operator *(MatrixRational left, MatrixRational right) {
                if (left.Columns == right.Lines) {
                    MatrixRational newMatrix = new MatrixRational(left.Lines, right.Columns);

                    for (int i = 0; i < newMatrix.Lines; i++) {
                        for (int j = 0; j < newMatrix.Columns; j++) {

                            for (int k = 0; k < left.Columns; k++) {
                                newMatrix.matrix[i, j] += left.matrix[i, k] * right.matrix[k, j];
                            }

                        }
                    }

                    return newMatrix;

                }
                else return null;
            }

            public static bool operator ==(MatrixRational left, MatrixRational right) {
                if (left.Lines == right.Lines && right.Columns == right.Columns) {
                    for (int i = 0; i < left.Lines; i++) {
                        for (int j = 0; j < left.Columns; j++) {
                            if (left[i, j] != right[i, j]) {
                                return false;
                            }
                        }
                    }
                }
                else return false;

                return true;
            }
            public static bool operator !=(MatrixRational left, MatrixRational right) {
                return !(left == right);
            }
            
            public override string ToString() {
                string tmp = "";

                for (int i = 0; i < Lines; i++) {
                    for (int j = 0; j < Columns; j++) {
                        tmp += this[i, j] + "\t";
                    }
                    tmp += "\n";
                }

                return tmp;
            }
            public override bool Equals(object obj) {
                return base.Equals(obj);
            }
            public override int GetHashCode() {
                return base.GetHashCode();
            }
        }
        public class Matrix {
            public int Lines {
                get {
                    return matrix.GetLength(0);
                }
            }
            public int Columns {
                get {
                    return matrix.GetLength(1);
                }
            }

            private readonly double[,] matrix;

            private Matrix(int lines, int columns) {
                matrix = new double[lines, columns];
            }
            public Matrix(double[,] data) {
                matrix = data;
            }

            public double this[int i, int j] {
                get {
                    return matrix[i, j];
                }
                set {
                    matrix[i, j] = value;
                }
            }

            public bool isSymmetrical {
                get {
                    return this == Transpose();
                }
            }
            public bool isSkewSymmetric {
                get {
                    return this == -Transpose();
                }
            }
            public bool isDiagonal {
                get {
                    bool isD = true;

                    for (int i = 0; i < Lines; i++) {
                        for (int j = 0; j < Columns; j++) {
                            if (i != j && matrix[i, j] != 0) {
                                isD = false;
                                break;
                            }
                        }
                    }

                    return isSquare && isD;
                }
            }
            public bool isUpper {
                get {
                    bool isU = true;

                    for (int i = 0; i < Lines; i++) {
                        for (int j = 0; j < Columns; j++) {
                            if (i > j && matrix[i, j] != 0) {
                                isU = false;
                                break;
                            }
                        }
                    }

                    return isSquare && isU;
                }
            }
            public bool isDown {
                get {
                    bool isD = true;

                    for (int i = 0; i < Lines; i++) {
                        for (int j = 0; j < Columns; j++) {
                            if (i < j && matrix[i, j] != 0) {
                                isD = false;
                                break;
                            }
                        }
                    }

                    return isSquare && isD;
                }
            }
            public bool isSquare {
                get {
                    return Lines == Columns;
                }
            }

            public static Matrix operator +(Matrix left, Matrix right) {
                if (left.Lines == right.Lines && left.Columns == right.Columns) {
                    Matrix newMatrix = new Matrix(left.Lines, left.Columns);

                    for (int i = 0; i < newMatrix.Lines; i++) {
                        for (int j = 0; j < newMatrix.Columns; j++) {
                            newMatrix.matrix[i, j] = left.matrix[i, j] + right.matrix[i, j];
                        }
                    }

                    return newMatrix;
                }
                else return null;
            }
            public static Matrix operator -(Matrix left, Matrix right) {
                return left + (right * -1);
            }
            public static Matrix operator -(Matrix operand) {
                return operand * (-1);
            }

            public static Matrix operator *(double scalar, Matrix matrix) {
                Matrix newMatrix = new Matrix(matrix.Lines, matrix.Columns);
                for (int i = 0; i < newMatrix.Lines; i++) {
                    for (int j = 0; j < newMatrix.Columns; j++) {
                        newMatrix.matrix[i, j] = matrix.matrix[i, j] * scalar;
                    }
                }
                return newMatrix;
            }
            public static Matrix operator *(Matrix matrix, double scalar) {
                return scalar * matrix;
            }

            public static Matrix operator /(double scalar, Matrix matrix) {
                return (1 / scalar) * matrix;
            }
            public static Matrix operator /(Matrix matrix, double scalar) {
                return scalar / matrix;
            }

            public Matrix Transpose() {
                double[,] newMatrix = new double[Columns, Lines];

                int iMax = Lines, jMax = Columns;
                for (int i = 0; i < iMax; i++) {
                    for (int j = 0; j < jMax; j++) {
                        newMatrix[j, i] = matrix[i, j];
                    }
                }

                return new Matrix(newMatrix);
            }

            public static Matrix operator *(Matrix left, Matrix right) {
                if (left.Columns == right.Lines) {
                    Matrix newMatrix = new Matrix(left.Lines, right.Columns);

                    for (int i = 0; i < newMatrix.Lines; i++) {
                        for (int j = 0; j < newMatrix.Columns; j++) {

                            for (int k = 0; k < left.Columns; k++) {
                                newMatrix.matrix[i, j] += left.matrix[i, k] * right.matrix[k, j];
                            }

                        }
                    }

                    return newMatrix;

                }
                else return null;
            }

            public static bool operator ==(Matrix left, Matrix right) {
                if (left.Lines == right.Lines && right.Columns == right.Columns) {
                    for (int i = 0; i < left.Lines; i++) {
                        for (int j = 0; j < left.Columns; j++) {
                            if (left[i, j] != right[i, j]) {
                                return false;
                            }
                        }
                    }
                }
                else return false;

                return true;
            }
            public static bool operator !=(Matrix left, Matrix right) {
                return !(left == right);
            }

            public override string ToString() {
                string tmp = "";

                for (int i = 0; i < Lines; i++) {
                    for (int j = 0; j < Columns; j++) {
                        tmp += this[i, j] + "\t";
                    }
                    tmp += "\n";
                }

                return tmp;
            }
            public override bool Equals(object obj) {
                return base.Equals(obj);
            }
            public override int GetHashCode() {
                return base.GetHashCode();
            }
        }
        
    }
}
