using System;

namespace Billmorro.Schema.Produkte
{
    public struct ArtikelId
    {
        public readonly Guid Id;

        public ArtikelId(Guid id)
        {
            Id = id;
        }

        public static implicit operator Guid(ArtikelId that)
        {
            return that.Id;
        }

        public static explicit operator ArtikelId(Guid that)
        {
            return new ArtikelId(that);
        }

        public override string ToString()
        {
            return $"Artikel-{Id}";
        }

        public static ArtikelId Neu => new ArtikelId(Guid.NewGuid());
    }
}
