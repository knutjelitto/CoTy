using System;
using System.IO;

namespace CoTy.Support
{
    public interface IWindow
    {
        void Write(string s);
        void WriteLine(string s);
        void WriteLine();
        void Clear();
        int CursorLeft { get; set; }
        int CursorTop { get; set; }
        void SetCursorPosition(int left, int top);
        Stream OpenStandardOutput();
        int WindowWidth { get; }
        int WindowHeight { get; }
        int BufferWidth { get; }
        int BufferHeight { get; }
        ConsoleColor ForegroundColor { get; set; }
        ConsoleColor BackgroundColor { get; set; }

        string GetLine(string prompt);
        ConsoleKeyInfo ReadKey(bool intercept);

        event ConsoleCancelEventHandler CancelKeyPress;
    }
}
