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
using System.Diagnostics;

namespace Explore10
{
    /// <summary>
    /// Interaction logic for ExploreView.xaml
    /// </summary>
        public partial class ExploreView : UserControl
    {
        public Bitmap DefaultBitmap;
        public int width = 48;

        public ExploreView()
        {
            DynamicWidth = 48;
            List<FileItem> _files = new List<FileItem>();
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
        public Thickness ExplorerThickness
        {
            get
            {
                return new Thickness { Left = this.Width - width };
            }
        }
        public void FillView(string location)
        {
            DirectoryInfo dirinfo = new DirectoryInfo(location);
            FileInfo[] files = dirinfo.GetFiles();
            
            foreach (FileInfo file in files)
            {
                string FileName = file.Name;

                string FullName = file.FullName;
                System.Diagnostics.Debug.WriteLine(FullName);
                if (!FullName.Equals(null))
                {
                    FileItem item = new FileItem();
                    item.Width = 128;
                    item.Height = 128;
                    item.FillItem(FileName);
                    FilesView.Items.Add(item);
                }
                
                
                

            }
            this.DataContext = this;
            

            
        }
        
        private void StarMenu(object sender, RoutedEventArgs e)
        {
            if (LeftSide.Width != 200) 
            { LeftSide.Width = 200; }
            else if (LeftSide.Width == 200 && LeftSide.Height==48)
            { Star.Height = 100;
            OneDrive.Margin = new Thickness { Right = 0, Top = 160, Bottom = 0, Left = 3 };
            Home.Margin = new Thickness { Right = 0, Top = 160, Bottom = 0, Left = 3 };
            }
            else
            { LeftSide.Width = 48;  }
            
        }
        private void OpenOneDrive(object sender, RoutedEventArgs e)
        {
            if (LeftSide.Width != 200)
            { LeftSide.Width = 200; }
            else if (LeftSide.Width == 200 && LeftSide.Height != 48)
            {
                OneDrive.Height = 100;
                Star.Margin = new Thickness { Right = 0, Top = 160, Bottom = 0, Left = 3 };
                Home.Margin = new Thickness { Right = 0, Top = 160, Bottom = 0, Left = 3 };
            }
            else
            { LeftSide.Width = 48; }
        }
        private void GoHome(object sender, RoutedEventArgs e)
        {
            if (LeftSide.Width != 200)
            { LeftSide.Width = 200; }
            else if (LeftSide.Width == 200 && LeftSide.Height == 48)
            {
                Home.Height = 100;
                OneDrive.Margin = new Thickness { Right = 0, Top = 160, Bottom = 0, Left = 3 };
                Star.Margin = new Thickness { Right = 0, Top = 160, Bottom = 0, Left = 3 };
            }
            else
            { LeftSide.Width = 48; }

        }
        
            
        
        
    }
        

}
