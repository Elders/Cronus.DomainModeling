using System;

namespace Elders.Cronus;

public interface IBlobId
{
    ReadOnlySpan<byte> RawId { get; }
}
