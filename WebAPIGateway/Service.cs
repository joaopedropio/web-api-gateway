namespace WebAPIGateway
{
    public class Service
    {
        public string Name { get; set; }
        public string URL { get; set; }

        public Service(string name, string url)
        {
            Name = name;
            URL = url;
        }
    }
}
