using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Explore10
{

    /// <summary>
    /// Interaction logic for FileItem.xaml
    /// </summary>
    public partial class FileItem : Button
    {
    
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        public string _filepath;
        public BitmapImage _fileimg;
        public FileItem()
        {
            this.DataContext = this;
        }
        public void FillItem(string path,string _name) {
            _filepath = path;
            StackPanel ImageStack = new StackPanel();
            ImageStack.Background = System.Windows.Media.Brushes.Transparent;
            ImageStack.Orientation = Orientation.Vertical;
            System.Windows.Controls.Image FileImage = new System.Windows.Controls.Image();

            //set the source!!!
            FileImage.Source = SmartThumnailProvider.GetThumbInt(path, 128, 128,ThumbOptions.BiggerOk);



            TextBlock FileText = new TextBlock();
            FileText.FontSize = 18;
            FileText.TextTrimming = TextTrimming.CharacterEllipsis;
            FileText.Text = _name;
            FileText.TextAlignment = TextAlignment.Center;
            FileText.VerticalAlignment = VerticalAlignment.Bottom;
            FileText.Height = 30;
            FileImage.Height = 100;
            FileImage.Width = 100;
            ImageStack.Children.Add(FileImage);
            ImageStack.Children.Add(FileText);
            
            this.Content = ImageStack;

        }
        
    }
}
