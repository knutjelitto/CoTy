//
// getline.cs: A command line editor
//
// Authors:
//   Miguel de Icaza (miguel@microsoft.com)
//
// Copyright 2008 Novell, Inc.
// Copyright 2016 Xamarin Inc
// Copyright 2017 Microsoft
//
// Completion wanted:
//
//   * Enable bash-like completion window the window as an option for non-GUI people?
//
//   * Continue completing when Backspace is used?
//
//   * Should we keep the auto-complete on "."?
//
//   * Completion produces an error if the value is not resolvable, we should hide those errors
//
// Dual-licensed under the terms of the MIT X11 license or the
// Apache License 2.0
//
// USE -define:DEMO to build this as a standalone file and test it
//
// TODO:
//    Enter an error (a = 1);  Notice how the prompt is in the wrong line
//		This is caused by Stderr not being tracked by System.Console.
//    Completion support
//    Why is Thread.Interrupt not working?   Currently I resort to Abort which is too much.
//
// Limitations in System.Console:
//    Console needs SIGWINCH support of some sort
//    Console needs a way of updating its position after things have been written
//    behind its back (P/Invoke puts for example).
//    System.Console needs to get the DELETE character, and report accordingly.
//
// Bug:
//   About 8 lines missing, type "Con<TAB>" and not enough lines are inserted at the bottom.
// 
//
using System;
using System.Text;
using System.Threading;

namespace CoTy.Support
{

    /// <summary>
    /// Interactive line editor.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     LineEditor is an interative line editor for .NET applications that provides
    ///     editing capabilities for an input line with common editing capabilities and
    ///     navigation expected in modern application as well as history, incremental
    ///     search over the history and various Emacs-like commands.
    ///   </para>
    ///   <para>
    ///     When you create your line editor, you can pass the name of your application, 
    ///     which will be used to load and save the history of commands entered by the user
    ///     for this particular application.    
    ///   </para>
    ///   <para>
    ///     
    ///   </para>
    ///   <example>
    ///     The following example shows how you can instantiate a line editor that
    ///     can provide code completion for some words when the user presses TAB
    ///     and how the user can edit them. 
    ///     <code>
    /// LineEditor le = new LineEditor ("myshell");
    ///		
    /// string s;
    ///		
    /// while ((s = le.Edit("shell> ", "")) != null)
    ///    Console.WriteLine("You typed: [{0}]", s);			}
    ///     </code>
    ///   </example>
    ///   <para>
    ///      Users can use the cursor keys to navigate both the text on the current
    ///      line, or move back and forward through the history of commands that have
    ///      been entered.   
    ///   </para>
    ///   <para>
    ///     The interactive commands and keybindings are inspired by the GNU bash and
    ///     GNU readline capabilities and follow the same concepts found there.
    ///   </para>
    ///   <para>
    ///      Copy and pasting works like bash, deleted words or regions are added to 
    ///      the kill buffer.   Repeated invocations of the same deleting operation will
    ///      append to the kill buffer (for example, repeatedly deleting words) and to
    ///      paste the results you would use the Control-y command (yank).
    ///   </para>
    ///   <para>
    ///      The history search capability is triggered when you press 
    ///      Control-r to start a reverse interactive-search
    ///      and start typing the text you are looking for, the edited line will
    ///      be updated with matches.  Typing control-r again will go to the next
    ///      match in history and so on.
    ///   </para>
    ///   <list type="table"> 
    ///     <listheader>
    ///       <term>Shortcut</term>
    ///       <description>Action performed</description>
    ///     </listheader>
    ///     <item>
    ///        <term>Left cursor, Control-b</term>
    ///        <description>
    ///          Moves the editing point left.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Right cursor, Control-f</term>
    ///        <description>
    ///          Moves the editing point right.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Alt-b</term>
    ///        <description>
    ///          Moves one word back.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Alt-f</term>
    ///        <description>
    ///          Moves one word forward.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Up cursor, Control-p</term>
    ///        <description>
    ///          Selects the previous item in the editing history.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Down cursor, Control-n</term>
    ///        <description>
    ///          Selects the next item in the editing history.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Home key, Control-a</term>
    ///        <description>
    ///          Moves the cursor to the beginning of the line.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>End key, Control-e</term>
    ///        <description>
    ///          Moves the cursor to the end of the line.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Delete, Control-d</term>
    ///        <description>
    ///          Deletes the character in front of the cursor.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Backspace</term>
    ///        <description>
    ///          Deletes the character behind the cursor.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Tab</term>
    ///        <description>
    ///           Triggers the completion and invokes the AutoCompleteEvent which gets
    ///           both the line contents and the position where the cursor is.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Control-k</term>
    ///        <description>
    ///          Deletes the text until the end of the line and replaces the kill buffer
    ///          with the deleted text.   You can paste this text in a different place by
    ///          using Control-y.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Control-l refresh</term>
    ///        <description>
    ///           Clears the screen and forces a refresh of the line editor, useful when
    ///           a background process writes to the console and garbles the contents of
    ///           the screen.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Control-r</term>
    ///        <description>
    ///          Initiates the reverse search in history.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Alt-backspace</term>
    ///        <description>
    ///           Deletes the word behind the cursor and adds it to the kill ring.  You 
    ///           can paste the contents of the kill ring with Control-y.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Alt-d</term>
    ///        <description>
    ///           Deletes the word above the cursor and adds it to the kill ring.  You 
    ///           can paste the contents of the kill ring with Control-y.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Control-y</term>
    ///        <description>
    ///           Pastes the content of the kill ring into the current position.
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term>Control-q</term>
    ///        <description>
    ///          Quotes the next input character, to prevent the normal processing of
    ///          key handling to take place.
    ///        </description>
    ///     </item>
    ///   </list>
    /// </remarks>
    public class LineEditor
    {
        private readonly IWindow console;

