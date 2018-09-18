namespace CoTy.Objects
{
    public class Binding : Cobject
    {
        public Binding(Context scope, object value, bool isSealed, bool isOpaque)
        {
            Scope = scope;
            Value = value;
            IsSealed = isSealed;
            IsOpaque = isOpaque;
        }

        public Context Scope { get; }
        public object Value { get; set; }
        public bool IsSealed { get; set; }
        public bool IsOpaque { get; }
    }
}
