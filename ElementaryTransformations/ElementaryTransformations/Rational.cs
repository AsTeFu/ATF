using System;

namespace RationalNumbers {
    
    class Rational  {

        private long Numerator;
        private long Denominator;
        private int sign;
                
        public Rational(long numerator, long denominator = 1) {
            Numerator = Math.Abs(numerator);
            if (denominator == 0)
                throw new Exception("Divided by zero");
            Denominator = Math.Abs(denominator); //кидать ексепшин при нуле?

            sign = numerator * denominator > 0 ? 1 : -1;

            Reduce();
        }
        
        private void Reduce() {
            long gcd = GCD(Numerator, Denominator);
            Numerator /= gcd;
            Denominator /= gcd;
        }

        private static long GCD(long numerator, long denominator) {
            if (denominator == 0)
                return numerator;
            return GCD(denominator, numerator % denominator);
        }
        private static long SCM(long num1, long num2) {
            return num1 * num2 / GCD(num1, num2);
        }

        public static implicit operator Rational (long num) {
            return new Rational(num);
        }

        public static Rational operator +(Rational rational1, Rational rational2) {
            long scm = SCM(rational1.Denominator, rational2.Denominator);
            return new Rational((rational1.Numerator * rational1.sign * (scm / rational1.Denominator)) 
                              + (rational2.Numerator * rational2.sign * (scm / rational2.Denominator)), scm);
        }

        public static Rational operator -(Rational rational1, Rational rational2) {
            long scm = SCM(rational1.Denominator, rational2.Denominator);
            return new Rational((rational1.Numerator * rational1.sign * (scm / rational1.Denominator))
                              - (rational2.Numerator * rational2.sign * (scm / rational2.Denominator)), scm);
        }

        public static Rational operator *(Rational rational1, Rational rational2) {
            return new Rational(rational1.Numerator * rational2.Numerator * rational1.sign * rational2.sign, rational1.Denominator * rational2.Denominator);
        }

        public static Rational operator /(Rational rational1, Rational rational2) {
            return new Rational(rational1.Numerator * rational2.Denominator * rational1.sign * rational2.sign, rational1.Denominator * rational2.Numerator);
        }

        public static Rational operator ++(Rational rational) {
            return rational + 1;
        }
        public static Rational operator --(Rational rational) {
            return rational - 1;
        }

        public static bool operator ==(Rational rational1, Rational rational2) {
            return rational1.Numerator == rational2.Numerator && rational1.Denominator == rational2.Denominator && rational1.sign == rational2.sign;
        }
        public static bool operator !=(Rational rational1, Rational rational2) {
            return !(rational1 == rational2);
        }

        public static bool operator >(Rational rational1, Rational rational2) {
            return rational1.Numerator * rational1.sign * rational2.Denominator > rational2.Numerator * rational2.sign * rational1.Denominator;
        }
        public static bool operator <(Rational rational1, Rational rational2) {
            return rational1.Numerator * rational1.sign * rational2.Denominator < rational2.Numerator * rational2.sign * rational1.Denominator;
        }

        public static bool operator >=(Rational rational1, Rational rational2)
        {
            return rational1.Numerator * rational1.sign * rational2.Denominator >= rational2.Numerator * rational2.sign * rational1.Denominator;
        }
        public static bool operator <=(Rational rational1, Rational rational2)
        {
            return rational1.Numerator * rational1.sign * rational2.Denominator <= rational2.Numerator * rational2.sign * rational1.Denominator;
        }


        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override string ToString() {
            return $"{(sign == 1 || Numerator == 0 ? "" : "-")}{Numerator}{(Denominator == 1 ? "" : "//" + Denominator)}";
        }
    }

    class MathRational {
        public static Rational Abs(Rational number) {
            return number > 0 ? number : -1 * number;
        }
    }
}
