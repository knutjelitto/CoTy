// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global

using System;
using CoTy.Errors;

namespace CoTy.Objects.Impl
{
    public partial class CobjectImpl
    {
        public object Plus(object value1, object value2)
        {
            if (value1.GetType() != value2.GetType())
            {
                throw new DynaException(nameof(Plus), typeof(object), value1.GetType(), value2.GetType());
            }

            var zero = Dyn.Zero(value1);
            while (Dyn.Compare(value2, zero) > 0)
            {
                value1 = Dyn.Succ(value1);
                value2 = Dyn.Pred(value2);
            }

            return value1;
        }

        public object Minus(object value1, object value2)
        {
            if (value1.GetType() != value2.GetType())
            {
                throw new DynaException(nameof(Plus), typeof(object), value1.GetType(), value2.GetType());
            }

            var zero = Dyn.Zero(value1);
            while (Dyn.Compare(value2, zero) > 0)
            {
                value1 = Dyn.Pred(value1);
                value2 = Dyn.Pred(value2);
            }

            return value1;
        }

        public object Star(object value1, object value2)
        {
            if (value1.GetType() != value2.GetType())
            {
                throw new DynaException(nameof(Plus), typeof(object), value1.GetType(), value2.GetType());
            }

            var zero = Dyn.Zero(value1);
            var result = zero;
            while (Dyn.Compare(value2, zero) > 0)
            {
                result = Dyn.Plus(result, value1);
                value2 = Dyn.Pred(value2);
            }

            return result;
        }

        public object Slash(object value1, object value2)
        {
            if (value1.GetType() != value2.GetType())
            {
                throw new DynaException(nameof(Plus), typeof(object), value1.GetType(), value2.GetType());
            }

            var zero = Dyn.Zero(value1);
            var result = zero;
            while (Dyn.Compare(value1, value2) >= 0)
            {
                result = Dyn.Succ(result);
                value1 = Dyn.Minus(value1, value2);
            }

            return result;
        }
    }
}
