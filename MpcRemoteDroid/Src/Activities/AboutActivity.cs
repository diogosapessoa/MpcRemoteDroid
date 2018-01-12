using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Widget;

namespace MpcRemoteDroid.Src.Activities
{
    [Activity(Label = "@string/about", ParentActivity = typeof(SettingsActivity), ScreenOrientation = ScreenOrientation.Portrait)]
    [IntentFilter(actions: new[] { "com.oneguy.mpcremote.about" }, Categories = new[] { Android.Content.Intent.CategoryDefault })]
    public class AboutActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.About);

            var version = FindViewById<TextView>(Resource.Id.about_version);
            try
            {
                version.Text += $" {PackageManager.GetPackageInfo(PackageName, 0).VersionName}";
            }
            catch (Exception ex)
            {
                Log.Debug(nameof(AboutActivity), ex.Message);
            }
        }
    }
}