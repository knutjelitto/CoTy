using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace CoTy.Support
{
    public class CoWriter : IWindow
    {
        private int Left { get; }
        private int Top { get; }
        public int Width { get; }
        public int Height { get; }
        private int Right => Left + Width;
        private int Bottom => Top + Height;

        private readonly LineEditor line;

        public static void Setup(int left, int top, int width, int height)
        {
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);
            GetConsoleMode(handle, out var mode);
            mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            SetConsoleMode(handle, mode);

            var leftOff = 10;
            var rightOff = 0;

            G.C = new CoWriter(left + leftOff, top, width - leftOff - rightOff, height);
            G.C.SetCursorPosition(0,0);
        }

        private CoWriter(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;

            this.line = new LineEditor(this, "coty");
        }

        int IWindow.CursorLeft
        {
            get => Console.CursorLeft - Left;
            set => Console.CursorLeft = value + Left;
        }

        int IWindow.CursorTop
        {
            get => Console.CursorTop - Top;
            set => Console.CursorTop = value + Top;
        }

        public void Write(string s)
        {
            s = string.Join("", s.Where(c => !(char.IsControl(c) || char.IsLowSurrogate(c) || char.IsHighSurrogate(c))));

            var rest = Right - Console.CursorLeft;

            while (s.Length > rest)
            {
                var part = s.Substring(0, rest);
                s = s.Substring(rest);
                Console.Write(part);
                if (Console.CursorTop + 1 >= Bottom)
                {
                    ScrollUp();
                    Console.SetCursorPosition(Left, Console.CursorTop);
                }
                else
                {
                    Console.SetCursorPosition(Left, Console.CursorTop + 1);
                }

                rest = Width;
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
            var clear = new string(' ', Width);
            for (var row = Top; row < Bottom; ++row)
            {
                Console.SetCursorPosition(Left, row);
                Console.Write(clear);
            }
            Console.SetCursorPosition(Left, Top);
        }

        private void NextLine()
        {
            if (Console.CursorTop + 1 >= Bottom)
            {
                ScrollUp();
                Console.SetCursorPosition(Left, Console.CursorTop);
            }
            else
            {
                Console.SetCursorPosition(Left, Console.CursorTop + 1);
            }
        }

        private void ScrollUp()
        {
            Console.MoveBufferArea(Left, Top + 1, Width, Height - 1, Left, Top);
        }

        public string GetLine(string prompt)
        {
            return this.line.Edit(prompt, "");
        }

        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;
        private const int STD_OUTPUT_HANDLE = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);    
        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);    
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);


        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(Left + left, Top + top);
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
    }
}
