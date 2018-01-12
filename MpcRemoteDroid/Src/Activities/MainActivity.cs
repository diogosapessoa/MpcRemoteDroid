using System.Threading.Tasks;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Content;
using Android.Preferences;
using Android.Net.Wifi;

using HockeyApp.Android;

using MpcRemoteDroid.Src.BroadcastReceivers;
using MpcRemoteDroid.Src.HockeyAppCustom;

namespace MpcRemoteDroid.Src.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public partial class MainActivity : Activity
    {
        public const int VolumeSteps = 10;

        private SeekBar Volume { get; set; }
        private ImageView Preview { get; set; }
        private SeekBar Position { get; set; }
        private ImageButton Back { get; set; }
        private ImageButton PlayAndPause { get; set; }
        private ImageButton Forward { get; set; }

        private ISharedPreferences Prefs { get; set; }

        private NotificationBroadcastReceiver Broadcast { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            #region Properties and events
            Prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            Volume = FindViewById<SeekBar>(Resource.Id.seekbar_volume);
            Volume.StartTrackingTouch += VolumeStartTrackingTouch;
            Volume.StopTrackingTouch += VolumeStopTrackingTouch;

            Preview = FindViewById<ImageView>(Resource.Id.image_preview);

            Position = FindViewById<SeekBar>(Resource.Id.seekbar_position);
            Position.StartTrackingTouch += PositionStartTrackingTouch;
            Position.StopTrackingTouch += PositionStopTrackingTouch;

            Back = FindViewById<ImageButton>(Resource.Id.button_skip_back);
            Back.Click += BackClick;

            PlayAndPause = FindViewById<ImageButton>(Resource.Id.button_play_and_pause);
            PlayAndPause.Click += PlayAndPauseClick;

            Forward = FindViewById<ImageButton>(Resource.Id.button_skip_forward);
            Forward.Click += ForwardClick;
            #endregion

            Task.Run(UpdateViewWithDataFromPlayer);

            Broadcast = new NotificationBroadcastReceiver();
            var intentFilter = new IntentFilter();
            intentFilter.AddAction(NotificationBroadcastReceiver.ActionMediaPlayerControlFwd);
            intentFilter.AddAction(NotificationBroadcastReceiver.ActionMediaPlayerControlPlayAndPause);
            intentFilter.AddAction(NotificationBroadcastReceiver.ActionMediaPlayerControlRew);
            RegisterReceiver(Broadcast, intentFilter);

            CrashManager.Register(this, "6b762f5af26743b08b6f971555427c48", new CrashListener());
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.controls_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.controls_settings:
                    StartActivity(new Intent(this, typeof(SettingsActivity)));
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            IsVolumeChanging = true;
            switch (e.KeyCode)
            {
                case Keycode.VolumeDown:
                    if (e.Action == KeyEventActions.Down)
                    {
                        VolumeDecrease(Volume.Progress);
                    }
                    IsVolumeChanging = false;
                    return true;
                case Keycode.VolumeUp:
                    if (e.Action == KeyEventActions.Down)
                    {
                        VolumeIncrease(Volume.Progress);
                    }
                    IsVolumeChanging = false;
                    return true;
            }
            return base.DispatchKeyEvent(e);
        }

        protected override void OnResume()
        {
            base.OnResume();

            var wifiManager = GetSystemService(WifiService) as WifiManager;
            if (!wifiManager.IsWifiEnabled)
            {
                wifiManager.SetWifiEnabled(true);
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            HideNotification();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            IsRunning = false;
            HideNotification();
            UnregisterReceiver(Broadcast);
        }
    }
}