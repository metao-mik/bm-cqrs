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

        public static explicit operator ArtikelId(string valid_guid)
        {
            return new ArtikelId(new Guid(valid_guid));
        }

        public override string ToString()
        {
            return $"Artikel-{Id}";
        }

        public static ArtikelId Neu => new ArtikelId(Guid.NewGuid());

        public static bool operator ==(ArtikelId lhs, ArtikelId rhs)
        {
          return lhs.Equals(rhs);
        }

        public static bool operator !=(ArtikelId lhs, ArtikelId rhs)
        {
          return !(lhs==rhs);
        }
    }
}
