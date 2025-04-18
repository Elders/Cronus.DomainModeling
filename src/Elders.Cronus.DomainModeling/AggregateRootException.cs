﻿using System;

namespace Elders.Cronus;

/// <summary>
/// 
/// </summary>
[Serializable]
public class AggregateRootException : Exception
{
    /// <summary>
    /// Constructs a new AggregateRootException.
    /// </summary>
    /// <param name="message">The exception message</param>
    public AggregateRootException(string message) : base(message) { }

    /// <summary>
    /// Constructs a new AggregateRootException.
    /// </summary>
    public AggregateRootException() { }

    /// <summary>
    /// Constructs a new AggregateRootException.
    /// </summary>
    /// <param name="message">The exception message</param>
    /// <param name="innerException">The inner exception</param>
    public AggregateRootException(string message, Exception innerException) : base(message, innerException) { }
}
