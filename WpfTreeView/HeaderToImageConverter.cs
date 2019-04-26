using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfTreeView
{
    /// <summary>
    /// Converts a full path to a specific image
    /// </summary>
    /// 
    [ValueConversion(typeof(string), typeof(BitmapImage))]

    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var path = (string)value;

            if (path == null)
                return null;

            var name = MainWindow.GetFileFolderName(path);

            var image = "file.png";

            // drives will set name = ""
            if (string.IsNullOrEmpty(name))
            {
                image = "drive.png";
            }
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
            {
                image = "folder.png";
            }

            return new BitmapImage(new Uri(string.Format("pack://application:,,,/images/{0}", image)));
       }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
