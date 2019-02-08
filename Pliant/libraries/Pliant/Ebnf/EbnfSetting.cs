using Pliant.Utilities;
using System;

namespace Pliant.Ebnf
{
    public class EbnfSetting : EbnfNode
    {
        private readonly int _hashCode;

        public EbnfSettingIdentifier SettingIdentifier { get; }
        public EbnfQualifiedIdentifier QualifiedIdentifier { get; }

        public EbnfSetting(EbnfSettingIdentifier settingIdentifier, EbnfQualifiedIdentifier qualifiedIdentifier)
        {
            SettingIdentifier = settingIdentifier;
            QualifiedIdentifier = qualifiedIdentifier;
            this._hashCode = (SettingIdentifier, QualifiedIdentifier).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfSetting>(obj, out var other) &&
                   (SettingIdentifier, QualifiedIdentifier).Equals((other.SettingIdentifier, other.QualifiedIdentifier));
        }

        public override int GetHashCode()
        {
            return this._hashCode;
        }
    }
}