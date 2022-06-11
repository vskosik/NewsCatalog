using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatalog.DAL.Infrastructure
{
    public static class NewsApiConfig
    {
        public static string BaseUrl { get; set; } = "https://newsapi.org/v2/";
        public static string ApiKey { get; } = "b6cf136c39864c85894ba17c1e61e459";
    }
}
