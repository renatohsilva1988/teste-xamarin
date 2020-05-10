using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Flurl;
using MarvelApp.Model;
using MarvelApp.ServicesContracts;

namespace MarvelApp.Helpers
{
    public static class MarvelApiExtensions
    {
        public static Url AddMarvelAuthenticationParameters(this Url url, string publicKey, string privateKey)
        {
            var timestamp = DateTime.Now.ToFileTime();
            var contentToHash = $"{timestamp}{privateKey}{publicKey}";
            var hash = CalculateMd5Hash(contentToHash).ToLower();

            return url
                .SetQueryParam("apikey", publicKey)
                .SetQueryParam("ts", timestamp)
                .SetQueryParam("hash", hash);
        }

        public static Url AddMarvelPageParameters(this Url url, PageData pageData) => url
            .SetQueryParam("limit", pageData.PageSize)
            .SetQueryParam("offset", pageData.RecordOffset);

        private static string CalculateMd5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (byte t in hash)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
