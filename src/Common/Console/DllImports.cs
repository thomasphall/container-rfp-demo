using System.Runtime.InteropServices;

namespace Common.Console
{
    public static class DllImports
    {
        // imports required for a Windows container to successfully notice when a "docker stop" command
        // has been run and allow for a graceful shutdown of the endpoint
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine handler, bool add);
    }
}