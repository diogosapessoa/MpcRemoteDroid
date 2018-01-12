using HockeyApp.Android;

namespace MpcRemoteDroid.Src.HockeyAppCustom
{
    public class CrashListener : CrashManagerListener
    {
        public CrashListener() : base() { }

        public override bool ShouldAutoUploadCrashes() => true;
    }
}