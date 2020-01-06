using System.Collections.Generic;

namespace Fb2Kindle.Localization
{
    internal class Ru : ILocale
    {
        public Dictionary<string, string> Messages { get; } = new Dictionary<string, string>
        {
            {"ConverterError", "Ошибка конвертера."},
            {"ConverterNotFound", "fb2converter не найден!"},
            {"DeviceFound", "Kindle подключен как {0}"},
            {"DeviceNotFound", "Kindle не обнаружен!"},
            {"Success", "Отправлено книг: {0}."},
            {"Working", "Обрабатываю книгу..."},
        };
    }
}
