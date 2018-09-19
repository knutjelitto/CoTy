using System;
using System.Linq;
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
            s = string.Join("", s.Where(c => !(char.IsControl(c) || char.IsLowSurrogate(c) || char.IsHighSurrogate(c))));

            var rest = this.right - Console.CursorLeft;

            while (s.Length > rest)
            {
                var part = s.Substring(0, rest);
                s = s.Substring(rest);
                Console.Write(part);
                if (Console.CursorTop + 1 >= this.bottom)
                {
                    ScrollUp();
                    Console.SetCursorPosition(this.left, Console.CursorTop);
                }
                else
                {
                    Console.SetCursorPosition(this.left, Console.CursorTop + 1);
                }
            }

            if (s.Length > 0)
            {
                Console.Write(s);
            }
        }

        public void WriteLine(string s)
        {
            Write(s);
            NextLine();
        }

        public void WriteLine()
        {
            NextLine();
        }

        public void Clear()
        {
            for (var row = this.top; row < this.bottom; ++row)
            {
                Console.SetCursorPosition(this.left, row);
                Console.Write(new string(':', this.width));
            }
            Console.SetCursorPosition(this.left, this.top);
        }

        private void NextLine()
        {
            if (Console.CursorTop + 1 >= this.bottom)
            {
                ScrollUp();
                Console.SetCursorPosition(this.left, Console.CursorTop);
            }
            else
            {
                Console.SetCursorPosition(this.left, Console.CursorTop + 1);
            }
        }

        private void ScrollUp()
        {
            Console.MoveBufferArea(this.left, this.top + 1, this.width, this.height - 1, this.left, this.top);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const int STD_OUTPUT_HANDLE = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);    
        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);    
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    }
}
