using System;

namespace Elders.Cronus;

/// <summary>
/// Uniform Resource Names (URNs) are intended to serve as persistent, location-independent, resource identifiers and are designed to make
/// it easy to map other namespaces (which share the properties of URNs) into URN-space.Therefore, the URN syntax provides a means to encode
/// character data in a form that can be sent in existing protocols, transcribed on most keyboards, etc.
/// </summary>
/// <remarks>
/// Where to use Base64/Base64URL/Plain
///
/// Base64 URL
///
/// - Transfer via HTTP
///
/// Base64
///
/// - Transfer via AMQP(RabbitMQ)
/// - In storages
///
/// Plain
///
/// - Only in Memory operations
/// </remarks>
/// <example>"urn:NID:NSS</example>
public interface IUrn : IEquatable<IUrn>
{
    /// <summary>
    /// Gets the Namespace Identifier. The Namespace ID determines the _syntactic_ interpretation of the Namespace Specific String
    /// </summary>
    string NID { get; }

    /// <summary>
    /// Gets the Namespace Specific String
    /// </summary>
    string NSS { get; }

    /// <summary>
    /// Gets the URN
    /// </summary>
    string Value { get; }

    /// <summary>
    /// The r-component is intended for passing parameters to URN resolution
    /// services and interpreted by those
    /// services.<see cref="https://tools.ietf.org/html/rfc8141#section-2.3.1"/>
    /// </summary>
    string R_Component { get; }

    /// <summary>
    /// The q-component is intended for passing parameters to either the
    /// named resource or a system that can supply the requested service, for
    ///  interpretation by that resource or system.
    /// <see cref="https://tools.ietf.org/html/rfc8141#section-2.3.2"/>
    /// </summary>
    string Q_Component { get; }

    /// <summary>
    /// The f-component is intended to be interpreted by the client as a
    //  specification for a location within, or region of, the named
    //  resource
    /// <see cref="https://tools.ietf.org/html/rfc8141#section-2.3.3"/>
    /// </summary>
    string F_Component { get; }
}
