using System;

namespace CoTy.Support
{
    public class CoWriter
    {
        private readonly int left;
        private readonly int top;
        private readonly int width;
        private readonly int height;

        public CoWriter(int left, int top, int width, int height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
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
            var clear = new string('.', this.width);
            var col = this.left;
            for (var row = this.top; row < this.top + this.height; ++row)
            {
                Console.SetCursorPosition(col, row);
                Console.Write(clear);
            }
            Console.SetCursorPosition(this.left, this.top);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
