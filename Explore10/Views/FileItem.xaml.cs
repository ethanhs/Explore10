using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;


namespace Explore10
{

    /// <summary>
    /// Interaction logic for FileItem.xaml
    /// </summary>
    public partial class FileItem
    {
    
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        public string Filepath;
        public BitmapImage Fileimg;
        public FileItem()
        {
            DataContext = this;
        }
        public void FillItem(string path,string name) {
            Filepath = path;
            var imageStack = new StackPanel
            {
                Background = System.Windows.Media.Brushes.Transparent,
                Orientation = Orientation.Vertical
            };

            var fileImage = new Image
            {
                Source = SmartThumnailProvider.GetThumbInt(path, 128, 128, ThumbOptions.BiggerOk)
            };

            var fileText = new TextBlock
            {
                FontSize = 18,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Text = name,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 30
            };
            fileImage.Height = 100;
            fileImage.Width = 100;
            imageStack.Children.Add(fileImage);
            imageStack.Children.Add(fileText);
            
            Content = imageStack;

        }

        
    }
}