        // The text being edited.
        private StringBuilder text;

        // The text as it is rendered (replaces (char)1 with ^A on display for example).
        private readonly StringBuilder rendered_text;

        // The prompt specified, and the prompt shown to the user.
        private string shown_prompt;

        // The current cursor position, indexes into "text", for an index
        // into rendered_text, use TextToRenderPos
        private int textIndex;

        // The row where we started displaying data.
        private int home_row;

        // The maximum length that has been displayed on the screen
        private int max_rendered;

        // If we are done editing, this breaks the interactive loop
        private bool done;

        // The thread where the Editing started taking place
        private Thread edit_thread;

        // Our object that tracks history
        private readonly LineHistory history;

        // The contents of the kill buffer (cut/paste in Emacs parlance)
        private string kill_buffer = "";

        // The string being searched for
        private string search;
        private string last_search;

        // whether we are searching (-1= reverse; 0 = no; 1 = forward)
        private int searching;

        // The position where we found the match.
        private int match_at;

        // Used to implement the Kill semantics (multiple Alt-Ds accumulate)
        private Action last_handler;

        private static LineHandler[] handlers;

        /// <summary>
        /// Initializes a new instance of the LineEditor, using the specified name for 
        /// retrieving and storing the history.   The history will default to 10 entries.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="name">Prefix for storing the editing history.</param>
        public LineEditor(IWindow console, string name) : this(console, name, 100) { }

        /// <summary>
        /// Initializes a new instance of the LineEditor, using the specified name for 
        /// retrieving and storing the history.   
        /// </summary>
        /// <param name="console"></param>
        /// <param name="name">Prefix for storing the editing history.</param>
        /// <param name="histsize">Number of entries to store in the history file.</param>
        private LineEditor(IWindow console, string name, int histsize)
        {
            this.console = console;
            handlers = new[] {
                new LineHandler (ConsoleKey.Home,       CmdHome),
                new LineHandler (ConsoleKey.End,        CmdEnd),
                new LineHandler (ConsoleKey.LeftArrow,  CmdLeft),
                new LineHandler (ConsoleKey.RightArrow, CmdRight),
                new LineHandler (ConsoleKey.UpArrow,    CmdHistoryPrev),
                new LineHandler (ConsoleKey.DownArrow,  CmdHistoryNext),
                new LineHandler (ConsoleKey.Enter,      CmdDone),
                new LineHandler (ConsoleKey.Backspace,  CmdBackspace),
                new LineHandler (ConsoleKey.Delete,     CmdDeleteChar),
                new LineHandler (ConsoleKey.Tab,        CmdTabOrComplete),
				
				// Emacs keys
				LineHandler.Control ('A', CmdHome),
                LineHandler.Control ('E', CmdEnd),
                LineHandler.Control ('B', CmdLeft),
                LineHandler.Control ('F', CmdRight),
                LineHandler.Control ('P', CmdHistoryPrev),
                LineHandler.Control ('N', CmdHistoryNext),
                LineHandler.Control ('K', CmdKillToEnd),
                LineHandler.Control ('Y', CmdYank),
                LineHandler.Control ('D', CmdDeleteChar),
                LineHandler.Control ('L', CmdRefresh),
                LineHandler.Control ('R', CmdReverseSearch),
                LineHandler.Control ('G', CmdCheck ),
                LineHandler.Alt ('B', ConsoleKey.B, CmdBackwardWord),
                LineHandler.Alt ('F', ConsoleKey.F, CmdForwardWord),

                LineHandler.Alt ('D', ConsoleKey.D, CmdDeleteWord),
                LineHandler.Alt ((char) 8, ConsoleKey.Backspace, CmdDeleteBackword),
				
				// quote
				LineHandler.Control ('Q', delegate { HandleChar (console.ReadKey(true).KeyChar); })
            };

            this.rendered_text = new StringBuilder();
            this.text = new StringBuilder();

            this.history = new LineHistory(name, histsize);
        }

