using Prism.Commands;

namespace DataModule.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string>? ImagePath { get; set; }
        public int Counter { get; set; }
        public DelegateCommand<object> LikeCommand { get; set; }
        public DelegateCommand<object> EditCommand { get; set; }
    }
}
