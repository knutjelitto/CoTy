using System.IO;
using DynamicData;

namespace CoEd
{
    public class Document
    {
        public SourceList<string> Lines { get; }

        public Document()
        {
            Lines = new SourceList<string>();
        }

        public void LoadFrom(string fileName)
        {
            Lines.Edit(list =>
            {
                list.Clear();
                list.AddRange(File.ReadLines(fileName));
            });
        }
    }
}
