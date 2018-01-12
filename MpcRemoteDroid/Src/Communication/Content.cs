using System.Collections.Generic;

using MpcRemoteDroid.Src.Enums;

namespace MpcRemoteDroid.Src.Communication
{
    public class Content
    {
        private static readonly Dictionary<HttpContentType, string> ContentTypes = new Dictionary<HttpContentType, string>
        {
            { HttpContentType.Html, "text/hml" },
            { HttpContentType.Json, "application/json" },
            { HttpContentType.Jpeg, "image/jpeg" },
            { HttpContentType.Png, "image/png" }
        };

        public static string GetTypeMime(HttpContentType contentType)
        {
            return ContentTypes[contentType];
        }
    }
}