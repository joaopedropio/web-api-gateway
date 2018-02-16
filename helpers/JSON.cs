using System.IO;
using Newtonsoft.Json.Linq;
using ParseStream;

namespace JSON
{
    public static class JSON
    {
        public static JObject toJSON(this string str)
        {
            return JObject.Parse(str);
        }
        public static JArray toJSONArray(this string str)
        {
            return JArray.Parse(str);
        }
        
        public static JObject toJSON(this Stream stream)
        {
            return JObject.Parse(stream.toString());
        }
        public static JArray toJSONArray(this Stream stream)
        {
            return JArray.Parse(stream.toString());
        }
    }
}