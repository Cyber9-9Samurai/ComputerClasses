using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;

namespace ComputerClasses.Services
{
    public partial class WorkFileService : ObservableObject
    {
        private FileInfo _currentFile;
        public WorkFileService() { }

        public FileInfo GetCurrentWorkFile()
        {
            return _currentFile;
        }

        public void SetCurrentWorkFile(FileInfo file)
        {
            _currentFile = file;
        }


    }
}
