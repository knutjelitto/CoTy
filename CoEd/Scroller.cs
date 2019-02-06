using System;
using ReactiveUI;

namespace CoEd
{
    public class Scroller : ReactiveObject
    {
        public Scroller()
        {
            Horizontal = new Pane(this, () => Vertical);
            Vertical = new Pane(this, () => Horizontal);
        }

        public Pane Horizontal { get; }
        public Pane Vertical { get; }

        public class Pane : ReactiveObject
        {
            private readonly Scroller scroller;
            private readonly Func<Pane> other;
            private double value;
            private double extend;
            private double view;

            public Pane(Scroller scroller, Func<Pane> other)
            {
                this.scroller = scroller;
                this.other = other;

                this.value = 0;
                this.extend = 100;

                this.WhenAnyValue(pane => pane.Value).Subscribe(v => Console.WriteLine($@"value:{v}"));
            }

            private Pane Other => this.other();

            public double Value
            {
                get => this.value;
                set => this.RaiseAndSetIfChanged(ref this.value, value);
            }

            public double Extend
            {
                get => this.extend;
                set => this.RaiseAndSetIfChanged(ref this.extend, value);
            }

            public double View
            {
                get => this.view;
                set => this.RaiseAndSetIfChanged(ref this.view, value);
            }
        }
    }
}
