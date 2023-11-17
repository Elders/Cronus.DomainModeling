using System;

namespace Elders.Cronus;

public interface IMessage
{
    DateTimeOffset Timestamp { get; }
}
