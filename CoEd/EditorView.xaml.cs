using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Input;
using ReactiveUI;

namespace CoEd
{
    public class EditorViewBase : ReactiveUserControl<EditorViewModel> { }

    public partial class EditorView
    {
        static EditorView()
        {
            ClipToBoundsProperty.OverrideMetadata(typeof(EditorView), new PropertyMetadata(true));
        }

        public EditorView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.Text, v => v.text.ViewModel).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Scroll.Vertical.Value, v => v.verticalBar.Value).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.Scroll.Horizontal.Value, v => v.horizontalBar.Value).DisposeWith(d);

                this.Events().KeyDown.Subscribe(args =>
                {
                    Console.WriteLine($@"KEY:{args.Key}/{args.SystemKey}/{args.KeyStates}/{args.DeadCharProcessedKey}/{args.ImeProcessedKey}/{args.IsDown}/{args.IsRepeat}/{args.IsToggled}/{args.IsUp}");

                }).DisposeWith(d);

                this.Events().TextInput.Subscribe(args => { Console.WriteLine($@"text:{args.Text}"); }).DisposeWith(d);

                this.Events().GotFocus.Subscribe(args => Console.WriteLine(@"GotFocus"));
                this.Events().GotKeyboardFocus.Subscribe(args => Console.WriteLine(@"GotKeyboardFocus"));
                this.Events().LostFocus.Subscribe(args => Console.WriteLine(@"LostFocus"));
                this.Events().LostKeyboardFocus.Subscribe(args => Console.WriteLine(@"LostKeyboardFocus"));

                Keyboard.Focus(this);
            });
        }
    }
}
