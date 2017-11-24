using System;

namespace Billmorro.Schema.Verkauf
{
    public struct PositionId
    {
        public readonly Guid Id;

        public PositionId(Guid id)
        {
            Id = id;
        }

        public static implicit operator Guid(PositionId that)
        {
            return that.Id;
        }

        public override string ToString()
        {
            return $"Position-{Id}";
        }

        public static PositionId Neu => new PositionId(Guid.NewGuid());
    }
}