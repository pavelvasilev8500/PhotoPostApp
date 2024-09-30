using ModulePost.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ModulePost
{
    public class ModulePostModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModulePostModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegion region = _regionManager.Regions["ContentRegion"];
            var postView = containerProvider.Resolve<PostView>();
            region.Add(postView);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PostView>();
            containerRegistry.RegisterForNavigation<AddPostView>();
        }
    }
}