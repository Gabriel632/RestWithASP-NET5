using System.Text;

namespace RestWithASPNET5.Hypermedia
{
    public class HyperMediaLink
    {
        public string Rel { get; set; }
        private string href;
        public string Href {
            get
            {
                object _lock = new();
                lock (_lock)
                {
                    StringBuilder sb = new(href);
                    return sb.Replace("%2f", "/").ToString();
                }
            }
            set { href = value; }
        }
        public string Type { get; set; }
        public string Action { get; set; }
    }
}
