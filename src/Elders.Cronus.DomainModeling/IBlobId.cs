using System;

namespace Elders.Cronus;

public interface IBlobId
{
    ReadOnlyMemory<byte> RawId { get; }
}
