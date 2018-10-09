using CoTy.Support;

namespace CoTy.Definitions
{
    public class PlusConsole : Core
    {
        public PlusConsole() : base("console") { }

        public override void Define(Maker into)
        {
            into.Define("print", value => G.C.Write($"{value}"));
            into.Define("println", value => G.C.WriteLine($"{value}"));
            into.Define("nl", () => G.C.WriteLine());
            into.Define("cls", () => G.C.Clear());
        }
    }
}
