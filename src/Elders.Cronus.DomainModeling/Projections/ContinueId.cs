using System;

namespace Elders.Cronus.Projections;

public sealed class ContinueId : IBlobId
{
    public ReadOnlySpan<byte> RawId => ReadOnlySpan<byte>.Empty;
}
