using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Preferences;

namespace MpcRemoteDroid.Src.Activities
{
    [Activity(Label = "@string/settings", ParentActivity = typeof(MainActivity), 
        ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SettingsActivity : PreferenceActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AddPreferencesFromResource(Resource.Xml.Preferences);

            foreach (var prefName in new[] { "connection_host", "connection_port" })
            {
                if (FindPreference(prefName) is EditTextPreference editTextPreference)
                {
                    editTextPreference.Summary = editTextPreference.Text;
                }
            }
        }
    }
}