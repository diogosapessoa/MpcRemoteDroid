using Android.Content;

namespace MpcRemoteDroid.Src.Preferences
{
    public class PrefsInfo
    {
        public const string CommStartupKey = "connection_startup";
        public const string CommHostKey = "connection_host";
        public const string CommPortKey = "connection_port";

        public const string HostDefaultvalue = "127.0.0.1";
        public const string PortDefaultValue = "13579";

        public bool IsReady { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }

        public PrefsInfo(ISharedPreferences preferences)
        {
            Host = HostDefaultvalue;
            Port = PortDefaultValue;
            if (preferences != null)
            {
                IsReady = preferences.GetBoolean(CommStartupKey, false);
                Host = preferences.GetString(CommHostKey, HostDefaultvalue);
                Port = preferences.GetString(CommPortKey, PortDefaultValue);
            }
        }

        public override string ToString()
        {
            return $"[PrefsCommInfo: IsReady={IsReady}, Host={Host}, Port={Port}]";
        }
    }
}