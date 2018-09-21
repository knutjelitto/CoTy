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
using System.IO;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;

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
    ///     search over the history, completion (both textual or visual) and various 
    ///     Emacs-like commands.
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
    /// LineEditor le = new LineEditor ("myshell") { HeuristicsMode = "csharp" };
    /// le.AutoCompleteEvent += delegate (string line, int point){
    ///     string prefix = "";
    ///     var completions = new string [] { 
    ///         "One", "Two", "Three", "Four", "Five", 
    ///          "Six", "Seven", "Eight", "Nine", "Ten" 
    ///     };
    ///     return new Mono.Terminal.LineEditor.Completion(prefix, completions);
    /// };
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

        /// <summary>
        /// Completion results returned by the completion handler.
        /// </summary>
        /// <remarks>
        /// You create an instance of this class to return the completion
        /// results for the text at the specific position.   The prefix parameter
        /// indicates the common prefix in the results, and the results contain the
        /// results without the prefix.   For example, when completing "ToString" and "ToDate"
        /// prefix would be "To" and the completions would be "String" and "Date".
        /// </remarks>
        public class Completion
        {
            /// <summary>
            /// Array of results, with the stem removed.
            /// </summary>
            public readonly string[] Result;

            /// <summary>
            /// Shared prefix for the completion results.
            /// </summary>
            public readonly string Prefix;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:Mono.Terminal.LineEditor.Completion"/> class.
            /// </summary>
            /// <param name="prefix">Common prefix for all results, an be null.</param>
            /// <param name="result">Array of possible completions.</param>
            public Completion(string prefix, string[] result)
            {
                this.Prefix = prefix;
                this.Result = result;
            }
        }

        /// <summary>
        /// Method signature for auto completion handlers.
        /// </summary>
        /// <remarks>
        /// The completion handler receives the text as it is being edited as
        /// well as the position of the cursor in that line.   The method
        /// must return an instance of Completion with the possible completions.
        /// </remarks>
        public delegate Completion AutoCompleteHandler(string text, int pos);

        /// <summary>
        /// The heuristics mode used by code completion.
        /// </summary>
        /// <remarks>
        ///    <para>
        ///      This controls the heuristics style used to show the code
        ///      completion popup as well as when to accept an entry.
        ///    </para>
        ///    <para>
        ///      The default value is null which requires the user to explicitly
        ///      use the TAB key to trigger a completion.    
        ///    </para>
        ///    <para>
        ///      Another possible value is "csharp" which will trigger auto-completion when a 
        ///      "." is entered.
        ///    </para>
        /// </remarks>
        public string HeuristicsMode;

        //static StreamWriter log;

        // The text being edited.
        private StringBuilder text;

        // The text as it is rendered (replaces (char)1 with ^A on display for example).
        private readonly StringBuilder rendered_text;

        // The prompt specified, and the prompt shown to the user.
        private string shown_prompt;

        // The current cursor position, indexes into "text", for an index
        // into rendered_text, use TextToRenderPos
        private int cursor;

        // The row where we started displaying data.
        private int home_row;

        // The maximum length that has been displayed on the screen
        private int max_rendered;

        // If we are done editing, this breaks the interactive loop
        private bool done;

        // The thread where the Editing started taking place
        private Thread edit_thread;

        // Our object that tracks history
        private readonly History history;

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
        private KeyHandler last_handler;

        // If we have a popup completion, this is not null and holds the state.
        private CompletionState current_completion;

        // If this is set, it contains an escape sequence to reset the Unix colors to the ones that were used on startup
        private static byte[] unix_reset_colors;

        // This contains a raw stream pointing to stdout, used to bypass the TermInfoDriver
        private static Stream unix_raw_output;

        private delegate void KeyHandler();

        private struct Handler
        {
            public readonly ConsoleKeyInfo CKI;
            public readonly KeyHandler KeyHandler;
            public readonly bool ResetCompletion;

            public Handler(ConsoleKey key, KeyHandler h, bool resetCompletion = true)
            {
                this.CKI = new ConsoleKeyInfo((char)0, key, false, false, false);
                this.KeyHandler = h;
                this.ResetCompletion = resetCompletion;
            }

            private Handler(char c, KeyHandler h, bool resetCompletion = true)
            {
                this.KeyHandler = h;
                // Use the "Zoom" as a flag that we only have a character.
                this.CKI = new ConsoleKeyInfo(c, ConsoleKey.Zoom, false, false, false);
                this.ResetCompletion = resetCompletion;
            }

            private Handler(ConsoleKeyInfo cki, KeyHandler h, bool resetCompletion = true)
            {
                this.CKI = cki;
                this.KeyHandler = h;
                this.ResetCompletion = resetCompletion;
            }

            public static Handler Control(char c, KeyHandler h, bool resetCompletion = true)
            {
                return new Handler((char)(c - 'A' + 1), h, resetCompletion);
            }

            public static Handler Alt(char c, ConsoleKey k, KeyHandler h)
            {
                var cki = new ConsoleKeyInfo(c, k, false, true, false);
                return new Handler(cki, h);
            }
        }

        /// <summary>
        ///   Invoked when the user requests auto-completion using the tab character
        /// </summary>
        /// <remarks>
        ///    The result is null for no values found, an array with a single
        ///    string, in that case the string should be the text to be inserted
        ///    for example if the word at pos is "T", the result for a completion
        ///    of "ToString" should be "oString", not "ToString".
        ///
        ///    When there are multiple results, the result should be the full
        ///    text
        /// </remarks>
        public AutoCompleteHandler AutoCompleteEvent;

        private static Handler[] handlers;

        private readonly bool isWindows;

        /// <summary>
        /// Initializes a new instance of the LineEditor, using the specified name for 
        /// retrieving and storing the history.   The history will default to 10 entries.
        /// </summary>
        /// <param name="name">Prefix for storing the editing history.</param>
        public LineEditor(string name) : this(name, 10) { }

        /// <summary>
        /// Initializes a new instance of the LineEditor, using the specified name for 
        /// retrieving and storing the history.   
        /// </summary>
        /// <param name="name">Prefix for storing the editing history.</param>
        /// <param name="histsize">Number of entries to store in the history file.</param>
        protected LineEditor(string name, int histsize)
        {
            handlers = new[] {
                new Handler (ConsoleKey.Home,       CmdHome),
                new Handler (ConsoleKey.End,        CmdEnd),
                new Handler (ConsoleKey.LeftArrow,  CmdLeft),
                new Handler (ConsoleKey.RightArrow, CmdRight),
                new Handler (ConsoleKey.UpArrow,    CmdUp, resetCompletion: false),
                new Handler (ConsoleKey.DownArrow,  CmdDown, resetCompletion: false),
                new Handler (ConsoleKey.Enter,      CmdDone, resetCompletion: false),
                new Handler (ConsoleKey.Backspace,  CmdBackspace, resetCompletion: false),
                new Handler (ConsoleKey.Delete,     CmdDeleteChar),
                new Handler (ConsoleKey.Tab,        CmdTabOrComplete, resetCompletion: false),
				
				// Emacs keys
				Handler.Control ('A', CmdHome),
                Handler.Control ('E', CmdEnd),
                Handler.Control ('B', CmdLeft),
                Handler.Control ('F', CmdRight),
                Handler.Control ('P', CmdUp, resetCompletion: false),
                Handler.Control ('N', CmdDown, resetCompletion: false),
                Handler.Control ('K', CmdKillToEOF),
                Handler.Control ('Y', CmdYank),
                Handler.Control ('D', CmdDeleteChar),
                Handler.Control ('L', CmdRefresh),
                Handler.Control ('R', CmdReverseSearch),
                Handler.Control ('G', delegate {} ),
                Handler.Alt ('B', ConsoleKey.B, CmdBackwardWord),
                Handler.Alt ('F', ConsoleKey.F, CmdForwardWord),

                Handler.Alt ('D', ConsoleKey.D, CmdDeleteWord),
                Handler.Alt ((char) 8, ConsoleKey.Backspace, CmdDeleteBackword),
				
				// DEBUG
				//Handler.Control ('T', CmdDebug),

				// quote
				Handler.Control ('Q', delegate { HandleChar (G.C.ReadKey(true).KeyChar); })
            };

            this.rendered_text = new StringBuilder();
            this.text = new StringBuilder();

            this.history = new History(name, histsize);

            this.isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            GetUnixConsoleReset();
            //if (File.Exists ("log"))File.Delete ("log");
            //log = File.CreateText ("log"); 
        }

        // On Unix, there is a "default" color which is not represented by any colors in
        // ConsoleColor and it is not possible to set is by setting the ForegroundColor or
        // BackgroundColor properties, so we have to use the terminfo driver in Mono to
        // fetch these values

        private void GetUnixConsoleReset()
        {
            //
            // On Unix, we want to be able to reset the color for the pop-up completion
            //
            if (this.isWindows)
            {
                return;
            }

            // Sole purpose of this call is to initialize the Terminfo driver
            var unused = G.C.CursorLeft;

            try
            {
                var terminfo_driver = Type.GetType("System.ConsoleDriver")?.GetField("driver", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null);
                if (terminfo_driver == null)
                {
                    return;
                }

                if (terminfo_driver.GetType()?.GetField("origPair", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(terminfo_driver) is string unix_reset_colors_str)
                {
                    unix_reset_colors = Encoding.UTF8.GetBytes(unix_reset_colors_str);
                }

                unix_raw_output = G.C.OpenStandardOutput();
            }
            catch (Exception e)
            {
                G.C.WriteLine("Error: " + e);
            }
        }

#if false
        private void CmdDebug()
        {
            this.history.Dump();
            G.C.WriteLine();
            Render();
        }
#endif

        private void Render()
        {
            G.C.Write(this.shown_prompt);
            G.C.Write(this.rendered_text.ToString());

            var max = Math.Max(this.rendered_text.Length + this.shown_prompt.Length, this.max_rendered);

            for (var i = this.rendered_text.Length + this.shown_prompt.Length; i < this.max_rendered; i++)
            {
                G.C.Write(" ");
            }
            this.max_rendered = this.shown_prompt.Length + this.rendered_text.Length;

            // Write one more to ensure that we always wrap around properly if we are at the
            // end of a line.
            G.C.Write(" ");

            UpdateHomeRow(max);
        }

        private void UpdateHomeRow(int screenpos)
        {
            var lines = 1 + screenpos / G.C.WindowWidth;

            this.home_row = G.C.CursorTop - (lines - 1);
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
                G.C.Write(this.rendered_text[i].ToString());
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
                    G.C.Write(" ");
                }
            }
        }

        private void ComputeRendered()
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

        private int LineCount => (this.shown_prompt.Length + this.rendered_text.Length) / G.C.WindowWidth;

        private void ForceCursor(int newpos)
        {
            this.cursor = newpos;

            var actual_pos = this.shown_prompt.Length + TextToRenderPos(this.cursor);
            var row = this.home_row + actual_pos / G.C.WindowWidth;
            var col = actual_pos % G.C.WindowWidth;

            if (row >= G.C.BufferHeight)
            {
                row = G.C.BufferHeight - 1;
            }

            G.C.SetCursorPosition(col, row);

            //log.WriteLine ("Going to cursor={0} row={1} col={2} actual={3} prompt={4} ttr={5} old={6}", newpos, row, col, actual_pos, prompt.Length, TextToRenderPos (cursor), cursor);
            //log.Flush ();
        }

        private void UpdateCursor(int newpos)
        {
            if (this.cursor == newpos)
            {
                return;
            }

            ForceCursor(newpos);
        }

        private void InsertChar(char c)
        {
            var prev_lines = LineCount;
            this.text = this.text.Insert(this.cursor, c);
            ComputeRendered();
            if (prev_lines != LineCount)
            {

                G.C.SetCursorPosition(0, this.home_row);
                Render();
                ForceCursor(++this.cursor);
            }
            else
            {
                RenderFrom(this.cursor);
                ForceCursor(++this.cursor);
                UpdateHomeRow(TextToScreenPos(this.cursor));
            }
        }

        private static void SaveExcursion(Action code)
        {
            var saved_col = G.C.CursorLeft;
            var saved_row = G.C.CursorTop;
            var saved_fore = G.C.ForegroundColor;
            var saved_back = G.C.BackgroundColor;

            code();

            G.C.CursorLeft = saved_col;
            G.C.CursorTop = saved_row;
            if (unix_reset_colors != null)
            {
                unix_raw_output.Write(unix_reset_colors, 0, unix_reset_colors.Length);
            }
            else
            {
                G.C.ForegroundColor = saved_fore;
                G.C.BackgroundColor = saved_back;
            }
        }

        private class CompletionState
        {
            public string Prefix;
            public string[] Completions;
            private readonly int Col;
            private readonly int Row;
            private readonly int Width;
            private readonly int Height;
            private int selected_item, top_item;

            public CompletionState(int col, int row, int width, int height)
            {
                this.Col = col;
                this.Row = row;
                this.Width = width;
                this.Height = height;

                if (this.Col < 0)
                {
                    throw new ArgumentException("Cannot be less than zero" + this.Col, nameof(col));
                }

                if (this.Row < 0)
                {
                    throw new ArgumentException("Cannot be less than zero", nameof(row));
                }

                if (this.Width < 1)
                {
                    throw new ArgumentException("Cannot be less than one", nameof(width));
                }

                if (this.Height < 1)
                {
                    throw new ArgumentException("Cannot be less than one", nameof(height));
                }
            }

            private void DrawSelection()
            {
                for (var r = 0; r < this.Height; r++)
                {
                    var item_idx = this.top_item + r;
                    var selected = item_idx == this.selected_item;

                    G.C.ForegroundColor = selected ? ConsoleColor.Black : ConsoleColor.Gray;
                    G.C.BackgroundColor = selected ? ConsoleColor.Cyan : ConsoleColor.Blue;

                    var item = this.Prefix + this.Completions[item_idx];
                    if (item.Length > this.Width)
                    {
                        item = item.Substring(0, this.Width);
                    }

                    G.C.CursorLeft = this.Col;
                    G.C.CursorTop = this.Row + r;
                    G.C.Write(item);
                    for (var space = item.Length; space <= this.Width; space++)
                    {
                        G.C.Write(" ");
                    }
                }
            }

            public string Current => this.Completions[this.selected_item];

            public void Show()
            {
                SaveExcursion(DrawSelection);
            }

            public void SelectNext()
            {
                if (this.selected_item + 1 < this.Completions.Length)
                {
                    this.selected_item++;
                    if (this.selected_item - this.top_item >= this.Height)
                    {
                        this.top_item++;
                    }

                    SaveExcursion(DrawSelection);
                }
            }

            public void SelectPrevious()
            {
                if (this.selected_item > 0)
                {
                    this.selected_item--;
                    if (this.selected_item < this.top_item)
                    {
                        this.top_item = this.selected_item;
                    }

                    SaveExcursion(DrawSelection);
                }
            }

            private void Clear()
            {
                for (var r = 0; r < this.Height; r++)
                {
                    G.C.CursorLeft = this.Col;
                    G.C.CursorTop = this.Row + r;
                    for (var space = 0; space <= this.Width; space++)
                    {
                        G.C.Write(" ");
                    }
                }
            }

            public void Remove()
            {
                SaveExcursion(Clear);
            }
        }

        private void ShowCompletions(string prefix, string[] completions)
        {
            // Ensure we have space, determine window size
            var window_height = Math.Min(completions.Length, G.C.WindowHeight / 5);
            var target_line = G.C.WindowHeight - window_height - 1;
            if (!this.isWindows && G.C.CursorTop > target_line)
            {
                var delta = G.C.CursorTop - target_line;
                G.C.CursorLeft = 0;
                G.C.CursorTop = G.C.WindowHeight - 1;
                for (var i = 0; i < delta + 1; i++)
                {
                    for (var c = G.C.WindowWidth; c > 0; c--)
                    {
                        G.C.Write(" "); // To debug use ("{0}", i%10);
                    }
                }
                G.C.CursorTop = target_line;
                G.C.CursorLeft = 0;
                Render();
            }

            const int MaxWidth = 50;
            var window_width = 12;
            var plen = prefix.Length;
            foreach (var s in completions)
            {
                window_width = Math.Max(plen + s.Length, window_width);
            }

            window_width = Math.Min(window_width, MaxWidth);

            if (this.current_completion == null)
            {
                var left = G.C.CursorLeft - prefix.Length;

                if (left + window_width + 1 >= G.C.WindowWidth)
                {
                    left = G.C.WindowWidth - window_width - 1;
                }

                this.current_completion = new CompletionState(left, G.C.CursorTop + 1, window_width, window_height)
                {
                    Prefix = prefix,
                    Completions = completions,
                };
            }
            else
            {
                this.current_completion.Prefix = prefix;
                this.current_completion.Completions = completions;
            }

            this.current_completion.Show();
            G.C.CursorLeft = 0;
        }

        private void HideCompletions()
        {
            if (this.current_completion == null)
            {
                return;
            }

            this.current_completion.Remove();
            this.current_completion = null;
        }

        //
        // Triggers the completion engine, if insertBestMatch is true, then this will
        // insert the best match found, this behaves like the shell "tab" which will
        // complete as much as possible given the options.
        //
        private void Complete()
        {
            var completion = this.AutoCompleteEvent(this.text.ToString(), this.cursor);
            var completions = completion.Result;
            if (completions == null)
            {
                HideCompletions();
                return;
            }

            var ncompletions = completions.Length;
            if (ncompletions == 0)
            {
                HideCompletions();
                return;
            }

            if (completions.Length == 1)
            {
                InsertTextAtCursor(completions[0]);
                HideCompletions();
            }
            else
            {
                var last = -1;

                for (var p = 0; p < completions[0].Length; p++)
                {
                    var c = completions[0][p];


                    for (var i = 1; i < ncompletions; i++)
                    {
                        if (completions[i].Length < p)
                        {
                            goto mismatch;
                        }

                        if (completions[i][p] != c)
                        {
                            goto mismatch;
                        }
                    }
                    last = p;
                }
            mismatch:
                var prefix = completion.Prefix;
                if (last != -1)
                {
                    InsertTextAtCursor(completions[0].Substring(0, last + 1));

                    // Adjust the completions to skip the common prefix
                    prefix += completions[0].Substring(0, last + 1);
                    for (var i = 0; i < completions.Length; i++)
                    {
                        completions[i] = completions[i].Substring(last + 1);
                    }
                }
                ShowCompletions(prefix, completions);
                Render();
                ForceCursor(this.cursor);
            }
        }

        //
        // When the user has triggered a completion window, this will try to update
        // the contents of it.   The completion window is assumed to be hidden at this
        // point
        // 
        private void UpdateCompletionWindow()
        {
            if (this.current_completion != null)
            {
                throw new Exception("This method should only be called if the window has been hidden");
            }

            var completion = this.AutoCompleteEvent(this.text.ToString(), this.cursor);
            var completions = completion.Result;
            if (completions == null)
            {
                return;
            }

            var ncompletions = completions.Length;
            if (ncompletions == 0)
            {
                return;
            }

            ShowCompletions(completion.Prefix, completion.Result);
            Render();
            ForceCursor(this.cursor);
        }


        //
        // Commands
        //
        private void CmdDone()
        {
            if (this.current_completion != null)
            {
                InsertTextAtCursor(this.current_completion.Current);
                HideCompletions();
                return;
            }

            this.done = true;
        }

        private void CmdTabOrComplete()
        {
            var complete = false;

            if (this.AutoCompleteEvent != null)
            {
                if (TabAtStartCompletes)
                {
                    complete = true;
                }
                else
                {
                    for (var i = 0; i < this.cursor; i++)
                    {
                        if (!char.IsWhiteSpace(this.text[i]))
                        {
                            complete = true;
                            break;
                        }
                    }
                }

                if (complete)
                {
                    Complete();
                }
                else
                {
                    HandleChar('\t');
                }
            }
            else
            {
                HandleChar('t');
            }
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
            if (this.cursor == 0)
            {
                return;
            }

            UpdateCursor(this.cursor - 1);
        }

        private void CmdBackwardWord()
        {
            var p = WordBackward(this.cursor);
            if (p == -1)
            {
                return;
            }

            UpdateCursor(p);
        }

        private void CmdForwardWord()
        {
            var p = WordForward(this.cursor);
            if (p == -1)
            {
                return;
            }

            UpdateCursor(p);
        }

        private void CmdRight()
        {
            if (this.cursor == this.text.Length)
            {
                return;
            }

            UpdateCursor(this.cursor + 1);
        }

        private void RenderAfter(int p)
        {
            ForceCursor(p);
            RenderFrom(p);
            ForceCursor(this.cursor);
        }

        private void CmdBackspace()
        {
            if (this.cursor == 0)
            {
                return;
            }

            var completing = this.current_completion != null;
            HideCompletions();

            this.text.Remove(--this.cursor, 1);
            ComputeRendered();
            RenderAfter(this.cursor);
            if (completing)
            {
                UpdateCompletionWindow();
            }
        }

        private void CmdDeleteChar()
        {
            // If there is no input, this behaves like EOF
            if (this.text.Length == 0)
            {
                this.done = true;
                this.text = null;
                G.C.WriteLine();
                return;
            }

            if (this.cursor == this.text.Length)
            {
                return;
            }

            this.text.Remove(this.cursor, 1);
            ComputeRendered();
            RenderAfter(this.cursor);
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
            var pos = WordForward(this.cursor);

            if (pos == -1)
            {
                return;
            }

            var k = this.text.ToString(this.cursor, pos - this.cursor);

            if (this.last_handler == CmdDeleteWord)
            {
                this.kill_buffer = this.kill_buffer + k;
            }
            else
            {
                this.kill_buffer = k;
            }

            this.text.Remove(this.cursor, pos - this.cursor);
            ComputeRendered();
            RenderAfter(this.cursor);
        }

        private void CmdDeleteBackword()
        {
            var pos = WordBackward(this.cursor);
            if (pos == -1)
            {
                return;
            }

            var k = this.text.ToString(pos, this.cursor - pos);

            if (this.last_handler == CmdDeleteBackword)
            {
                this.kill_buffer = k + this.kill_buffer;
            }
            else
            {
                this.kill_buffer = k;
            }

            this.text.Remove(pos, this.cursor - pos);
            ComputeRendered();
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

        private void CmdUp()
        {
            if (this.current_completion == null)
            {
                CmdHistoryPrev();
            }
            else
            {
                this.current_completion.SelectPrevious();
            }
        }

        private void CmdDown()
        {
            if (this.current_completion == null)
            {
                CmdHistoryNext();
            }
            else
            {
                this.current_completion.SelectNext();
            }
        }

        private void CmdKillToEOF()
        {
            this.kill_buffer = this.text.ToString(this.cursor, this.text.Length - this.cursor);
            this.text.Length = this.cursor;
            ComputeRendered();
            RenderAfter(this.cursor);
        }

        private void CmdYank()
        {
            InsertTextAtCursor(this.kill_buffer);
        }

        private void InsertTextAtCursor(string str)
        {
            var prev_lines = LineCount;
            this.text.Insert(this.cursor, str);
            ComputeRendered();
            if (prev_lines != LineCount)
            {
                G.C.SetCursorPosition(0, this.home_row);
                Render();
                this.cursor += str.Length;
                ForceCursor(this.cursor);
            }
            else
            {
                RenderFrom(this.cursor);
                this.cursor += str.Length;
                ForceCursor(this.cursor);
                UpdateHomeRow(TextToScreenPos(this.cursor));
            }
        }

        private void SetSearchPrompt(string s)
        {
            SetPrompt("(reverse-i-search)`" + s + "': ");
        }

        private void ReverseSearch()
        {
            int p;

            if (this.cursor == this.text.Length)
            {
                // The cursor is at the end of the string

                p = this.text.ToString().LastIndexOf(this.search, StringComparison.Ordinal);
                if (p != -1)
                {
                    this.match_at = p;
                    this.cursor = p;
                    ForceCursor(this.cursor);
                    return;
                }
            }
            else
            {
                // The cursor is somewhere in the middle of the string
                var start = this.cursor == this.match_at ? this.cursor - 1 : this.cursor;
                if (start != -1)
                {
                    p = this.text.ToString().LastIndexOf(this.search, start, StringComparison.Ordinal);
                    if (p != -1)
                    {
                        this.match_at = p;
                        this.cursor = p;
                        ForceCursor(this.cursor);
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
            if (this.cursor < this.text.Length)
            {
                var r = this.text.ToString(this.cursor, this.text.Length - this.cursor);
                if (r.StartsWith(this.search))
                {
                    return;
                }
            }

            ReverseSearch();
        }

        private void CmdRefresh()
        {
            G.C.Clear();
            this.max_rendered = 0;
            Render();
            ForceCursor(this.cursor);
        }

        private void InterruptEdit(object sender, ConsoleCancelEventArgs a)
        {
            // Do not abort our program:
            a.Cancel = true;

            // Interrupt the editor
            this.edit_thread.Abort();
        }

        //
        // Implements heuristics to show the completion window based on the mode
        //
        private bool HeuristicAutoComplete(bool wasCompleting, char insertedChar)
        {
            if (this.HeuristicsMode == "csharp")
            {
                // csharp heuristics
                if (wasCompleting)
                {
                    if (insertedChar == ' ')
                    {
                        return false;
                    }
                    return true;
                }
                // If we were not completing, determine if we want to now
                if (insertedChar == '.')
                {
                    // Avoid completing for numbers "1.2" for example
                    if (this.cursor > 1 && char.IsDigit(this.text[this.cursor - 2]))
                    {
                        for (var p = this.cursor - 3; p >= 0; p--)
                        {
                            var c = this.text[p];
                            if (char.IsDigit(c))
                            {
                                continue;
                            }

                            if (c == '_')
                            {
                                return true;
                            }

                            if (char.IsLetter(c) || char.IsPunctuation(c) || char.IsSymbol(c) || char.IsControl(c))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        private void HandleChar(char c)
        {
            if (this.searching != 0)
            {
                SearchAppend(c);
            }
            else
            {
                var completing = this.current_completion != null;
                HideCompletions();

                InsertChar(c);
                if (HeuristicAutoComplete(completing, c))
                {
                    UpdateCompletionWindow();
                }
            }
        }

        private void EditLoop()
        {
            while (!this.done)
            {
                ConsoleModifiers mod;

                var cki = G.C.ReadKey(true);
                if (cki.Key == ConsoleKey.Escape)
                {
                    if (this.current_completion != null)
                    {
                        HideCompletions();
                        continue;
                    }
                    else
                    {
                        cki = G.C.ReadKey(true);

                        mod = ConsoleModifiers.Alt;
                    }
                }
                else
                {
                    mod = cki.Modifiers;
                }

                var handled = false;

                foreach (var handler in handlers)
                {
                    var t = handler.CKI;

                    if (t.Key == cki.Key && t.Modifiers == mod)
                    {
                        handled = true;
                        if (handler.ResetCompletion)
                        {
                            HideCompletions();
                        }

                        handler.KeyHandler();
                        this.last_handler = handler.KeyHandler;
                        break;
                    }
                    else if (t.KeyChar == cki.KeyChar && t.Key == ConsoleKey.Zoom)
                    {
                        handled = true;
                        if (handler.ResetCompletion)
                        {
                            HideCompletions();
                        }

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

                if (cki.KeyChar != (char)0)
                {
                    HandleChar(cki.KeyChar);
                }
            }
        }

        private void InitText(string initial)
        {
            this.text = new StringBuilder(initial);
            ComputeRendered();
            this.cursor = this.text.Length;
            Render();
            ForceCursor(this.cursor);
        }

        private void SetText(string newtext)
        {
            G.C.SetCursorPosition(0, this.home_row);
            InitText(newtext);
        }

        private void SetPrompt(string newprompt)
        {
            this.shown_prompt = newprompt;
            G.C.SetCursorPosition(0, this.home_row);
            Render();
            ForceCursor(this.cursor);
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
            G.C.CancelKeyPress += InterruptEdit;

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
                    G.C.WriteLine();
                    SetPrompt(prompt);
                    SetText("");
                }
            } while (!this.done);
            G.C.WriteLine();

            G.C.CancelKeyPress -= InterruptEdit;

            if (this.text == null)
            {
                this.history.Close();
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
            this.history?.Close();
        }

        /// <summary>
        /// Gets or sets a value indicating whether hitting the TAB key before any text exists triggers completion or inserts a "tab" character into the buffer.  This is useful to allow users to copy/paste code that might contain whitespace at the start and you want to preserve it.
        /// </summary>
        /// <value><c>true</c> if tab at start completes; otherwise, <c>false</c>.</value>
        public bool TabAtStartCompletes { get; set; }

        //
        // Emulates the bash-like behavior, where edits done to the
        // history are recorded
        //
        private class History
        {
            private readonly string[] history;
            private int head, tail;
            private int cursor, count;
            private readonly string histfile;

            public History(string app, int size)
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
                this.head = this.tail = this.cursor = 0;

                if (File.Exists(this.histfile))
                {
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
                }
            }

            public void Close()
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
                //G.C.WriteLine ("APPENDING {0} head={1} tail={2}", s, head, tail);
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
                //G.C.WriteLine ("DONE: head={1} tail={2}", s, head, tail);
            }

            //
            // Updates the current cursor location with the string,
            // to support editing of history items.   For the current
            // line to participate, an Append must be done before.
            //
            public void Update(string s)
            {
                this.history[this.cursor] = s;
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
                //G.C.WriteLine ("h={0} t={1} cursor={2}", head, tail, cursor);
                if (this.count == 0)
                {
                    return false;
                }

                var next = this.cursor - 1;
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

                var next = (this.cursor + 1) % this.history.Length;
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

                this.cursor--;
                if (this.cursor < 0)
                {
                    this.cursor = this.history.Length - 1;
                }

                return this.history[this.cursor];
            }

            public string Next()
            {
                if (!NextAvailable())
                {
                    return null;
                }

                this.cursor = (this.cursor + 1) % this.history.Length;
                return this.history[this.cursor];
            }

            public void CursorToEnd()
            {
                if (this.head == this.tail)
                {
                    return;
                }

                this.cursor = this.head;
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
                    var slot = this.cursor - i - 1;
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
                        this.cursor = slot;
                        return this.history[slot];
                    }
                }

                return null;
            }

        }
    }

#if DEMO
	class Demo
	{
		static void MainX ()
		{
		    var le = new LineEditor("foo")
		    {
		        HeuristicsMode = "csharp"
		    };
		    le.AutoCompleteEvent += (a, pos) =>
		    {
		        var prefix = "";
		        var completions = new[] {"One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten"};
		        return new LineEditor.Completion(prefix, completions);
		    };
			
			string s;
			
			while ((s = le.Edit ("shell> ", "")) != null)
			{
				G.C.WriteLine ($"----> [{s}]");
			}
		}
	}
#endif
}