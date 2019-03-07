using Newtonsoft.Json;
using System;

namespace WebAPIGateway.JSON
{
    public class UnknownErrorJsonData : IJsonData
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "stackTrace")]
        public string StackTrace { get; set; }

        [JsonConstructor]
        public UnknownErrorJsonData(string error, string stackTrace)
        {
            this.Error = error;
            this.StackTrace = stackTrace;
        }

        public UnknownErrorJsonData() { }

        public override bool Equals(object obj)
        {
            UnknownErrorJsonData data;
            try
            {
                data = (UnknownErrorJsonData)obj;
            }
            catch (Exception)
            {
                return false;
            }

            return this.Error == data.Error && this.StackTrace == data.StackTrace;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Error, StackTrace);
        }
    }
}
