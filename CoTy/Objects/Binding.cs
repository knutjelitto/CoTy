namespace CoTy.Objects
{
    public class Binding : Cobject
    {
        public Binding(IBinder binder, Cobject value, Bool isSealed, Bool isOpaque)
        {
            Binder = binder;
            Value = value;
            IsSealed = isSealed;
            IsOpaque = isOpaque;
        }

        public IBinder Binder { get; }
        public Cobject Value { get; set; }
        public Bool IsSealed { get; set; }
        public Bool IsOpaque { get; set; }
    }
}
