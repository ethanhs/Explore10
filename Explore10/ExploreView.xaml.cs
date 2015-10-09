using System;
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
using System.ComponentModel;
using System.IO;
using System.Drawing;

namespace Explore10
{
    /// <summary>
    /// Interaction logic for ExploreView.xaml
    /// </summary>

    public partial class ExploreView : UserControl
    {
        
        public int width = 48;

        public ExploreView()
        {
            DynamicWidth = 48;
            InitializeComponent();
            
        }
        public int DynamicWidth 
        { 
            get 
            {
                return width;
            }
            set 
            {
                if (value != width) { 
                width = value;
                
                }
            } 
        }
        public void FillView(string location)
        {
            foreach (var file in Directory.GetFiles(location)) {
                int THUMB_SIZE = 256;
                Bitmap thumbnail = ThumbnailGenerator.WindowsThumbnailProvider.GetThumbnail(
                   file, THUMB_SIZE, THUMB_SIZE, 0x00);
                ExploreItem item = new ExploreItem();
                item.ImgSource= thumbnail;
                thumbnail.Dispose();
                FileView.Children.Add(item);
            }
        }
        
        private void StarMenu(object sender, RoutedEventArgs e)
        {
            if (LeftSide.Width != 200) { LeftSide.Width = 200; }
            else { LeftSide.Width = 48; }
            
        }
        private void OpenOneDrive(object sender, RoutedEventArgs e)
        {
            if (LeftSide.Width != 200) { LeftSide.Width = 200; }
            else { LeftSide.Width = 48; }
        }
        private void GoHome(object sender, RoutedEventArgs e)
        {
            if (LeftSide.Width != 200) { LeftSide.Width = 200; }
            else { LeftSide.Width = 48; }
        }
    }

}
