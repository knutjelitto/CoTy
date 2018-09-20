using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CoTy.Support
{
    public class CoWriter
    {
        private readonly int Left;
        private readonly int Top;
        private readonly int Width;
        private readonly int Height;
        private readonly int Right;
        private readonly int Bottom;

        private readonly LineEditor line;

        public static void Setup(int left, int top, int width, int height)
        {
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);
            GetConsoleMode(handle, out var mode);
            mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            SetConsoleMode(handle, mode);

            G.C = new CoWriter(left, top, width, height);
            G.C.SetCursorPosition(0,0);
        }

        private CoWriter(int left, int top, int width, int height)
        {
            this.Left = left;
            this.Top = top;
            this.Width = width;
            this.Height = height;
            this.Right = this.Left + this.Width;
            this.Bottom = this.Top + this.Height;

            this.line = new LineEditor("coty") { HeuristicsMode = "coty" };
        }

        public bool IsInputRedirected => Console.IsInputRedirected;

        public int CursorLeft
        {
            get => Console.CursorLeft - this.Left;
            set => Console.CursorLeft = value + this.Left;
        }

        public int WindowWidth => this.Width;
        public int WindowHeight => this.Height;


        public int CursorTop
        {
            get => Console.CursorTop - this.Top;
            set => Console.CursorTop = value + this.Top;
        }

        public int BufferHeight => this.Height;

        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        public void Write(string s)
        {
            s = string.Join("", s.Where(c => !(char.IsControl(c) || char.IsLowSurrogate(c) || char.IsHighSurrogate(c))));

            var rest = this.Right - Console.CursorLeft;

            while (s.Length > rest)
            {
                var part = s.Substring(0, rest);
                s = s.Substring(rest);
                Console.Write(part);
                if (Console.CursorTop + 1 >= this.Bottom)
                {
                    ScrollUp();
                    Console.SetCursorPosition(this.Left, Console.CursorTop);
                }
                else
                {
                    Console.SetCursorPosition(this.Left, Console.CursorTop + 1);
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
            var clear = new string(' ', this.Width);
            for (var row = this.Top; row < this.Bottom; ++row)
            {
                Console.SetCursorPosition(this.Left, row);
                Console.Write(clear);
            }
            Console.SetCursorPosition(this.Left, this.Top);
        }

        private void NextLine()
        {
            if (Console.CursorTop + 1 >= this.Bottom)
            {
                ScrollUp();
                Console.SetCursorPosition(this.Left, Console.CursorTop);
            }
            else
            {
                Console.SetCursorPosition(this.Left, Console.CursorTop + 1);
            }
        }

        private void ScrollUp()
        {
            Console.MoveBufferArea(this.Left, this.Top + 1, this.Width, this.Height - 1, this.Left, this.Top);
        }

        public string GetLine(string prompt)
        {
            string s;
            if ((s = this.line.Edit(prompt, "")) != null)
            {
                return s;
            }

            return string.Empty;
        }

        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const int STD_OUTPUT_HANDLE = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);    
        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);    
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);


        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(this.Left + left, this.Top + top);
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return Console.ReadKey(intercept);
        }

        public event ConsoleCancelEventHandler CancelKeyPress
        {
            add => Console.CancelKeyPress += value;
            remove => Console.CancelKeyPress -= value;
        }

        public Stream OpenStandardOutput()
        {
            return Console.OpenStandardOutput();
        }
    }
}
