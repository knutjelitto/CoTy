using System;
using System.Reactive.Disposables;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace CoEd
{
    public class EditorViewModel : ReactiveObject, ISupportsActivation
    {
        public EditorViewModel()
        {
            Scroll = new Scroller();
            Text = new TextViewModel();

            this.WhenActivated((CompositeDisposable disposableRegistration) =>
            {
            });
        }

        [Reactive] public TextViewModel Text { get; set; }

        public Scroller Scroll { get; set; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
