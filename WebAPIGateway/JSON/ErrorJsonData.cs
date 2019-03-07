using Newtonsoft.Json;
using System;

namespace WebAPIGateway.JSON
{
    public class ErrorJsonData : IJsonData
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonConstructor]
        public ErrorJsonData(string error)
        {
            this.Error = error;
        }

        public ErrorJsonData() { }

        public override bool Equals(object obj)
        {
            ErrorJsonData data;
            try
            {
                data = (ErrorJsonData)obj;
            }
            catch (Exception)
            {
                return false;
            }

            return this.Error == data.Error;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Error);
        }
    }
}
