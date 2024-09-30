using Prism.Commands;
using Prism.Mvvm;
using System;

namespace PhotoPostApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand Close {  get; private set; }

        public MainWindowViewModel()
        {
            Close = new DelegateCommand(() => 
            {
                Environment.Exit(0);
            });
        }
    }
}
