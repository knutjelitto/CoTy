using System;

namespace CoTy.Support
{
    public interface IWindow : IView
    {
        void Write(string s);
        void WriteLine(string s);
        void WriteLine();
        void Clear();
        int CursorLeft { get; set; }
        int CursorTop { get; set; }
        void SetCursorPosition(int left, int top);
        string GetLine(string prompt);
        ConsoleKeyInfo ReadKey(bool intercept);

        event ConsoleCancelEventHandler CancelKeyPress;
    }
}
