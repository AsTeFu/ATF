namespace ATF {
    using System;
    namespace Complex {
        public class Complex {
            private static readonly Complex ImaginaryUnit = new Complex(0, 1);

            public Complex Conjugate {
                get {
                    return (Real, -Imaginary);
                }
            }

            public readonly double Real;
            public readonly double Imaginary;

            public Complex(double real, double imaginary = 0) {
                Real = real;
                Imaginary = imaginary;
            }

            public static implicit operator Complex((double, double) complex) {
                return new Complex(complex.Item1, complex.Item2);
            }
            public static implicit operator Complex(double real) {
                return new Complex(real);
            }

            public static bool operator ==(Complex complex1, Complex complex2) {
                return complex1.Real == complex2.Real && complex1.Imaginary == complex2.Imaginary;
            }
            public static bool operator !=(Complex complex1, Complex complex2) {
                return !(complex1 == complex2);
            }

            public static Complex operator +(Complex complex1, Complex complex2) {
                return (complex1.Real + complex2.Real, complex1.Imaginary + complex2.Imaginary);
            }
            public static Complex operator *(Complex complex1, Complex complex2) {
                return (complex1.Real * complex2.Real - complex1.Imaginary * complex2.Imaginary, complex1.Real * complex2.Imaginary + complex1.Imaginary * complex2.Real);
            }

            public static Complex operator -(Complex complex1, Complex complex2) {
                return complex1 + (-1 * complex2);
            }
            public static Complex operator /(Complex complex1, Complex complex2) {
                return ((complex1.Real * complex2.Real + complex1.Imaginary * complex2.Imaginary) / (complex2.Real * complex2.Real + complex2.Imaginary * complex2.Imaginary),
                        (complex1.Imaginary * complex2.Real - complex1.Real * complex2.Imaginary) / (complex2.Real * complex2.Real + complex2.Imaginary * complex2.Imaginary));
            }

            public static Complex operator -(Complex complex) {
                return complex * -1;
            }

            public static double Abs(Complex complex) {
                return Math.Sqrt(complex.Real * complex.Real + complex.Imaginary * complex.Imaginary);
            }

            public override bool Equals(object obj) {
                return base.Equals(obj);
            }
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            public override string ToString() {
                return $"{Real} {(Imaginary > 0 ? "+" : "-")} {Math.Abs(Imaginary)}i";
            }
        }
    }
}
