namespace MatrixATF {
    using RationalNumbers;
    
    class Matrix {

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

        private Rational[,] matrix;

        private Matrix(int lines, int columns) {
            matrix = new Rational[lines, columns];
        }
        public Matrix(Rational[,] data) {
            matrix = data;
        }
        public Matrix() {
            matrix = new Rational[0, 0];
        }

        public Rational this[int i, int j] {
            get {
                return matrix[i, j];
            }
            set {
                matrix[i, j] = value;
            }
        }

        public static Matrix empty {
            get {
                return new Matrix();
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
        
        public static Matrix operator +(Matrix leftOperand, Matrix rightOperand) {
            if (leftOperand.Lines == rightOperand.Lines && leftOperand.Columns == rightOperand.Columns) {
                Matrix newMatrix = new Matrix(leftOperand.Lines, leftOperand.Columns);

                for (int i = 0; i < newMatrix.Lines; i++) {
                    for (int j = 0; j < newMatrix.Columns; j++) {
                        newMatrix.matrix[i, j] = leftOperand.matrix[i, j] + rightOperand.matrix[i, j];
                    }
                }

                return newMatrix;
            } else return new Matrix();
        }

        public static Matrix operator -(Matrix leftOperand, Matrix rigthOperand) {
            return leftOperand + (rigthOperand * -1);
        }
        public static Matrix operator -(Matrix operand) {
            return operand * (-1);
        }
        
        public static Matrix operator *(Rational scalar, Matrix matrix) {
            return scalarMultiplication(scalar, matrix);
        }
        public static Matrix operator *(Matrix matrix, Rational scalar) {
            return scalarMultiplication(scalar, matrix);
        }
        private static Matrix scalarMultiplication(Rational scalar, Matrix matrix) {
            Matrix newMatrix = new Matrix(matrix.Lines, matrix.Columns);
            for (int i = 0; i < newMatrix.Lines; i++) {
                for (int j = 0; j < newMatrix.Columns; j++) {
                    newMatrix.matrix[i, j] = matrix.matrix[i, j] * scalar;
                }
            }
            return newMatrix;
        }

        public static Matrix operator /(Rational scalar, Matrix matrix) {
            return scalarDivision(scalar, matrix);
        }
        public static Matrix operator /(Matrix matrix, Rational scalar) {
            return scalarDivision(scalar, matrix);
        }
        private static Matrix scalarDivision(Rational scalar, Matrix matrix) {
            return scalarMultiplication(1 / scalar, matrix);
        }

        public Matrix Transpose() {
            Rational[,] newMatrix = new Rational[Columns, Lines];

            int iMax = Lines, jMax = Columns;
            for (int i = 0; i < iMax; i++) {
                for (int j = 0; j < jMax; j++) {
                    newMatrix[j, i] = matrix[i, j];
                }
            }

            return new Matrix(newMatrix);
        }

        public static Matrix operator *(Matrix leftOperand, Matrix rigthOperand) {
            if (leftOperand.Columns == rigthOperand.Lines) {

                Matrix newMatrix = new Matrix(leftOperand.Lines, rigthOperand.Columns);

                for (int i = 0; i < newMatrix.Lines; i++) {
                    for (int j = 0; j < newMatrix.Columns; j++) {

                        for (int k = 0; k < leftOperand.Columns; k++) {
                            newMatrix.matrix[i, j] += leftOperand.matrix[i, k] * rigthOperand.matrix[k, j];
                        }
                        
                    }
                }

                return newMatrix;

            } else return new Matrix();
        }

        public static Matrix operator ^(Matrix matrix, int degree) {
            Matrix newMatrix = GetSingleMatrix(matrix.Lines, matrix.Columns);
            if (degree > 0) {
                for (int i = 0; i < degree; i++) {
                    newMatrix *= matrix;
                }

                return newMatrix;
            } else if (degree == 0) {
                return GetSingleMatrix(matrix.Lines, matrix.Columns);
            } else return matrix;
         }

        public static bool operator ==(Matrix leftOperand, Matrix rigthOperand) {
            if (leftOperand.Lines == rigthOperand.Lines && rigthOperand.Columns == rigthOperand.Columns) {
                for (int i = 0; i < leftOperand.Lines; i++) {
                    for (int j = 0; j < leftOperand.Columns; j++) {
                        if (leftOperand[i, j] != rigthOperand[i, j]) {
                            return false;
                        }
                    }
                }
            } else return false;

            return true;
        }
        public static bool operator !=(Matrix leftOperand, Matrix rigthOperand) {
            return !(leftOperand == rigthOperand);
        }

        public static Matrix GetSingleMatrix(int lines, int columns) {
            Rational[,] E = new Rational[lines, columns];

            for (int i = 0; i < lines; i++) {
                for (int j = 0; j < columns; j++) {
                    if (i == j)
                        E[i, j] = 1;
                    else E[i, j] = 0;
                }
            }

            return new Matrix(E);
        }
        
        public string GetElemet(int i, int j) {
            if (i <= Lines && j <= Columns)
                return $"Элемент [{i}, {j}] = {matrix[i - 1, j - 1]}";
            else return "Элемент не найден";
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
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
