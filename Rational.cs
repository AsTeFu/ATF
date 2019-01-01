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

            sign = numerator * denominator >= 0 ? 1 : -1;

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
        public static Rational operator -(Rational rational) {
            return rational * -1;
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

        public static bool TryParse(string s, out Rational number) {
            long num, den;
            number = new Rational(1);

            string[] str = s.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (str.Length == 1 && long.TryParse(str[0], out num)) {
                number = new Rational(num, 1);
                return true;
            }
            else if (str.Length == 2 && long.TryParse(str[0], out num) && long.TryParse(str[1], out den) && den != 0) {
                number = new Rational(num, den);
                return true;
            }

            return false;
        }
        public static Rational Parse(string s) {
            long num, den = 1;
            Rational number;
            try {
                string[] str = s.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (str.Length == 1) {
                    num = long.Parse(str[0]);
                    number = new Rational(num, den);
                    return number;
                } else if (str.Length == 2) {
                    num = long.Parse(str[0]);
                    den = long.Parse(str[1]);
                    number = new Rational(num, den);
                    return number;
                }
                throw new Exception();
            } catch {
                throw new Exception();
            }
        }

        public static Rational Abs(Rational number) {
            return number > 0 ? number : -number;
        }
        public static Rational Floor(Rational number) {
            return new Rational((long)Math.Floor((decimal)number.Numerator / number.Denominator * number.sign));
        }
        public static Rational Ceiling(Rational number) {
            return new Rational((long)Math.Ceiling((decimal)number.Numerator / number.Denominator * number.sign));
        }
        public static Rational Min(Rational[] numbers) {
            Rational min = numbers[0];

            for (int i = 0; i < numbers.Length; i++) {
                if (numbers[i] < min) {
                    min = numbers[i];
                }
            }

            return min;
        }
        public static Rational Max(Rational[] numbers) {
            Rational max = numbers[0];

            for (int i = 0; i < numbers.Length; i++) {
                if (numbers[i] > max) {
                    max = numbers[i];
                }
            }

            return max;
        }
        public static Rational Pow(Rational a, long b) {
            if (b >= 0)
                return new Rational((long)Math.Pow(a.sign, b) * (long)Math.Pow(a.Numerator, b), (long)Math.Pow(a.Denominator, b));
            else return new Rational((long)Math.Pow(a.sign, -b) * (long)Math.Pow(a.Denominator, -b), (long)Math.Pow(a.Numerator, -b));
        }
        public static int Sign(Rational rational) {
            return rational < 0 ? -1 : (rational == 0 ? 0 : 1);
        }
        public static Rational Truncate(Rational rational) {
            return new Rational((long)Math.Truncate((decimal)rational.Numerator / rational.Denominator * rational.sign), 1);
        }
        
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override string ToString() {
            return $"{(sign == 1 ? "" : "-")}{Numerator}{(Denominator == 1 ? "" : "//" + Denominator)}";
        }
    }
}
