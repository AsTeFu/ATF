namespace ATF {
    using System;

    namespace RationalNumbers {
        public sealed class Rational {
            public readonly long Numerator;
            public readonly long Denominator;
            public readonly int sign;

            public Rational(long numerator, long denominator = 1) {
                Numerator = Math.Abs(numerator);
                if (denominator == 0)
                    throw new Exception("Divided by zero");
                Denominator = Math.Abs(denominator); //кидать ексепшин при нуле?

                sign = numerator * denominator >= 0 ? 1 : -1;

                var rational = Reduce(Numerator, Denominator);
                Numerator = rational.Item1;
                Denominator = rational.Item2;
            }
            public Rational(double real) {
                string[] num = real.ToString().Split(new string[] { ",", "." }, StringSplitOptions.RemoveEmptyEntries);
                long numerator, denominator;

                if (num.Length == 2) {
                    denominator = (long)Math.Pow(10, num[1].Length);

                    long tmp = long.Parse(num[0]);
                    numerator = ((Math.Abs(tmp) * denominator) + long.Parse(num[1])) * Math.Sign(tmp);
                }
                else {
                    numerator = long.Parse(num[0]);

                    denominator = 1;
                }
                sign = Math.Sign(numerator);
                numerator = Math.Abs(numerator);

                var rational = Reduce(numerator, denominator);
                Numerator = rational.Item1;
                Denominator = rational.Item2;
            }

            public (long, long) Reduce(long Numerator, long Denominator) {
                long gcd = GCD(Numerator, Denominator);
                Numerator /= gcd;
                Denominator /= gcd;

                return (Numerator, Denominator);
            }

            private static long GCD(long numerator, long denominator) {
                if (denominator == 0)
                    return numerator;
                return GCD(denominator, numerator % denominator);
            }
            private static long SCM(long num1, long num2) {
                return num1 * num2 / GCD(num1, num2);
            }

            public static implicit operator Rational(long num) {
                return new Rational(num);
            }
            public static implicit operator Rational(double num) {
                return new Rational(num);
            }
            public static implicit operator Rational((long, long) num) {
                return new Rational(num.Item1, num.Item2);
            }

            public static explicit operator double(Rational rational) {
                return rational.sign * (double)rational.Numerator / rational.Denominator;
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

            public static bool operator >=(Rational rational1, Rational rational2) {
                return rational1.Numerator * rational1.sign * rational2.Denominator >= rational2.Numerator * rational2.sign * rational1.Denominator;
            }
            public static bool operator <=(Rational rational1, Rational rational2) {
                return rational1.Numerator * rational1.sign * rational2.Denominator <= rational2.Numerator * rational2.sign * rational1.Denominator;
            }

            public static bool TryParse(string s, out Rational number) {
                long num, den;
                number = null;

                string[] str = s.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (str.Length == 1 && long.TryParse(str[0], out num)) {
                    number = new Rational(num, 1);
                    return true;
                }
                else if (str.Length == 2 && long.TryParse(str[0], out num) && long.TryParse(str[1], out den) && den != 0) {
                    number = new Rational(num, den);
                    return true;
                }
                else if (str.Length == 1 && double.TryParse(str[0].Replace('.', ','), out double numD)) {
                    number = new Rational(numD);
                    return true;
                }

                return false;
            }
            public static Rational Parse(string s) {
                long den = 1;
                try {
                    string[] str = s.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if (str.Length == 1) {
                        if (long.TryParse(str[0], out long num)) {
                            return new Rational(num, den);
                        }
                        else if (double.TryParse(str[0], out double numD)) {
                            return new Rational(numD);
                        }
                        else {
                            throw new FormatException("Входная строка имела неверный формат");
                        }
                    }
                    else if (str.Length == 2) {
                        long num = long.Parse(str[0]);
                        den = long.Parse(str[1]);
                        return new Rational(num, den);
                    }
                    throw new FormatException("Входная строка имела неверный формат");
                }
                catch {
                    throw new FormatException("Входная строка имела неверный формат");
                }
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

        public static class MathRational {
            public static Rational Abs(Rational number) {
                return number > 0 ? number : -number;
            }
            public static Rational Floor(Rational number) {
                return new Rational((long)Math.Floor((decimal)number.Numerator / number.Denominator * number.sign));
            }
            public static Rational Ceiling(Rational number) {
                return new Rational((long)Math.Ceiling((decimal)number.Numerator / number.Denominator * number.sign));
            }

            public static Rational Min(Rational a, Rational b) {
                if (a < b)
                    return a;
                else return b;
            }
            public static Rational Min(Rational a, Rational b, Rational c) {
                Rational min = Min(a, b);
                if (min < c)
                    return min;
                else return c;
            }
            public static Rational Min(params Rational[] numbers) {
                if (numbers != null) {
                    Rational min = numbers[0];

                    for (int i = 0; i < numbers.Length; i++) {
                        if (numbers[i] < min) {
                            min = numbers[i];
                        }
                    }

                    return min;
                }
                return null;
            }

            public static Rational Max(Rational a, Rational b) {
                if (a > b)
                    return a;
                else return b;
            }
            public static Rational Max(Rational a, Rational b, Rational c) {
                Rational max = Max(a, b);
                if (max > c)
                    return max;
                else return c;
            }
            public static Rational Max(params Rational[] numbers) {
                if (numbers != null) {
                    Rational max = numbers[0];

                    for (int i = 0; i < numbers.Length; i++) {
                        if (numbers[i] > max) {
                            max = numbers[i];
                        }
                    }

                    return max;
                }
                else return null;
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
                return new Rational((long)rational);
            }
        }
        
    }
}
