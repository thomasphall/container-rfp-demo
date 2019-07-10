// ---------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleUtilities.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// Copyright 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------

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
