using System.Collections.Generic;

namespace Fb2Kindle.Localization
{
    internal class En : ILocale
    {
        public Dictionary<string, string> Messages { get; } = new Dictionary<string, string>
        {
            {"ConverterError", "Converter error."},
            {"ConverterNotFound", "fb2converter not found!"},
            {"DeviceFound", "Kindle found as {0}"},
            {"DeviceNotFound", "Kindle not found!"},
            {"Success", "Successfully converted {0} book(s)."},
            {"Working", "Processing books..."},
        };
    }
}
