using Android.App;
using Android.Content;

using MpcRemoteDroid.Src.BroadcastReceivers;
using MpcRemoteDroid.Src.Enums;

namespace MpcRemoteDroid.Src.Activities
{
    public partial class MainActivity
    {
        public const int NotificationId = 321;

        private PendingIntent CreateActionIntent(string action) =>
            PendingIntent.GetBroadcast(this, NotificationId, new Intent(action), PendingIntentFlags.UpdateCurrent);

        private Notification.Action CreateAction(int resActionIcon, int id, string actionBroadcast) =>
            new Notification.Action.Builder(resActionIcon, GetString(id), CreateActionIntent(actionBroadcast)).Build();

        private void ShowNotification(string text, string info, int progress)
        {
            var intent = new Intent(this, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            var back = CreateAction(Android.Resource.Drawable.IcMediaRew, Resource.String.back, NotificationBroadcastReceiver.ActionMediaPlayerControlRew);
            var playAndPause = CreateAction(
                PlayerData.State != PlayerState.Playing ? Android.Resource.Drawable.IcMediaPlay : Android.Resource.Drawable.IcMediaPause,
                PlayerData.State != PlayerState.Playing ? Resource.String.play : Resource.String.pause,
                NotificationBroadcastReceiver.ActionMediaPlayerControlPlayAndPause);
            var forward = CreateAction(Android.Resource.Drawable.IcMediaFf, Resource.String.forward, NotificationBroadcastReceiver.ActionMediaPlayerControlFwd);

            var notification = new Notification.Builder(this)
                .SetAutoCancel(false)
                .SetWhen(0)
                .SetProgress(100, progress, progress < 0)
                .SetColor(Notification.ColorDefault)
                .SetCategory(Notification.CategoryProgress)
                .SetPriority((int)NotificationPriority.Max)
                .SetVisibility(NotificationVisibility.Public)
                .SetContentIntent(pendingIntent)
                .SetSmallIcon(Resource.Drawable.ic_settings_remote_white_24dp)
                .SetContentTitle(ApplicationInfo.LoadLabel(PackageManager))
                .SetContentText(text)
                .SetContentInfo(info)
                .AddAction(back)
                .AddAction(playAndPause)
                .AddAction(forward)
                .Build();

            var notificatoinManager = GetSystemService(NotificationService) as NotificationManager;
            notificatoinManager.Notify(NotificationId, notification);
        }

        public void HideNotification()
        {
            var notificatoinManager = GetSystemService(NotificationService) as NotificationManager;
            notificatoinManager.Cancel(NotificationId);
        }
    }
}