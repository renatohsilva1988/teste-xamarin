using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MarvelApp.Helpers
{
    public class JsonHandler
    {

        private readonly static string applicationJson = "application/json";

        public static string ToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, GetJsonSerializerSettings());
        }

        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, GetJsonSerializerSettings());
        }

        public static IList<T> FromJsonList<T>(string json)
        {
            return FromJson<List<T>>(json);
        }

        public static StringContent GetStringContent(string json)
        {
            return new StringContent(json, Encoding.UTF8, applicationJson);
        }

        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            settings.Converters.Add(new StringEnumConverter());

            return settings;
        }
    }
}
