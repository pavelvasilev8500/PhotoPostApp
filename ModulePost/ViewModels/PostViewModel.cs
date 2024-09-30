using DataModule;
using DataModule.Models;
using Microsoft.Win32;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace ModulePost.ViewModels
{
    internal class PostViewModel : BindableBase, INavigationAware
    {

        private readonly IRegionManager _regionManager;

        private int _model;
        private bool _isPopupOpen = false;
        private string _searchReq;
        private string _imagePath;

        private ObservableCollection<PostModel> _posts;
        private Visibility _deleteVisibility = Visibility.Collapsed;

        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand OpenPopupCommand { get; private set; }
        public DelegateCommand DeleteItemCommand { get; private set; }
        public DelegateCommand SelectImageCommand { get; private set; }
        public DelegateCommand LikeCommand { get; private set; }
        public DelegateCommand StatisticsCommand { get; private set; }

        public string SearchReq
        {
            get => _searchReq;
            set 
            { 
                SetProperty(ref _searchReq, value);
                Posts.Clear();
                if(PostData.AllPosts.Count > 0)
                {
                    if (value != null && value != "")
                    {
                        var req = from p in PostData.AllPosts
                                  where p.Name.Contains(SearchReq)
                                  select p;
                        foreach(var o in req)
                        {
                            Posts.Add(o);
                        }
                        if(Posts.Count == 0)
                        {
                            Posts.Add(new PostModel
                            {
                                Name = "Empty"
                            });
                        }
                    }
                    else
                    {
                        Posts.Clear();
                        Posts = new ObservableCollection<PostModel>(PostData.AllPosts);
                    }
                }
            }
        }

        public Visibility DeleteVisibility
        {
            get => _deleteVisibility;
            set => SetProperty(ref _deleteVisibility, value);
        }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set => SetProperty(ref _isPopupOpen, value);
        }

        public ObservableCollection<PostModel> Posts
        {
            get => _posts;
            set
            {
                SetProperty(ref _posts, value);
                if (Posts.Count > 0)
                    DeleteVisibility = Visibility.Visible;
            }
        }

        public int PModel
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }
        public PostViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            OpenPopupCommand = new DelegateCommand(OpenPopup);
            DeleteItemCommand = new DelegateCommand(DeleteItem);
            SelectImageCommand = new DelegateCommand(SelectImage);
            StatisticsCommand = new DelegateCommand(Statistics);
            Posts = new ObservableCollection<PostModel>(PostData.AllPosts);
        }

        private void Statistics()
        {
            if (PostData.AllPosts.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Sheet1");
                IRow row = sheet.CreateRow(0);
                IRow row1 = sheet.CreateRow(1);
                for (int i = 0; i < PostData.AllPosts.Count; i++) 
                {
                    row.CreateCell(0).SetCellValue("Post Id");
                    row.CreateCell(i+1).SetCellValue(PostData.AllPosts[i].Id);
                    row1.CreateCell(0).SetCellValue("Like count");
                    row1.CreateCell(i + 1).SetCellValue(PostData.AllPosts[i].Counter);
                }
                using (FileStream fileStream = new FileStream($"{Environment.CurrentDirectory}/Statistics.xlsx", FileMode.Create))
                {
                    workbook.Write(fileStream, false);
                }
            }
        }

        private void SelectImage()
        {
            IsPopupOpen = false;
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                _imagePath = dialog.FileName;
            }
            if (_imagePath != null && _imagePath != "")
            {
                var postId = PostData.AllPosts.Count + 1;
                PostData.AllPosts.Add(new PostModel
                {
                    Id = postId,
                    Name = "",
                    Description = "",
                    ImagePath = new List<string> { _imagePath },
                    LikeCommand = new DelegateCommand<object>(Like),
                    EditCommand = new DelegateCommand<object>(Edit)
                });
            }
            Posts = new ObservableCollection<PostModel>(PostData.AllPosts);
        }

        private void Edit(object id)
        {
            var p = new NavigationParameters();
            p.Add("PostId", $"{id}");
            _regionManager.RequestNavigate("ContentRegion", "AddPostView", p);
        }

        private void Like(object id)
        {
            PostData.AllPosts[(int)id - 1].Counter++;
        }

        private void DeleteItem()
        {
            if (Posts.Count > 0 && PModel >= 0)
            {
                PostData.AllPosts.RemoveAt(PModel);
                Posts.RemoveAt(PModel);
            }
            if(PostData.AllPosts.Count == 0)
                DeleteVisibility = Visibility.Collapsed;
        }

        private void OpenPopup()
        {
            IsPopupOpen = _isPopupOpen == true ? false : true;
        }

        private void Navigate(string obj)
        {
            _regionManager.RequestNavigate("ContentRegion", obj);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Posts = new ObservableCollection<PostModel>(PostData.AllPosts);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
