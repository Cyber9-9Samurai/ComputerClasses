using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ComputerClasses.Domain.Import
{
    public sealed record ImportError(int RowNumber, string ColumnName, string Message);

    public sealed partial class ImportResult<T> : ObservableObject
    {
        [ObservableProperty]
        private List<T> items = new();
        public List<ImportError> Errors { get; } = [];
        public bool HasErrors => Errors.Count > 0;
    }
}
