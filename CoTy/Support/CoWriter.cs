using System;
using System.Runtime.InteropServices;

namespace CoTy.Support
{
    public class CoWriter
    {
        private readonly int left;
        private readonly int top;
        private readonly int width;
        private readonly int height;
        private readonly int right;
        private readonly int bottom;

        public static void Setup(int left, int top, int width, int height)
        {
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);
            GetConsoleMode(handle, out var mode);
            mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            SetConsoleMode(handle, mode);

            G.C = new CoWriter(left, top, width, height);
        }

        private CoWriter(int left, int top, int width, int height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            this.right = this.left + this.width;
            this.bottom = this.top + this.height;
        }

        public bool IsInputRedirected => Console.IsInputRedirected;

        public void Write(string s)
        {
            Console.Write(s);
        }

        public void WriteLine(string s)
        {
            Console.WriteLine(s);
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        const int STD_OUTPUT_HANDLE = -11;

        [DllImport("kernel32.dll", SetLastError = true)]    
        static extern IntPtr GetStdHandle(int nStdHandle);    
        [DllImport("kernel32.dll")]    
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);    
        [DllImport("kernel32.dll")]    
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    }
}
