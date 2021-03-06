﻿using System;

namespace Billmorro.Schema.Verkauf
{
    public struct BonId
    {
        public readonly Guid Id;

        public BonId(Guid id)
        {
            Id = id;
        }

        public static implicit operator Guid(BonId that)
        {
            return that.Id;
        }

        public override string ToString()
        {
            return $"Bon-{Id}";
        }

        public static BonId Neu => new BonId(Guid.NewGuid());
    }
}