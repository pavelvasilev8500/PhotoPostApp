using DataModule;
using DataModule.Models;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;

namespace ModulePost.ViewModels
{
    class AddPostViewModel : BindableBase, IRegionMemberLifetime, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private int _postId;
        private string _postName;
        private string _postdescription;
        private string _photoPath;

        private ObservableCollection<string> _photoPathList = new ObservableCollection<string>();
        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand<string> AddPostCommand {  get; private set; }
        public DelegateCommand SelectPhotoCommand { get; private set; }
        public DelegateCommand DeletePhotoCommand { get; private set; }
        public bool KeepAlive => false;

        public ObservableCollection<string> PhotoPathList
        {
            get => _photoPathList;
            set => SetProperty(ref _photoPathList, value);
        }

        public string PostName
        {
            get => _postName;
            set => SetProperty(ref _postName, value);
        }

        public string PostDescription
        {
            get => _postdescription; 
            set => SetProperty(ref _postdescription, value); 
        }


        public AddPostViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            SelectPhotoCommand = new DelegateCommand(SelectPhoto);
            AddPostCommand = new DelegateCommand<string>(NewPost);
        }

        private void SelectPhoto()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                _photoPath = dialog.FileName;
            }
            PhotoPathList.Add(_photoPath);
        }

        private void NewPost(string obj)
        {
            if(_postId > 0)
            {
                PostData.AllPosts[_postId - 1].Name = PostName;
                PostData.AllPosts[_postId - 1].Description = PostDescription;
                PostData.AllPosts[_postId - 1].ImagePath = PhotoPathList.ToList();
            }
            else
            {
                var postId = PostData.AllPosts.Count + 1;
                PostData.AllPosts.Add(new PostModel()
                {
                    Id = postId,
                    Name = PostName,
                    Description = PostDescription,
                    ImagePath = PhotoPathList.ToList(),
                    Counter = 0,
                    LikeCommand = new DelegateCommand<object>(Like),
                    EditCommand = new DelegateCommand<object>(Edit)
                });
            }
            _regionManager.RequestNavigate("ContentRegion", obj);
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

        private void Navigate(string obj)
        {
            _regionManager.RequestNavigate("ContentRegion", obj);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.GetValue<string>("PostId") != "" && navigationContext.Parameters.GetValue<string>("PostId") != null)
            {
                _postId = int.Parse(navigationContext.Parameters.GetValue<string>("PostId"));
                PostName = PostData.AllPosts[_postId - 1].Name;
                PostDescription = PostData.AllPosts[_postId - 1].Description;
                PhotoPathList = new ObservableCollection<string>(PostData.AllPosts[_postId - 1].ImagePath);
            }
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
