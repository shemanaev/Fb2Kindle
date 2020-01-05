namespace Fb2Kindle.Localization
{
    internal class LocaleManager
    {
        private ILocale _locale;

        public LocaleManager()
        {
            switch (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName)
            {
                case "ru":
                    _locale = new Ru();
                    break;

                default:
                    _locale = new En();
                    break;
            }
        }

        public string GetString(string name)
        {
            if (_locale.Messages.TryGetValue(name, out string message))
            {
                return message;
            }
            else
            {
                return name;
            }
        }

        public string GetString(string name, params object[] args)
        {
            return string.Format(GetString(name), args);
        }
    }
}
