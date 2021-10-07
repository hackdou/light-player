using System;
using System.Text;

namespace LightPlayer.Desktop.Utils
{
    public static class Base64Utils
    {
        public static string ToBase64(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static string FromBase64(string text)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(text));
        }
    }
}