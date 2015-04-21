using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public static class Ensure
    {
        public static void NotNull<T>(T argument, string argumentName) where T : class
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);
        }

        public static void NotNullOrEmpty(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argument))
                throw new ArgumentNullException(argument, argumentName);
        }

        public static void IsExactly(decimal number1, decimal number2, string argumentName)
        {
            if (number1 != number2)
                throw new ArgumentException(argumentName + " does not equal " + number2.ToString(), argumentName);
        }

        public static void Positive(int number, string argumentName)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(argumentName + " should be positive", argumentName);
        }

        public static void Between(int number, int number1, int number2, string argumentName)
        {
            if (number < number1 || number > number2)
                throw new ArgumentOutOfRangeException(argumentName + " should be between " + number1.ToString() + " and " + number2.ToString(), argumentName);
        }

        public static void Positive(decimal number, string argumentName)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(argumentName + " should be positive", argumentName);
        }

        public static void IsPercentage(decimal number, string argumentName)
        {
            if (number < 0.0m || number > 1.0m)
                throw new ArgumentOutOfRangeException(argumentName + " should be between 0% and 100%", argumentName);
        }

        public static void NonNegative(int number, string argumentName)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(argumentName + " should be non negative", argumentName);
        }

        public static void NonNegative(decimal number, string argumentName)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(argumentName + " should be non negative", argumentName);
        }

        public static void NotEmptyGuid(Guid guid, string argumentName)
        {
            if (Guid.Empty == guid)
                throw new ArgumentException(argumentName + " shoud be non-empty GUID", argumentName);
        }
    }
}
