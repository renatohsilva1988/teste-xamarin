namespace MarvelApp.Setup
{
    public static class Startup
    {
        public static string BaseAddress()
        {
            return AppSettingsManager.Settings["BaseAddress"];
        }

        public static string ApiPublicKey()
        {
            return AppSettingsManager.Settings["ApiPublicKey"];
        }

        public static string ApiPrivateKey()
        {
            return AppSettingsManager.Settings["ApiPrivateKey"];
        }
    }
}
