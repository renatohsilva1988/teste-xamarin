using System;
using Xamarin.Forms;

namespace MarvelApp.Helpers
{
    public static class ResourcesHelper
    {
        public const string DynamicTextColor = nameof(DynamicTextColor);
        public const string DynamicSecondaryTextColor = nameof(DynamicSecondaryTextColor);

        public const string DynamicNavigationBarColor = nameof(DynamicNavigationBarColor);
        public const string DynamicBackgroundColor = nameof(DynamicBackgroundColor);
        public const string DynamicBarColor = nameof(DynamicBarColor);

        public const string DynamicHasShadow = nameof(DynamicHasShadow);

        public static void SetDynamicResource(string targetResourceName, string sourceResourceName)
        {
            if (!Application.Current.Resources.TryGetValue(sourceResourceName, out var value))
            {
                throw new InvalidOperationException($"key {sourceResourceName} not found in the resource dictionary");
            }

            Application.Current.Resources[targetResourceName] = value;
        }

        public static void SetDynamicResource<T>(string targetResourceName, T value)
        {
            Application.Current.Resources[targetResourceName] = value;
        }

        public static void SetDarkMode()
        {
            SetDynamicResource(DynamicBarColor, "ColorLighterBlueGray");
            SetDynamicResource(DynamicHasShadow, false);
            SetDynamicResource(DynamicTextColor, "ColorWhite");
            SetDynamicResource(DynamicBackgroundColor, "ColorBlack");
        }

        public static void SetLightMode()
        {
            SetDynamicResource(DynamicBarColor, "ColorLighterBlueGray");
            SetDynamicResource(DynamicHasShadow, true);
            SetDynamicResource(DynamicTextColor, "ColorBlack");
            SetDynamicResource(DynamicBackgroundColor, "ColorWhite");
        }    
    }
}
