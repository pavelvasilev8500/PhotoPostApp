using DataModule.Models;
using System.Collections.ObjectModel;

namespace DataModule
{
    public static class PostData
    {
        private static ObservableCollection<PostModel> _allPosts = new ObservableCollection<PostModel>();

        public static ObservableCollection<PostModel> AllPosts
        {
            get => _allPosts;
            set => _allPosts = value;
        }
    }
}
