using System.IO;
using Newtonsoft.Json.Linq;
using ParseStream;

namespace JSON
{
    public static class JSON
    {
        public static JObject toJSON(this string str) => JObject.Parse(str);
        public static JArray toJSONArray(this string str) => JArray.Parse(str);
        public static JObject toJSON(this Stream stream) => JObject.Parse(stream.toString());
        public static JArray toJSONArray(this Stream stream) => JArray.Parse(stream.toString());
    }
}