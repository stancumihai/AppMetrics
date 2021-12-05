using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    public static class ConsoleUtility
    {

        private const string Kernel32DllName = "kernel32.dll";

        [DllImport(Kernel32DllName)]
        public static extern bool AllocConsole();
    }
}