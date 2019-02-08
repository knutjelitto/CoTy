using System.Diagnostics;

namespace Pliant.Grammars
{
    public class FullyQualifiedName
    {
        public string Name { get; }
        public string Namespace { get; }
        public string FullName { get; }

        private readonly int _hashCode;

        public FullyQualifiedName(string @namespace, string name)
        {
            Namespace = @namespace;
            Name = name;
            FullName = !string.IsNullOrWhiteSpace(@namespace)
                ? $"{@namespace}.{name}"
                : $"{name}";
            this._hashCode = FullName.GetHashCode();
        }

        public override string ToString()
        {
            return FullName;
        }

        public static bool operator ==(FullyQualifiedName fullyQualifiedName, string value)
        {
            return fullyQualifiedName.FullName.Equals(value);
        }

        public static bool operator !=(FullyQualifiedName fullyQualifiedName, string value)
        {
            return !fullyQualifiedName.FullName.Equals(value);
        }

        public override bool Equals(object obj)
        {
            return obj is FullyQualifiedName other && FullName == other.FullName;
        }

        public override int GetHashCode()
        {
            return this._hashCode;
        }
    }
}