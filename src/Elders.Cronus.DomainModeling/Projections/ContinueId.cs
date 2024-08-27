using System;

namespace Elders.Cronus.Projections;

public sealed class ContinueId : IBlobId
{
    public ReadOnlyMemory<byte> RawId => ReadOnlyMemory<byte>.Empty;
}
