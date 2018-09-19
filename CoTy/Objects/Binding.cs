namespace CoTy.Objects
{
    public class Binding : Cobject
    {
        public Binding(IBinder binder, object value, bool isSealed, bool isOpaque)
        {
            Binder = binder;
            Value = value;
            IsSealed = isSealed;
            IsOpaque = isOpaque;
        }

        public IBinder Binder { get; }
        public object Value { get; set; }
        public bool IsSealed { get; set; }
        public bool IsOpaque { get; set; }
    }
}