        private void Render()
        {
            this.console.Write(this.shown_prompt);
            this.console.Write(this.rendered_text.ToString());

            var max = Math.Max(this.rendered_text.Length + this.shown_prompt.Length, this.max_rendered);

            for (var i = this.rendered_text.Length + this.shown_prompt.Length; i < this.max_rendered; i++)
            {
                this.console.Write(" ");
            }
            this.max_rendered = this.shown_prompt.Length + this.rendered_text.Length;

            // Write one more to ensure that we always wrap around properly if we are at the
            // end of a line.
            this.console.Write(" ");

            UpdateHomeRow(max);
        }

        private void UpdateHomeRow(int screenpos)
        {
            var lines = 1 + screenpos / this.console.Width;

            this.home_row = this.console.CursorTop - (lines - 1);
            if (this.home_row < 0)
            {
                this.home_row = 0;
            }
        }

        private void RenderFrom(int pos)
        {
            var rpos = TextToRenderPos(pos);
            int i;

            for (i = rpos; i < this.rendered_text.Length; i++)
            {
                this.console.Write(this.rendered_text[i].ToString());
            }

            if (this.shown_prompt.Length + this.rendered_text.Length > this.max_rendered)
            {
                this.max_rendered = this.shown_prompt.Length + this.rendered_text.Length;
            }
            else
            {
                var max_extra = this.max_rendered - this.shown_prompt.Length;
                for (; i < max_extra; i++)
                {
                    this.console.Write(" ");
                }
            }
        }

        private void MakeRendered()
        {
            this.rendered_text.Length = 0;

            for (var i = 0; i < this.text.Length; i++)
            {
                var c = (int) this.text[i];
                if (c < 26)
                {
                    if (c == '\t')
                    {
                        this.rendered_text.Append("    ");
                    }
                    else
                    {
                        this.rendered_text.Append('^');
                        this.rendered_text.Append((char)(c + 'A' - 1));
                    }
                }
                else
                {
                    this.rendered_text.Append((char)c);
                }
            }
        }

        private int TextToRenderPos(int pos)
        {
            var p = 0;

            for (var i = 0; i < pos; i++)
            {
                var c = (int) this.text[i];

                if (c < 26)
                {
                    if (c == 9)
                    {
                        p += 4;
                    }
                    else
                    {
                        p += 2;
                    }
                }
                else
                {
                    p++;
                }
            }

            return p;
        }

        private int TextToScreenPos(int pos)
        {
            return this.shown_prompt.Length + TextToRenderPos(pos);
        }

        private int LineCount => (this.shown_prompt.Length + this.rendered_text.Length) / this.console.Width;

        private void ForceCursor(int newpos)
        {
            this.textIndex = newpos;

            var actual_pos = this.shown_prompt.Length + TextToRenderPos(this.textIndex);
            var row = this.home_row + actual_pos / this.console.Width;
            var col = actual_pos % this.console.Width;

            if (row >= this.console.Height)
            {
                row = this.console.Height - 1;
            }

            this.console.SetCursorPosition(col, row);
        }

        private void UpdateCursor(int newpos)
        {
            if (this.textIndex == newpos)
            {
                return;
            }

            ForceCursor(newpos);
        }

