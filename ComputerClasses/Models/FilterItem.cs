using CommunityToolkit.Mvvm.ComponentModel;

namespace ComputerClasses.Models
{
    public partial class FilterItem : ObservableObject
    {
        [ObservableProperty]
        private string value;
        [ObservableProperty]
        private string propName;
        public FilterItem(string value, string propName)
        {
            Value = value;
            PropName = propName;
        }
    }
}
