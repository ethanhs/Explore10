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
        public static extern bool DeleteObject(IntPtr hObject);

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

            //get the hbitmap
            
            IntPtr hBitmap = WindowsThumbnailProvider.GetHBitmap(System.IO.Path.GetFullPath(path), 256, 256, ThumbnailOptions.BiggerSizeOk);
            ImageSource retval;
            try
            {
                retval = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            FileImage.Source = retval;


            TextBlock FileText = new TextBlock();
            FileText.Text = _name;
            FileText.TextAlignment = TextAlignment.Center;
            
            ImageStack.Children.Add(FileImage);
            ImageStack.Children.Add(FileText);
            this.Content = ImageStack;

        }
        
    }
}
