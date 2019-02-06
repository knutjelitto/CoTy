using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace CoEd
{
    public class TextViewModel : ReactiveObject, ISupportsActivation
    {
        public TextViewModel()
        {
            Caret = new Caret();
            Document = new Document();

            this.WhenActivated(d =>
            {
                Document.LoadFrom(@"../../../CoEd/Scroller.cs");

                this.WhenAnyValue(vm => vm.Source)
                    .Where(source => source != null)
                    .Subscribe(source =>
                    {
                        //--
                        Caret.Height = source.RunProperties.LineHeight;
                    }).DisposeWith(d);
            });

        }

        [Reactive] public Caret Caret { get; set; }

        [Reactive] public Document Document { get; set; }

        [ObservableAsProperty] public Source Source { get; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
