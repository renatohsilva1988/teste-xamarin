using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;

namespace MarvelApp.Droid
{
    [Activity(Theme = "@style/Theme.Splash",
            MainLauncher = true,
            NoHistory = true)]
    public class SplashActivity : Activity
    {
        static readonly string _tag = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(_tag, "SplashActivity.OnCreate");
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { Startup(); });
            startupWork.Start();
        }

        public override void OnBackPressed() { }

        
        async void Startup()
        {
            Log.Debug(_tag, "Performing some startup work that takes a bit of time.");

            await Task.Delay(TimeSpan.FromMilliseconds(1000));

            Log.Debug(_tag, "Startup work is finished - starting MainActivity.");

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}