using System;
using System.IO;

namespace CoTy.Support
{
    //
    // Emulates the bash-like behavior, where edits done to the
    // history are recorded
    //
    public class LineHistory
    {
        private readonly string[] history;
        private int head, tail;
        private int current, count;
        private readonly string histfile;

        public LineHistory(string app, int size)
        {
            if (size < 1)
            {
                throw new ArgumentException("size");
            }

            if (app != null)
            {
                var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                if (!Directory.Exists(dir))
                {
                    try
                    {
                        Directory.CreateDirectory(dir);
                    }
                    catch
                    {
                        app = null;
                    }
                }
                if (app != null)
                {
                    this.histfile = Path.Combine(dir, app) + ".history";
                }
            }

            this.history = new string[size];
            this.head = this.tail = this.current = 0;

            if (File.Exists(this.histfile))
            {
#if true
                foreach (var line in File.ReadLines(this.histfile))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        Append(line);
                    }
                }
#else
                using (var sr = File.OpenText(this.histfile))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line != "")
                        {
                            Append(line);
                        }
                    }
                }
#endif
            }
        }

        public void Save()
        {
            if (this.histfile == null)
            {
                return;
            }

            try
            {
                using (var sw = File.CreateText(this.histfile))
                {
                    var start = this.count == this.history.Length ? this.head : this.tail;
                    for (var i = start; i < start + this.count; i++)
                    {
                        var p = i % this.history.Length;
                        sw.WriteLine(this.history[p]);
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        //
        // Appends a value to the history
        //
        public void Append(string s)
        {
            this.history[this.head] = s;
            this.head = (this.head + 1) % this.history.Length;
            if (this.head == this.tail)
            {
                this.tail = this.tail + 1 % this.history.Length;
            }

            if (this.count != this.history.Length)
            {
                this.count++;
            }
        }

        //
        // Updates the current cursor location with the string,
        // to support editing of history items.   For the current
        // line to participate, an Append must be done before.
        //
        public void Update(string s)
        {
            this.history[this.current] = s;
        }

        public void RemoveLast()
        {
            this.head = this.head - 1;
            if (this.head < 0)
            {
                this.head = this.history.Length - 1;
            }
        }

        public void Accept(string s)
        {
            var t = this.head - 1;
            if (t < 0)
            {
                t = this.history.Length - 1;
            }

            this.history[t] = s;
        }

        public bool PreviousAvailable()
        {
            if (this.count == 0)
            {
                return false;
            }

            var next = this.current - 1;
            if (next < 0)
            {
                next = this.count - 1;
            }

            if (next == this.head)
            {
                return false;
            }

            return true;
        }

        public bool NextAvailable()
        {
            if (this.count == 0)
            {
                return false;
            }

            var next = (this.current + 1) % this.history.Length;
            if (next == this.head)
            {
                return false;
            }

            return true;
        }


        //
        // Returns: a string with the previous line contents, or
        // nul if there is no data in the history to move to.
        //
        public string Previous()
        {
            if (!PreviousAvailable())
            {
                return null;
            }

            this.current--;
            if (this.current < 0)
            {
                this.current = this.history.Length - 1;
            }

            return this.history[this.current];
        }

        public string Next()
        {
            if (!NextAvailable())
            {
                return null;
            }

            this.current = (this.current + 1) % this.history.Length;
            return this.history[this.current];
        }

        public void CursorToEnd()
        {
            if (this.head == this.tail)
            {
                return;
            }

            this.current = this.head;
        }

#if false
            public void Dump()
            {
                G.C.WriteLine($"Head={this.head} Tail={this.tail} Cursor={this.cursor} count={this.count}");
                for (var i = 0; i < this.history.Length; i++)
                {
                    G.C.WriteLine($" {(i == this.cursor ? "==>" : "   ")} {i}: {this.history[i]}");
                }
                //log.Flush ();
            }
#endif

        public string SearchBackward(string term)
        {
            for (var i = 0; i < this.count; i++)
            {
                var slot = this.current - i - 1;
                if (slot < 0)
                {
                    slot = this.history.Length + slot;
                }

                if (slot >= this.history.Length)
                {
                    slot = 0;
                }

                if (this.history[slot] != null && this.history[slot].IndexOf(term, StringComparison.Ordinal) != -1)
                {
                    this.current = slot;
                    return this.history[slot];
                }
            }

            return null;
        }

    }
}
