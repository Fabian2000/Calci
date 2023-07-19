using System.Globalization;
using System.Text.RegularExpressions;

namespace CalciMath
{
    public static class Calci
    {
        public static double Calc(string math)
        {
            Validation(math);

            while (math.Contains("("))
            {
                math = CalculateInParenthesis(math);
            }

            while (math.Contains("**"))
            {
                math = Pow(math);
            }

            while (math.Contains("%"))
            {
                math = Mod(math);
            }

            while (math.Contains("/") || math.Contains("*"))
            {
                math = Multiply(math);
                math = Divide(math);
            }

            while ((math.Contains("-") && !math.StartsWith("-")) || math.Contains("+"))
            {
                math = Add(math);
                math = Sub(math);
            }

            return double.Parse(math);
        }

        private static void Validation(string math)
        {
            math = math.Replace(" ", string.Empty);

            if (Regex.IsMatch(math, @"[^\.+%\-\*/\(\)0-9]"))
            {
                Match issue = Regex.Match(math, @"[^\.+%\-\*/\(\)0-9]");
                throw new Exception($"Using invalid symbols is not allowed \"{issue}\"");
            }
            else if (math.Count(x => x == '(') != math.Count(x => x == ')'))
            {
                throw new Exception("A parenthesis was not closed correctly");
            }
            else if (Regex.IsMatch(math, @"\/0([\.+%\-\*/\(\)]|$)"))
            {
                throw new Exception("Dividing a number by 0 is not possible");
            }
            else if (Regex.IsMatch(math, @"[%*-+/]$"))
            {
                throw new Exception("Using an operator without a following number is not allowed");
            }
            else if (math.Contains(","))
            {
                throw new Exception("Using , instead of . is not allowed");
            }
        }

        private static string CalculateInParenthesis(string math)
        {
            Regex regex = new Regex(@"(\(([+\-\*/0-9]+)\))");

            Match match = regex.Match(math);
            string tempMath = match.Groups[2].Value;

            tempMath = Calc(tempMath).ToString();

            math = regex.Replace(math, tempMath);

            return math;
        }

        private static string Pow(string math)
        {
            Regex regex = new Regex(@"(-?[0-9\.]+(\*\*|\*\*-)[0-9\.]+)");

            return regex.Replace(math, result =>
            {
                string[] numbers = result.Value.Split('*');

                double pow = Math.Pow(double.Parse(numbers[0], CultureInfo.InvariantCulture), double.Parse(numbers[2], CultureInfo.InvariantCulture));
                return pow.ToString();
            }, 1);
        }

        private static string Mod(string math)
        {
            Regex regex = new Regex(@"(-?[0-9\.]+(%|%-)[0-9\.]+)");

            return regex.Replace(math, result =>
            {
                string[] numbers = result.Value.Split('%');

                double mod = double.Parse(numbers[0], CultureInfo.InvariantCulture) % double.Parse(numbers[1], CultureInfo.InvariantCulture);
                return mod.ToString();
            }, 1);
        }

        private static string Multiply(string math)
        {
            Regex regex = new Regex(@"(-?[0-9\.]+(\*|\*-)[0-9\.]+)");

            return regex.Replace(math, result =>
            {
                string[] numbers = result.Value.Split('*');

                double mod = double.Parse(numbers[0], CultureInfo.InvariantCulture) * double.Parse(numbers[1], CultureInfo.InvariantCulture);
                return mod.ToString();
            }, 1);
        }

        private static string Divide(string math)
        {
            Regex regex = new Regex(@"(-?[0-9\.]+(\/|\/-)[0-9\.]+)");

            return regex.Replace(math, result =>
            {
                string[] numbers = result.Value.Split('/');

                double mod = double.Parse(numbers[0], CultureInfo.InvariantCulture) / double.Parse(numbers[1], CultureInfo.InvariantCulture);
                return mod.ToString();
            }, 1);
        }

        private static string Add(string math)
        {
            Regex regex = new Regex(@"(-?[0-9\.]+(\+|\+-)[0-9\.]+)");

            return regex.Replace(math, result =>
            {
                string[] numbers = result.Value.Split('+');

                double mod = double.Parse(numbers[0], CultureInfo.InvariantCulture) + double.Parse(numbers[1], CultureInfo.InvariantCulture);
                return mod.ToString();
            }, 1);
        }

        private static string Sub(string math)
        {
            Regex regex = new Regex(@"(-?[0-9\.]+(\-|\--)[0-9\.]+)");

            return regex.Replace(math, result =>
            {
                string[] numbers = result.Value.Split('-');

                double mod = double.Parse(numbers[0], CultureInfo.InvariantCulture) - double.Parse(numbers[1], CultureInfo.InvariantCulture);
                return mod.ToString();
            }, 1);
        }
    }
}