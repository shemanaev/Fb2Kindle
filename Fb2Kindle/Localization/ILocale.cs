using System.Collections.Generic;

namespace Fb2Kindle.Localization
{
    internal interface ILocale
    {
        Dictionary<string, string> Messages { get; }
    }
}
