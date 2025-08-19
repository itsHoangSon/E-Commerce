using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuildingBlock.Extensions
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };


        public static T? FromJson<T>(this string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return default(T?);
            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }


        public static string ToJson<T>(this T obj) =>
            JsonSerializer.Serialize<T>(obj, _jsonOptions);

        public static string ToJsonV2(this object obj)
        {
            try
            {
                if (obj == null) return null;

                if (typeof(Stream).IsAssignableFrom(obj.GetType())) return "Stream";

                return JsonSerializer.Serialize(obj, _jsonOptions);
            }
            catch (Exception ex)
            {
                if (!obj.GetType().IsSerializable)
                    return obj.GetType().FullName;
                throw ex;
            }
        }
    }
}
