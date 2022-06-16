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
using System.Threading.Tasks;
using System.Windows.Input;

namespace NewsCatalog.UI.ViewModel
{
    public class MainViewModel : BaseNotifyPropertyChanged
    {
        #region private fields and properties

        private IBookmarkService _bookmarkService;

        private ObservableCollection<BookmarkDTO> _articles;
        private ObservableCollection<BookmarkDTO> _bookmarks;
        private ObservableCollection<BookmarkDTO> _bannedArticles;

        private BookmarkDTO _selectedArticle;
        private BookmarkDTO _selectedBookmark;
        private BookmarkDTO _selectedBannedArticle;

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
                ArticleViewModel.SelectedArticle = value;
                NotifyOnPropertyChanged();
            }
        }

        public BookmarkDTO SelectedBookmark
        {
            get => _selectedBookmark;
            set
            {
                _selectedBookmark = value;
                ArticleViewModel.SelectedArticle = value;
                NotifyOnPropertyChanged();
            }
        }

        public BookmarkDTO SelectedBannedArticle
        {
            get => _selectedBannedArticle;
            set
            {
                _selectedBannedArticle = value;
                ArticleViewModel.SelectedArticle = value;
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

        public int Page
        {
            get => _page;
            set
            {
                if (value <= 0 || value > 20)
                {
                    return;
                }

                _page = value;
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

        public ObservableCollection<BookmarkDTO> BannedArticles
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
                             NewsApiManager newsApiManager)
        {
            _bookmarkService = articleService;

            _newsApiManager = newsApiManager;

            SearchMode = true;

            InitCommands();
            Load();
        }

        #region public methods

        private void InitCommands()
        {
            ExitCommand = new RelayCommand((param) =>
            {
                PagesPull.CurrentPage = PagesPull.Pages["MainView"];
                Switcher.Switch(PagesPull.Pages["MainView"]);
            });

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
                    LoadEverythingNewsAsync();
                }
                else
                {
                    LoadTopHeadlinesNewsAsync();
                }
            });

            ViewArticleCommand = new RelayCommand((param) =>
            {
                SelectedArticle = Articles.FirstOrDefault(x => x.SourceUrl == (string)param);
                Switcher.Switch(PagesPull.Pages["ArticleView"]);
            });

            ViewBookmarkCommand = new RelayCommand((param) =>
            {
                SelectedBookmark = Bookmarks.FirstOrDefault(x => x.SourceUrl == (string)param);
                Switcher.Switch(PagesPull.Pages["ArticleView"]);
            });

            ViewBannedArticleCommand = new RelayCommand((param) =>
            {
                SelectedBannedArticle = BannedArticles.FirstOrDefault(x => x.SourceUrl == (string)param);
                Switcher.Switch(PagesPull.Pages["ArticleView"]);
            });

            NextPageCommand = new RelayCommand((param) =>
            {
                if (Articles == null || Articles?.Count == 0)
                {
                    return;
                }

                Page++;
                SearchCommand.Execute(param);

                if (Articles == null || Articles?.Count == 0)
                {
                    Page--;
                    SearchCommand.Execute(param);
                }
            });

            PreviousPageCommand = new RelayCommand((param) =>
            {
                if ((Articles == null || Articles?.Count == 0) && Page == 1)
                {
                    return;
                }

                Page--;
                SearchCommand.Execute(param);
            });

            LoginCommand = new RelayCommand((param) =>
            {
                var loginView = new LoginView();
                SingletonLoginResult.IsLogged = false;
                loginView.ShowDialog();

                if (SingletonLoginResult.IsLogged)
                {
                    PagesPull.CurrentPage = PagesPull.Pages["AdminView"];
                    Switcher.Switch(PagesPull.Pages["AdminView"]);
                }
            });

            BanCommand = new RelayCommand((param) =>
            {
                SelectedArticle = Articles.FirstOrDefault(x => x.SourceUrl == (string)param);
                SelectedArticle.IsBanned = true;
                _bookmarkService.Create(SelectedArticle);
                BannedArticles.Add(SelectedArticle);
                Articles.Remove(SelectedArticle);
            });

            UnbanCommand = new RelayCommand((param) =>
            {
                SelectedBannedArticle = BannedArticles.FirstOrDefault(x => x.Id == (int)param);
                _bookmarkService.Delete(SelectedBannedArticle);
                BannedArticles.Remove(SelectedBannedArticle);
            });
        }

        private async void Load()
        {
            var bookmarks = (await _bookmarkService.GetAllAsync()).Where(x => !x.IsBanned);
            Bookmarks = new ObservableCollection<BookmarkDTO>(bookmarks);

            var bannedArticles = (await _bookmarkService.GetAllAsync()).Where(x => x.IsBanned);
            BannedArticles = new ObservableCollection<BookmarkDTO>(bannedArticles);
        }

        private async void LoadTopHeadlinesNewsAsync()
        {
            await Task.Run(() =>
            {
                var articles = _newsApiManager.GetTopHeadlinesArticles(KeyWord, Country, Category, Language, Page);
                BannedArticles = new ObservableCollection<BookmarkDTO>(_bookmarkService.GetAll().Where(x => x.IsBanned));
                var toRemove = new ObservableCollection<BookmarkDTO>();

                foreach (var article in articles)
                {
                    if (BannedArticles.Any(x => x.SourceUrl == article.SourceUrl))
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

        private async void LoadEverythingNewsAsync()
        {
            await Task.Run(() =>
            {
                var articles = _newsApiManager.GetEverything(KeyWord, Language, From, To, SortBy, Page);
                BannedArticles = new ObservableCollection<BookmarkDTO>(_bookmarkService.GetAll().Where(x => x.IsBanned));
                var toRemove = new ObservableCollection<BookmarkDTO>();

                foreach (var article in articles)
                {
                    if (BannedArticles.Any(x => x.SourceUrl == article.SourceUrl))
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

        public ICommand ExitCommand { get; private set; }
        public ICommand SaveBookmarkCommand { get; private set; }
        public ICommand DeleteBookmarkCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand ViewArticleCommand { get; private set; }
        public ICommand ViewBookmarkCommand { get; private set; }
        public ICommand ViewBannedArticleCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }
        public ICommand BanCommand { get; private set; }
        public ICommand UnbanCommand { get; private set; }

        #endregion
    }
}
