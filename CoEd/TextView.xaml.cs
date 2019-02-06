using System;
using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace CoEd
{
    public class TextViewBase : ReactiveUserControl<TextViewModel> { }

    public partial class TextView
    {
        static TextView()
        {
            ClipToBoundsProperty.OverrideMetadata(typeof(TextView), new PropertyMetadata(true));
            FontFamilyProperty.OverrideMetadata(typeof(TextView),
                                                new FrameworkPropertyMetadata(
                                                    new FontFamily("Consolas")
                                                    //new FontFamily("Courier New")
                                                    //new FontFamily("Lucida Console")
                                                    //new FontFamily("Lucida Sans Typewriter")
                                                ));
            FontSizeProperty.OverrideMetadata(typeof(TextView), new FrameworkPropertyMetadata(13 * (96.0 / 72.0)));
        }

        public TextView()
        {
            InitializeComponent();

            Formatter = TextFormatter.Create(TextFormattingMode.Display);

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Caret.Height, v => v.caret.Height).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Caret.Width, v => v.caret.Width).DisposeWith(d);

                this.WhenAnyValue(v => v.FontFamily, v => v.FontSize, (family, size) =>
                    {
                        var typeface = new Typeface(family,
                                                    FontStyles.Normal,
                                                    FontWeights.Normal,
                                                    FontStretches.Normal);
                        return new Source(typeface, size, Brushes.Black);
                    })
                    .ToPropertyEx(ViewModel, v => v.Source)
                    .DisposeWith(d);

                this.Events().SizeChanged
                    .Throttle(TimeSpan.FromMilliseconds(40), RxApp.MainThreadScheduler)
                    .Where(args => args.HeightChanged)
                    .Do(args => Console.WriteLine($@"size-changed: <{args.PreviousSize}->{args.NewSize}>"))
                    .Subscribe(args =>
                    {
                        //--
                        Render();
                        InvalidateVisual();
                    })
                    .DisposeWith(d);

                ViewModel.Document.Lines.Connect().Subscribe(changeSet =>
                {
                    //--
                    Console.WriteLine($@"changeSet: <{changeSet}>");
                    foreach (var change in changeSet)
                    {
                        Console.WriteLine($@"change: <{change}>");
                    }

                    Render();

                    InvalidateVisual(); // ¯\_(ツ)_/¯

                }).DisposeWith(d);

            });
        }

        public TextFormatter Formatter { get; }

        private DrawingVisual textVisual;

        private void Render()
        {
            this.textVisual = new DrawingVisual();

            using (var dc = this.textVisual.RenderOpen())
            {
                Render(dc);
            }
        }

        private void Render(DrawingContext dc)
        {
            if (ViewModel.Source == null)
            {
                return;
            }

            var source = ViewModel.Source;

            var y = 0.0;
            var width = ActualWidth;

            foreach (var line in ViewModel.Document.Lines.Items)
            {
#if false
                var ft = new FormattedText(
                    line,
                    CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                    FontSize,
                    Brushes.Black,
                    null, 
                    VisualTreeHelper.GetDpi(this).PixelsPerDip);

                dc.DrawText(ft, new Point(0, y));
                y += ft.Height;
#else
                source.Text = line;
                using (var tx = Formatter.FormatLine(source, 0, width, source.ParagraphProperties, null))
                {
                    tx.Draw(dc, new Point(0, y), InvertAxes.None);
                    y += tx.Height;
                }
#endif

                if (y > ActualHeight)
                {
                    break;
                }
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            dc.DrawRectangle(Brushes.AntiqueWhite, null, new Rect(new Point(0, 0), RenderSize));
            if (this.textVisual != null)
            {
                dc.DrawDrawing(this.textVisual.Drawing);
            }
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            Console.WriteLine(@"OnGotKeyboardFocus");
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);

            Console.WriteLine(@"OnLostKeyboardFocus");
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            Console.WriteLine(@"OnKeyDown");
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            Console.WriteLine(@"OnKeyUp");
        }
    }
}
