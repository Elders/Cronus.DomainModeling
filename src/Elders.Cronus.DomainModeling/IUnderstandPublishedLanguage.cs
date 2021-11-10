using System.Collections.Generic;

namespace Elders.Cronus;

public interface IUnderstandPublishedLanguage
{
    IEnumerable<IPublicEvent> UncommittedPublicEvents { get; }
}
