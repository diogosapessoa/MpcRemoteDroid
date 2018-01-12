using System;
using System.Globalization;

using HtmlAgilityPack;

namespace MpcRemoteDroid.Src
{
    public static class HtmlDocumentExtensions
    {
        public static T GetValueById<T>(this HtmlDocument doc, string elementId, bool toLower = true) where T : IConvertible
        {
            var element = toLower ? elementId.ToLower() : elementId;
            var stringValue = doc.GetElementbyId(element).InnerText;
            var type = typeof(T);
            if (type.IsEnum)
            {
                return (T)Enum.Parse(type, stringValue);
            }
            return (T)Convert.ChangeType(stringValue, type, CultureInfo.InvariantCulture);
        }
    }
}