using System.Collections.Generic;
using CoEd;
using ReactiveUI;

namespace CoWpf
{
    public class MainViewModel : ReactiveObject
    {
        public MainViewModel()
        {
            Editor = new EditorViewModel();

            Editor.Text.Document = new Document();
        }

        public EditorViewModel Editor { get; }
    }
}
