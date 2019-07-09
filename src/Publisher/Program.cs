// ---------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Common.ConsoleSupport;

namespace Publisher
{
    public class Program
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);

        public static async Task Main()
        {
            // Completely wrong, intolerant way to way for dependencies.
            Thread.Sleep(10000);

            ConfigureExitLogic();
            var host = await StartHost();
            SetConsoleTitle(host);
            await EmitStartupSuccessMessages();
            await WaitAndStop(host);
        }

        private static void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            _semaphore.Release();
        }

        private static void ConfigureExitLogic()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                DllImports.SetConsoleCtrlHandler(ConsoleCtrlCheck, true);
            }
            else
            {
                Console.CancelKeyPress += CancelKeyPress;
                AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            }
        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            _semaphore.Release();

            return true;
        }

        private static async Task EmitStartupSuccessMessages()
        {
            await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Green, "NServiceBus endpoint connected.");
            await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Green, "Application running.");
            await Console.Out.WriteLineAsync();
            await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Yellow, "Press Ctrl+C to exit...");
        }

        private static void ProcessExit(object sender, EventArgs e)
        {
            _semaphore.Release();
        }

        private static void SetConsoleTitle(Host host)
        {
            Console.Title = host.EndpointName;
        }

        private static async Task<Host> StartHost()
        {
            var host = new Host();
            await host.Start();

            return host;
        }

        private static async Task WaitAndStop(Host host)
        {
            await _semaphore.WaitAsync();
            await host.Stop();
        }
    }
}
