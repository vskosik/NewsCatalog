using NewsAPI.Constants;
using NewsCatalog.BLL.ApiManager;
using NewsCatalog.BLL.DTO;
using NewsCatalog.BLL.Services;
using NewsCatalog.UI.Infrastructure;
using NewsCatalog.UI.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NewsCatalog.UI.ViewModel
{
    public class MainViewModel : BaseNotifyPropertyChanged
    {
        #region private fields and properties

        private IBookmarkService _bookmarkService;
        private IBannedArticleService _bannedArticleService;
        private IAdminService _adminService;

        private ObservableCollection<BookmarkDTO> _articles;
        private ObservableCollection<BookmarkDTO> _bookmarks;
        private ObservableCollection<BannedArticleDTO> _bannedArticles;

        private BookmarkDTO _selectedArticle;
        private BookmarkDTO _selectedBookmark;
        private BannedArticleDTO _selectedBannedArticle;

        private string _keyWord;
        private Countries? _country;
        private Categories? _category;
        private Languages? _language;
        private DateTime? _from;
        private DateTime? _to;
        private SortBys? _sortBy;
        private int _page = 1;
        private bool _searchMode;

        private NewsApiManager _newsApiManager;

        public BookmarkDTO SelectedArticle
        {
            get => _selectedArticle;
            set
            {
                _selectedArticle = value;
                SingletonArticle.SelectedArticle = value;
                NotifyOnPropertyChanged();
            }
        }

        public BookmarkDTO SelectedBookmark
        {
            get => _selectedBookmark;
            set
            {
                _selectedBookmark = value;
                NotifyOnPropertyChanged();
            }
        }

        public BannedArticleDTO SelectedBannedArticle
        {
            get => _selectedBannedArticle;
            set
            {
                _selectedBannedArticle = value;
                NotifyOnPropertyChanged();
            }
        }

        public Countries? Country
        {
            get => _country;
            set
            {
                _country = value;
                NotifyOnPropertyChanged();
            }
        }

        public Categories? Category
        {
            get => _category;
            set
            {
                _category = value;
                NotifyOnPropertyChanged();
            }
        }

        public Languages? Language
        {
            get => _language;
            set
            {
                _language = value;
                NotifyOnPropertyChanged();
            }
        }

        public DateTime? From
        {
            get => _from;
            set
            {
                _from = value;
                NotifyOnPropertyChanged();
            }
        }

        public DateTime? To
        {
            get => _to;
            set
            {
                _to = value;
                NotifyOnPropertyChanged();
            }
        }

        public SortBys? SortBy
        {
            get => _sortBy;
            set
            {
                _sortBy = value;
                NotifyOnPropertyChanged();
            }
        }

        public string KeyWord
        {
            get => _keyWord;
            set
            {
                _keyWord = value;
                NotifyOnPropertyChanged();
            }
        }

        public int TotalResults => _articles.Count;

        public int MaxPage => (TotalResults % 20 == 0 ? TotalResults / 20 : TotalResults / 20 + 1);

        public int Page
        {
            get => _page;
            set
            {

                if (value >= MaxPage)
                {
                    _page = MaxPage;
                }
                else if (value <= 0)
                {
                    _page = 1;
                }
                else
                {
                    _page = value;
                }

                NotifyOnPropertyChanged();
            }
        }

        public bool SearchMode
        {
            get => _searchMode;
            set
            {
                _searchMode = value;
                NotifyOnPropertyChanged();
            }
        }

        public ObservableCollection<BookmarkDTO> Articles
        {
            get => _articles;
            set
            {
                _articles = value;
                NotifyOnPropertyChanged();
            }
        }

        public ObservableCollection<BookmarkDTO> Bookmarks
        {
            get => _bookmarks;
            set
            {
                _bookmarks = value;
                NotifyOnPropertyChanged();
            }
        }

        public ObservableCollection<BannedArticleDTO> BannedArticles
        {
            get => _bannedArticles;
            set
            {
                _bannedArticles = value;
                NotifyOnPropertyChanged();
            }
        }

        public IEnumerable<Languages> Languages => Enum.GetValues(typeof(Languages)).Cast<Languages>();

        public IEnumerable<Countries> Countries => Enum.GetValues(typeof(Countries)).Cast<Countries>();

        public IEnumerable<Categories> Categories => Enum.GetValues(typeof(Categories)).Cast<Categories>();

        public IEnumerable<SortBys> SortBys => Enum.GetValues(typeof(SortBys)).Cast<SortBys>();

        #endregion

        public MainViewModel(IBookmarkService articleService,
                             IBannedArticleService bannedArticleService,
                             IAdminService adminService,
                             NewsApiManager newsApiManager)
        {
            _bookmarkService = articleService;
            _bannedArticleService = bannedArticleService;
            _adminService = adminService;

            _newsApiManager = newsApiManager;

            SearchMode = true;

            KeyWord = "Війна";
            Language = NewsAPI.Constants.Languages.UK;
            SortBy = NewsAPI.Constants.SortBys.Relevancy;

            InitCommands();
            LoadBookmarks();
            LoadBannedArticles();
        }

        #region public methods

        private void InitCommands()
        {
            SaveBookmarkCommand = new RelayCommand((param) =>
            {
                SelectedArticle = Articles.FirstOrDefault(x => x.SourceUrl == (string)param);
                Task.Run(() =>
                {
                    _bookmarkService.Create(SelectedArticle);
                });
                Bookmarks.Add(SelectedArticle);
            });

            DeleteBookmarkCommand = new RelayCommand((param) =>
            {
                SelectedBookmark = Bookmarks.FirstOrDefault(x => x.Id == (int)param);
                Task.Run(() =>
                {
                    _bookmarkService.Delete(SelectedBookmark);
                });
                Bookmarks.Remove(SelectedBookmark);
            });

            SearchCommand = new RelayCommand((param) =>
            {
                if (_searchMode)
                {
                    LoadEverythingNews();
                }
                else
                {
                    LoadTopHeadlinesNews();
                }
            });

            ViewArticleCommand = new RelayCommand((param) =>
            {
                SingletonArticle.SelectedArticle = Articles.FirstOrDefault(x => x.SourceUrl == (string)param);
                var articleView = new ArticleView();
                articleView.Show();
            });

            NextPageCommand = new RelayCommand((param) =>
            {
                Page++;

                if (_searchMode)
                {
                    LoadEverythingNews();
                }
                else
                {
                    LoadTopHeadlinesNews();
                }
            });

            PreviousPageCommand = new RelayCommand((param) =>
            {
                Page--;

                if (_searchMode)
                {
                    LoadEverythingNews();
                }
                else
                {
                    LoadTopHeadlinesNews();
                }
            });

            LoginCommand = new RelayCommand((param) =>
            {
                var loginView = new LoginView();
                loginView.ShowDialog();
                if (SingletonLoginResult.IsAllowed)
                {
                    var adminView = new AdminView();
                    ((Window)param).Hide();
                    adminView.ShowDialog();
                    ((Window)param).Show();
                }
            });

            BanCommand = new RelayCommand((param) =>
            {
                SelectedArticle = Articles.FirstOrDefault(x => x.SourceUrl == (string)param);
                var bannedArticle = new BannedArticleDTO
                {
                    Url = SelectedArticle.SourceUrl
                };
                Articles.Remove(SelectedArticle);
                _bannedArticleService.Create(bannedArticle);
                BannedArticles.Add(bannedArticle);
            });

            UnbanCommand = new RelayCommand((param) =>
            {
                SelectedBannedArticle = BannedArticles.FirstOrDefault(x => x.Id == (int)param);
                _bannedArticleService.Delete(SelectedBannedArticle);
                BannedArticles.Remove(SelectedBannedArticle);
            });
        }

        private void LoadBookmarks()
        {
            Task.Run(async () =>
            {
                var bookmarks = await _bookmarkService.GetAllAsync();
                Bookmarks = new ObservableCollection<BookmarkDTO>(bookmarks);
            });
        }

        private void LoadBannedArticles()
        {
            Task.Run(async () =>
            {
                var bannedArticles = await _bannedArticleService.GetAllAsync();
                BannedArticles = new ObservableCollection<BannedArticleDTO>(bannedArticles);
            });
        }

        private void LoadTopHeadlinesNews()
        {
            Task.Run(() =>
            {
                var articles = _newsApiManager.GetTopHeadlinesArticles(KeyWord, Country, Category, Language, Page);
                var banned = _bannedArticleService.GetAll();

                foreach (var article in articles)
                {
                    if (banned.Any(x => x.Url == article.SourceUrl))
                    {
                        articles.Remove(article);
                    }
                }
                Articles = new ObservableCollection<BookmarkDTO>(articles);
            });
        }

        private void LoadEverythingNews()
        {
            Task.Run(() =>
            {
                var articles = _newsApiManager.GetEverything(KeyWord, Language, From, To, SortBy, Page);
                var banned = _bannedArticleService.GetAll();
                var toRemove = new ObservableCollection<BookmarkDTO>();

                foreach (var article in articles)
                {
                    if (banned.Any(x => x.Url == article.SourceUrl))
                    {
                        toRemove.Add(article);
                    }
                }

                foreach (var article in toRemove)
                {
                    articles.Remove(article);
                }

                Articles = new ObservableCollection<BookmarkDTO>(articles);
            });
        }

        #endregion


        #region commands

        public ICommand SaveBookmarkCommand { get; private set; }
        public ICommand DeleteBookmarkCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand ViewArticleCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }
        public ICommand BanCommand { get; private set; }
        public ICommand UnbanCommand { get; private set; }

        #endregion
    }
}
