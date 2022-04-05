namespace Elders.Cronus;

public interface IUrnFormatProvider
{
    /// <summary>
    /// Gets a string representation of an urn
    /// </summary>
    /// <param name="urn">The urn</param>
    /// <returns>Retutns a string representation of the provided urn</returns>
    string Format(IUrn urn);

    /// <summary>
    /// Gets a plain urn string from specially formatted string created by <see cref="Format(IUrn)"/>
    /// </summary>
    /// <param name="input">The string containing preformatted string urn</param>
    /// <returns>Returns plain urn string</returns>
    string Parse(string input);
}
