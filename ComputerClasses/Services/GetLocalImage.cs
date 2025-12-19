using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerClasses.Services
{
    public class GetLocalImage 
    {
        private string _imagePath = @"pack://application:,,,/Resources/Assets/Images/";

        public BitmapImage GetImage(string imageName)
        {
            return new BitmapImage(new Uri(Path.Combine(_imagePath, imageName)));
        }
    }
}
