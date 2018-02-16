using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ParseStream
{
    public static class ParseStream
    {
        public static string toString(this Stream stream)
        {
            if(stream == null) return null;

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {  
                return reader.ReadToEnd();
            }
        }
    }
}