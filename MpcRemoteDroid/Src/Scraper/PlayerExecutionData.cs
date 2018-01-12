using HtmlAgilityPack;
using MpcRemoteDroid.Src.Enums;

namespace MpcRemoteDroid.Src.Scraper
{
    public class PlayerExecutionData
    {
        public long Duration { get; set; }
        public string DurationString { get; set; }
        public string File { get; set; }
        public string FileDir { get; set; }
        public string FileDirArg { get; set; }
        public string FilePath { get; set; }
        public string FilePathArg { get; set; }
        private int Muted { get; set; }
        public bool IsMuted
        {
            get { return Muted > 0; }
        }
        public double PlaybackRate { get; set; }
        public long Position { get; set; }
        public string PositionString { get; set; }
        public long ReloadTime { get; set; }
        public string Size { get; set; }
        public PlayerState State { get; set; }
        public string StateString { get; set; }
        public string Version { get; set; }
        public int VolumeLevel { get; set; }

        private PlayerExecutionData() { }

        public static PlayerExecutionData FromHtml(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return new PlayerExecutionData
            {
                Duration = doc.GetValueById<long>(nameof(Duration)),
                DurationString = doc.GetValueById<string>(nameof(DurationString)),
                File = doc.GetValueById<string>(nameof(File)),
                FileDir = doc.GetValueById<string>(nameof(FileDir)),
                FileDirArg = doc.GetValueById<string>(nameof(FileDirArg)),
                FilePath = doc.GetValueById<string>(nameof(FilePath)),
                FilePathArg = doc.GetValueById<string>(nameof(FilePathArg)),
                Muted = doc.GetValueById<int>(nameof(Muted)),
                PlaybackRate = doc.GetValueById<double>(nameof(PlaybackRate)),
                Position = doc.GetValueById<long>(nameof(Position)),
                PositionString = doc.GetValueById<string>(nameof(PositionString)),
                ReloadTime = doc.GetValueById<long>(nameof(ReloadTime)),
                Size = doc.GetValueById<string>(nameof(Size)),
                State = doc.GetValueById<PlayerState>(nameof(State)),
                StateString = doc.GetValueById<string>(nameof(StateString)),
                Version = doc.GetValueById<string>(nameof(Version)),
                VolumeLevel = doc.GetValueById<int>(nameof(VolumeLevel))
            };
        }
    }
}