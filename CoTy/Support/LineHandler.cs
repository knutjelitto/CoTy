using System;

namespace CoTy.Support
{
    public class LineHandler
    {
        public readonly ConsoleKeyInfo KeyInfo;
        public readonly Action KeyHandler;

        public LineHandler(ConsoleKey key, Action h)
        {
            this.KeyInfo = new ConsoleKeyInfo((char)0, key, false, false, false);
            this.KeyHandler = h;
        }

        private LineHandler(char c, Action h)
        {
            this.KeyHandler = h;
            // Use the "Zoom" as a flag that we only have a character.
            this.KeyInfo = new ConsoleKeyInfo(c, ConsoleKey.Zoom, false, false, false);
        }

        private LineHandler(ConsoleKeyInfo keyInfo, Action h)
        {
            this.KeyInfo = keyInfo;
            this.KeyHandler = h;
        }

        public static LineHandler Control(char c, Action h)
        {
            return new LineHandler((char)(c - 'A' + 1), h);
        }

        public static LineHandler Alt(char c, ConsoleKey k, Action h)
        {
            var cki = new ConsoleKeyInfo(c, k, false, true, false);
            return new LineHandler(cki, h);
        }
    }
}
