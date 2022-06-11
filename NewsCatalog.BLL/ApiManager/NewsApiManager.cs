using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsCatalog.BLL.DTO;
using NewsCatalog.DAL.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatalog.BLL.ApiManager
{
    public class NewsApiManager
    {
        NetworkManager networkManager;

        public NewsApiManager(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }

        public ObservableCollection<BookmarkDTO> GetTopHeadlinesArticles(
            string keyWord, Countries? country, Categories? category, Languages? lang, int? page)
        {
            var newsApiClient = new NewsApiClient(NewsApiConfig.ApiKey);

            var request = new TopHeadlinesRequest
            {
                Q = keyWord,
                Country = country,
                Category = category,
                Language = lang,
                Page = page ?? 1
            };

            var articlesResponse = newsApiClient.GetTopHeadlines(request);

            var articles = new ObservableCollection<BookmarkDTO>();

            if (articlesResponse.Status == Statuses.Ok)
            {
                foreach (var article in articlesResponse.Articles)
                {
                    articles.Add(new BookmarkDTO()
                    {
                        Title = article.Title,
                        Author = article.Author,
                        Description = article.Description,
                        SourceUrl = article.Url,
                        ImageUrl = article.UrlToImage,
                        PublishedData = article.PublishedAt.ToString(),
                        Content = article.Content,
                        IsBanned = false
                    });
                }
            }

            return articles;

            //-------------------------------------------------------------------------------------//

            /*if (parameters.ContainsKey("source"))
            {
                if (parameters.ContainsKey("country") || parameters.ContainsKey("category"))
                {
                    throw new ArgumentException("Incorrect param input!");
                }
            }

            string url = $"{NewsApiConfig.BaseUrl}top-headlines?apiKey={NewsApiConfig.ApiKey}";

            foreach (var parameter in parameters)
            {
                url += $"&{parameter.Key}={parameter.Value}";
            }

            string json = networkManager.GetJson(url);
            var articleSearch = JObject.Parse(json);
            IList<JToken> results;

            try
            {
                results = articleSearch["articles"].Children().ToList();
            }
            catch (Exception)
            {
                throw;
            }

            var articles = new ObservableCollection<ArticleDTO>();

            foreach (var token in results)
            {
                articles.Add(new ArticleDTO()
                {
                    Title = token["title"].ToString(),
                    Author = token["author"].ToString(),
                    Description = token["description"].ToString(),
                    SourceUrl = token["url"].ToString(),
                    ImageUrl = token["urlToImage"].ToString(),
                    PublishedData = token["publishedAt"].ToString(),
                    Content = token["content"].ToString()
                });
            }

            return articles; */
        }

        public ObservableCollection<BookmarkDTO> GetEverything(
            string keyWord, Languages? lang, DateTime? from, DateTime? to, SortBys? sortBy, int? page)
        {
            var newsApiClient = new NewsApiClient(NewsApiConfig.ApiKey);

            var request = new EverythingRequest
            {
                Language = lang,
                Q = keyWord,
                SortBy = sortBy,
                From = from,
                To = to,
                Page = page ?? 1
            };

            var articlesResponse = newsApiClient.GetEverything(request);

            var articles = new ObservableCollection<BookmarkDTO>();

            if (articlesResponse.Status == Statuses.Ok)
            {
                foreach (var article in articlesResponse.Articles)
                {
                    articles.Add(new BookmarkDTO()
                    {
                        Title = article.Title,
                        Author = article.Author,
                        Description = article.Description,
                        SourceUrl = article.Url,
                        ImageUrl = article.UrlToImage,
                        PublishedData = article.PublishedAt.ToString(),
                        Content = article.Content
                    });
                }
            }

            return articles;
        }
    }
}
