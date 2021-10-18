using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;

namespace ADP.Assignment.Common.Extensions
{
    public static class JsonExtension
    {
        public static JsonSerializerSettings JsonSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    FloatFormatHandling = FloatFormatHandling.DefaultValue,
                    FloatParseHandling = FloatParseHandling.Decimal,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    Converters = new[] { new IsoDateTimeConverter { DateTimeStyles = System.Globalization.DateTimeStyles.AssumeLocal } }
                };
            }
        }

        public static string ToJson(this object objToJson, bool useSettings = false)
        {
            return JsonConvert.SerializeObject(objToJson, JsonSettings);
        }

        public static T ToObject<T>(this string stringToObject, bool useSettings = false)
        {
            return JsonConvert.DeserializeObject<T>(stringToObject, JsonSettings);
        }
    }
}