        private void InsertChar(char c)
        {
            var prev_lines = LineCount;
            this.text.Insert(this.textIndex, c);
            MakeRendered();
            if (prev_lines != LineCount)
            {
                this.console.SetCursorPosition(0, this.home_row);
                Render();
                ForceCursor(++this.textIndex);
            }
            else
            {
                RenderFrom(this.textIndex);
                ForceCursor(++this.textIndex);
                UpdateHomeRow(TextToScreenPos(this.textIndex));
            }
        }

        //
        // Commands
        //
        private void CmdDone()
        {
            this.done = true;
        }

        private void CmdTabOrComplete()
        {
            HandleChar('\t');
        }

        private void CmdHome()
        {
            UpdateCursor(0);
        }

        private void CmdEnd()
        {
            UpdateCursor(this.text.Length);
        }

        private void CmdLeft()
        {
            if (this.textIndex == 0)
            {
                return;
            }

            UpdateCursor(this.textIndex - 1);
        }

        private void CmdBackwardWord()
        {
            var p = WordBackward(this.textIndex);
            if (p == -1)
            {
                return;
            }

            UpdateCursor(p);
        }

        private void CmdForwardWord()
        {
            var p = WordForward(this.textIndex);
            if (p == -1)
            {
                return;
            }

            UpdateCursor(p);
        }

        private void CmdRight()
        {
            if (this.textIndex == this.text.Length)
            {
                return;
            }

            UpdateCursor(this.textIndex + 1);
        }

        private void RenderAfter(int p)
        {
            ForceCursor(p);
            RenderFrom(p);
            ForceCursor(this.textIndex);
        }

        private void CmdBackspace()
        {
            if (this.textIndex == 0)
            {
                return;
            }

            this.text.Remove(--this.textIndex, 1);
            MakeRendered();
            RenderAfter(this.textIndex);
        }

        private void CmdDeleteChar()
        {
            // If there is no input, this behaves like EOF
            if (this.text.Length == 0)
            {
                this.done = true;
                this.text = null;
                //this.console.WriteLine();
                return;
            }

            if (this.textIndex == this.text.Length)
            {
                return;
            }

            this.text.Remove(this.textIndex, 1);
            MakeRendered();
            RenderAfter(this.textIndex);
        }

        private int WordForward(int p)
        {
            if (p >= this.text.Length)
            {
                return -1;
            }

            var i = p;
            if (char.IsPunctuation(this.text[p]) || char.IsSymbol(this.text[p]) || char.IsWhiteSpace(this.text[p]))
            {
                for (; i < this.text.Length; i++)
                {
                    if (char.IsLetterOrDigit(this.text[i]))
                    {
                        break;
                    }
                }
                for (; i < this.text.Length; i++)
                {
                    if (!char.IsLetterOrDigit(this.text[i]))
                    {
                        break;
                    }
                }
            }
            else
            {
                for (; i < this.text.Length; i++)
                {
                    if (!char.IsLetterOrDigit(this.text[i]))
                    {
                        break;
                    }
                }
            }
            if (i != p)
            {
                return i;
            }

            return -1;
        }

        private int WordBackward(int p)
        {
            if (p == 0)
            {
                return -1;
            }

            var i = p - 1;
            if (i == 0)
            {
                return 0;
            }

            if (char.IsPunctuation(this.text[i]) || char.IsSymbol(this.text[i]) || char.IsWhiteSpace(this.text[i]))
            {
                for (; i >= 0; i--)
                {
                    if (char.IsLetterOrDigit(this.text[i]))
                    {
                        break;
                    }
                }
                for (; i >= 0; i--)
                {
                    if (!char.IsLetterOrDigit(this.text[i]))
                    {
                        break;
                    }
                }
            }
            else
            {
                for (; i >= 0; i--)
                {
                    if (!char.IsLetterOrDigit(this.text[i]))
                    {
                        break;
                    }
                }
            }
            i++;

            if (i != p)
            {
                return i;
            }

            return -1;
        }

        private void CmdDeleteWord()
        {
            var pos = WordForward(this.textIndex);

            if (pos == -1)
            {
                return;
            }

            var k = this.text.ToString(this.textIndex, pos - this.textIndex);

            if (this.last_handler == CmdDeleteWord)
            {
                this.kill_buffer = this.kill_buffer + k;
            }
            else
            {
                this.kill_buffer = k;
            }

            this.text.Remove(this.textIndex, pos - this.textIndex);
            MakeRendered();
            RenderAfter(this.textIndex);
        }

