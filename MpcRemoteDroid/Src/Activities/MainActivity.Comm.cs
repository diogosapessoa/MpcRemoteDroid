using System;
using System.Threading.Tasks;

using MpcRemoteDroid.Src.Communication;
using MpcRemoteDroid.Src.Enums;
using MpcRemoteDroid.Src.Preferences;
using MpcRemoteDroid.Src.Scraper;

namespace MpcRemoteDroid.Src.Activities
{
    public partial class MainActivity
    {
        public const string UrlCommandFormat = "http://{0}:{1}/command.html?wm_command=";
        public const string UrlSnapshotFormat = "http://{0}:{1}/snapshot.jpg";
        public const string UrlVariablesFormat = "http://{0}:{1}/variables.html";

        private string FormatUrl(string format, string host, string port)
        {
            var url = string.Format(format, host, port);
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new Exception("Badly formed Uri");
            }
            return url;
        }

        private string FormatCommandResource(string host, string port, Command command, int percent = 0)
        {
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
            {
                throw new ArgumentException("Argument is null or empty");
            }
            var url = FormatUrl(UrlCommandFormat, host, port);
            switch (command)
            {
                case Command.Position:
                    return $"{url}-1&percent={percent}";
                case Command.Volume:
                    return $"{url}-2&volume={percent}";
                default:
                    return $"{url}{(int)command}";
            }
        }


        private async Task ExecuteCommandAsync(Command command, int percentValue = 0)
        {
            var prefsInfo = new PrefsInfo(Prefs);
            if (prefsInfo.IsReady)
            {
                try
                {
                    var url = FormatCommandResource(prefsInfo.Host, prefsInfo.Port, command, percentValue.Clamp(0, 100));
                    await HttpComm.Request()
                        .WithContentType(HttpContentType.Html)
                        .WithMethod(HttpMethod.Get)
                        .WithUrl(url)
                        .ExecuteAsync();
                }
                catch (Exception) { }
            }
        }

        private async Task<byte[]> GetSnapshotAsync()
        {
            var prefsInfo = new PrefsInfo(Prefs);
            if (prefsInfo.IsReady)
            {
                try
                {
                    var url = FormatUrl(UrlSnapshotFormat, prefsInfo.Host, prefsInfo.Port);
                    return await HttpComm.Request()
                    .WithContentType(HttpContentType.Jpeg)
                    .WithMethod(HttpMethod.Get)
                    .WithUrl(url)
                    .GetResponseBytesAsync();
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        private async Task<PlayerExecutionData> GetVariablesAsync()
        {
            var prefsInfo = new PrefsInfo(Prefs);
            if (prefsInfo.IsReady)
            {
                try
                {
                    var url = FormatUrl(UrlVariablesFormat, prefsInfo.Host, prefsInfo.Port);
                    var html = await HttpComm.Request()
                    .WithContentType(HttpContentType.Html)
                    .WithMethod(HttpMethod.Get)
                    .WithUrl(url)
                    .GetResponseStringAsync();
                    return PlayerExecutionData.FromHtml(html);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }
    }
}