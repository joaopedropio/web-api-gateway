using Newtonsoft.Json;
using System;

namespace WebAPIGateway.JSON
{
    public class SuccessJsonData : IJsonData
    {
        [JsonProperty(PropertyName = "success")]
        public string Success { get; set; }

        [JsonConstructor]
        public SuccessJsonData(string success)
        {
            this.Success = success;
        }
        public SuccessJsonData() { }

        public override bool Equals(object obj)
        {
            SuccessJsonData data;
            try
            {
                data = (SuccessJsonData)obj;
            }
            catch (Exception)
            {
                return false;
            }

            return this.Success == data.Success;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Success);
        }
    }
}
