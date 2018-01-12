using Android.Content;

using MpcRemoteDroid.Src.Activities;

namespace MpcRemoteDroid.Src.BroadcastReceivers
{
    [BroadcastReceiver]
    public class NotificationBroadcastReceiver : BroadcastReceiver
    {
        public const string ActionMediaPlayerControlFwd = "com.oneguy.remote.intent.action.MEDIA_PLAYER_CONTROL_FORWARD";
        public const string ActionMediaPlayerControlPlayAndPause = "com.oneguy.remote.intent.action.MEDIA_PLAYER_CONTROL_PLAY_AND_PAUSE";
        public const string ActionMediaPlayerControlRew = "com.oneguy.remote.intent.action.MEDIA_PLAYER_CONTROL_BACK";

        public override void OnReceive(Context context, Intent intent)
        {
            if (context is MainActivity mainActivity)
            {
                switch (intent.Action)
                {
                    case ActionMediaPlayerControlFwd:
                        mainActivity.SkipForwardCommandAsync();
                        break;
                    case ActionMediaPlayerControlPlayAndPause:
                        mainActivity.PlayAndPauseCommandAsync();
                        break;
                    case ActionMediaPlayerControlRew:
                        mainActivity.SkipBackCommandAsync();
                        break;
                }
            }
        }
    }
}