        private void CmdDeleteBackword()
        {
            var pos = WordBackward(this.textIndex);
            if (pos == -1)
            {
                return;
            }

            var k = this.text.ToString(pos, this.textIndex - pos);

            if (this.last_handler == CmdDeleteBackword)
            {
                this.kill_buffer = k + this.kill_buffer;
            }
            else
            {
                this.kill_buffer = k;
            }

            this.text.Remove(pos, this.textIndex - pos);
            MakeRendered();
            RenderAfter(pos);
        }

        //
        // Adds the current line to the history if needed
        //
        private void HistoryUpdateLine()
        {
            this.history.Update(this.text.ToString());
        }

        private void CmdHistoryPrev()
        {
            if (!this.history.PreviousAvailable())
            {
                return;
            }

            HistoryUpdateLine();

            SetText(this.history.Previous());
        }

        private void CmdHistoryNext()
        {
            if (!this.history.NextAvailable())
            {
                return;
            }

            this.history.Update(this.text.ToString());
            SetText(this.history.Next());

        }

        private void CmdKillToEnd()
        {
            this.kill_buffer = this.text.ToString(this.textIndex, this.text.Length - this.textIndex);
            this.text.Length = this.textIndex;
            MakeRendered();
            RenderAfter(this.textIndex);
        }

        private void CmdYank()
        {
            InsertTextAtCursor(this.kill_buffer);
        }

        private void InsertTextAtCursor(string str)
        {
            var prev_lines = LineCount;
            this.text.Insert(this.textIndex, str);
            MakeRendered();
            if (prev_lines != LineCount)
            {
                this.console.SetCursorPosition(0, this.home_row);
                Render();
                this.textIndex += str.Length;
                ForceCursor(this.textIndex);
            }
            else
            {
                RenderFrom(this.textIndex);
                this.textIndex += str.Length;
                ForceCursor(this.textIndex);
                UpdateHomeRow(TextToScreenPos(this.textIndex));
            }
        }

        private void SetSearchPrompt(string s)
        {
            SetPrompt("(reverse-i-search)`" + s + "': ");
        }

        private void ReverseSearch()
        {
            int p;

            if (this.textIndex == this.text.Length)
            {
                // The cursor is at the end of the string

                p = this.text.ToString().LastIndexOf(this.search, StringComparison.Ordinal);
                if (p != -1)
                {
                    this.match_at = p;
                    this.textIndex = p;
                    ForceCursor(this.textIndex);
                    return;
                }
            }
            else
            {
                // The cursor is somewhere in the middle of the string
                var start = this.textIndex == this.match_at ? this.textIndex - 1 : this.textIndex;
                if (start != -1)
                {
                    p = this.text.ToString().LastIndexOf(this.search, start, StringComparison.Ordinal);
                    if (p != -1)
                    {
                        this.match_at = p;
                        this.textIndex = p;
                        ForceCursor(this.textIndex);
                        return;
                    }
                }
            }

            // Need to search backwards in history
            HistoryUpdateLine();
            var s = this.history.SearchBackward(this.search);
            if (s != null)
            {
                this.match_at = -1;
                SetText(s);
                ReverseSearch();
            }
        }

        private void CmdReverseSearch()
        {
            if (this.searching == 0)
            {
                this.match_at = -1;
                this.last_search = this.search;
                this.searching = -1;
                this.search = "";
                SetSearchPrompt("");
            }
            else
            {
                if (this.search == "")
                {
                    if (!string.IsNullOrEmpty(this.last_search))
                    {
                        this.search = this.last_search;
                        SetSearchPrompt(this.search);

                        ReverseSearch();
                    }
                    return;
                }
                ReverseSearch();
            }
        }

        private void SearchAppend(char c)
        {
            this.search = this.search + c;
            SetSearchPrompt(this.search);

            //
            // If the new typed data still matches the current text, stay here
            //
            if (this.textIndex < this.text.Length)
            {
                var r = this.text.ToString(this.textIndex, this.text.Length - this.textIndex);
                if (r.StartsWith(this.search))
                {
                    return;
                }
            }

            ReverseSearch();
        }

