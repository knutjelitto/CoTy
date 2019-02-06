using System;
using System.Diagnostics;
using System.Windows.Input;
using ReactiveUI;

namespace CoWpf
{
    public class MainWindowBase : ReactiveWindow<MainViewModel> {}

    // MainWindow class derives off ReactiveWindow which implements the IViewFor<TViewModel>
    // interface using a WPF DependencyProperty. We need this to use WhenActivated extension
    // method that helps us handling View and ViewModel activation and deactivation.
    public partial class MainWindow : MainWindowBase
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainViewModel();

            // We create our bindings here. These are the code behind bindings which allow 
            // type safety. The bindings will only become active when the Window is being shown.
            // We register our subscription in our disposableRegistration, this will cause 
            // the binding subscription to become inactive when the Window is closed.
            // The disposableRegistration is a CompositeDisposable which is a container of 
            // other Disposables. We use the DisposeWith() extension method which simply adds 
            // the subscription disposable to the CompositeDisposable.
            this.WhenActivated(d => { this.OneWayBind(ViewModel, vm => vm.Editor, v => v.editor.ViewModel); });
        }
    }
}
