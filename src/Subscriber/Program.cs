using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Common.ConsoleSupport;

namespace Subscriber
{
    public class Program
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);

        public static async Task Main()
        {
            ConfigureExitLogic();
            SetConsoleTitle();
            var host = await StartHost();
            await EmitStartupSuccessMessages();
            await WaitAndStop(host);
        }

        private static async Task WaitAndStop(Host host)
        {
            await _semaphore.WaitAsync();
            await host.Stop();
        }

        private static async Task EmitStartupSuccessMessages()
        {
            await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Green, "NServiceBus endpoint connected.");
            await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Green, "Application running.");
            await Console.Out.WriteLineAsync();
            await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Yellow, "Press Ctrl+C to exit...");
        }

        private static async Task<Host> StartHost()
        {
            var host = new Host();
            await host.Start();
            return host;
        }

        private static void SetConsoleTitle()
        {
            Console.Title = Host.EndpointName;
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

        private static void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            _semaphore.Release();
        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            _semaphore.Release();

            return true;
        }

        private static void ProcessExit(object sender, EventArgs e)
        {
            _semaphore.Release();
        }
    }
}