        private void CmdCheck()
        {
            this.console.SetCursorPosition(this.console.Width - 1, this.console.Height - 1);
            this.console.Write("X");
        }

        private void CmdRefresh()
        {
            this.console.Clear();
            this.max_rendered = 0;
            Render();
            ForceCursor(this.textIndex);
        }

        private void InterruptEdit(object sender, ConsoleCancelEventArgs a)
        {
            // Do not abort our program:
            a.Cancel = true;

            // Interrupt the editor
            this.edit_thread.Abort();
        }

        private void HandleChar(char c)
        {
            if (this.searching != 0)
            {
                SearchAppend(c);
            }
            else
            {
                InsertChar(c);
            }
        }

        private void EditLoop()
        {
            while (!this.done)
            {
                ConsoleModifiers mod;

                var keyInfo = this.console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    keyInfo = this.console.ReadKey(true);

                    mod = ConsoleModifiers.Alt;
                }
                else
                {
                    mod = keyInfo.Modifiers;
                }

                var handled = false;

                foreach (var handler in handlers)
                {
                    var t = handler.KeyInfo;

                    if (t.Key == keyInfo.Key && t.Modifiers == mod)
                    {
                        handled = true;

                        handler.KeyHandler();
                        this.last_handler = handler.KeyHandler;
                        break;
                    }
                    if (t.KeyChar == keyInfo.KeyChar && t.Key == ConsoleKey.Zoom)
                    {
                        handled = true;

                        handler.KeyHandler();
                        this.last_handler = handler.KeyHandler;
                        break;
                    }
                }
                if (handled)
                {
                    if (this.searching != 0)
                    {
                        if (this.last_handler != CmdReverseSearch)
                        {
                            this.searching = 0;
                            SetPrompt(string.Empty);
                        }
                    }
                    continue;
                }

                if (keyInfo.KeyChar != (char)0)
                {
                    HandleChar(keyInfo.KeyChar);
                }
            }
        }

        private void InitText(string initial)
        {
            this.text = new StringBuilder(initial);
            MakeRendered();
            this.textIndex = this.text.Length;
            Render();
            ForceCursor(this.textIndex);
        }

        private void SetText(string newtext)
        {
            this.console.SetCursorPosition(0, this.home_row);
            InitText(newtext);
        }

        private void SetPrompt(string newprompt)
        {
            this.shown_prompt = newprompt;
            this.console.SetCursorPosition(0, this.home_row);
            Render();
            ForceCursor(this.textIndex);
        }

        /// <summary>
        /// Edit a line, and provides both a prompt and the initial contents to edit
        /// </summary>
        /// <returns>The edit.</returns>
        /// <param name="prompt">Prompt shown to edit the line.</param>
        /// <param name="initial">Initial contents, can be null.</param>
        public string Edit(string prompt, string initial)
        {
            this.edit_thread = Thread.CurrentThread;
            this.searching = 0;
            this.console.CancelKeyPress += InterruptEdit;

            this.done = false;
            this.history.CursorToEnd();
            this.max_rendered = 0;

            this.shown_prompt = prompt;
            InitText(initial);
            this.history.Append(initial);

            do
            {
                try
                {
                    EditLoop();
                }
                catch (ThreadAbortException)
                {
                    this.searching = 0;
                    Thread.ResetAbort();
                    this.console.WriteLine();
                    SetPrompt(prompt);
                    SetText("");
                }
            } while (!this.done);

            this.console.WriteLine();

            this.console.CancelKeyPress -= InterruptEdit;

            if (this.text == null)
            {
                this.history.Save();
                return null;
            }

            var result = this.text.ToString();
            if (result != "")
            {
                this.history.Accept(result);
            }
            else
            {
                this.history.RemoveLast();
            }

            return result;
        }

        /// <summary>
        /// Triggers the history to be written at this point, usually not necessary, history is saved on each line edited.
        /// </summary>
        public void SaveHistory()
        {
            this.history?.Save();
        }

    }

#if DEMO
	class Demo
	{
	    private static void MainX ()
	    {
	        var le = new LineEditor("foo");
			
			string s;
			
			while ((s = le.Edit ("shell> ", "")) != null)
			{
				G.C.WriteLine ($"----> [{s}]");
			}
		}
	}
#endif
}