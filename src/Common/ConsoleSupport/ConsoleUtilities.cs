using System;
using System.Threading.Tasks;

namespace Common.ConsoleSupport
{
    public static class ConsoleUtilities
    {
        public static async Task WriteAsyncWithColor(ConsoleColor consoleColor, object value)
        {

            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            await Console.Out.WriteAsync(value.ToString()).ConfigureAwait(false);
            Console.ForegroundColor = originalColor;
        }

        public static async Task WriteLineAsyncWithColor(ConsoleColor consoleColor, object value)
        {

            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            await Console.Out.WriteLineAsync(value.ToString()).ConfigureAwait(false);
            Console.ForegroundColor = originalColor;
        }

        public static void WriteLineWithColor(ConsoleColor consoleColor, object value)
        {

            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(value.ToString());
            Console.ForegroundColor = originalColor;
        }

        public static void WriteWithColor(ConsoleColor consoleColor, object value)
        {

            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            Console.Write(value.ToString());
            Console.ForegroundColor = originalColor;
        }
    }
}