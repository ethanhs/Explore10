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
            List<FileItem> _files = new List<FileItem>();
            InitializeComponent();
            
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
            //clear for next view
            try { FilesView.Items.Clear(); }
            catch { Debug.WriteLine("Unable to clear view"); }
            AddressBar.Text = location;
            DirectoryInfo dirinfo = new DirectoryInfo(location);
            FileInfo[] files = dirinfo.GetFiles();
            
            foreach (FileInfo file in files)
            {
                string FileName = file.Name;

                string FullName = file.FullName;
                if (!FullName.Equals(null))
                {
                    FileItem item = new FileItem();
                    item.Width = 280;
                    item.Height = 280;
                    item.Padding= new Thickness(8);
                    item.Margin = new Thickness(8);
                    item.Background = System.Windows.Media.Brushes.Transparent;
                    item.BorderBrush = System.Windows.Media.Brushes.Transparent;
                    item.FillItem(FullName, FileName);
                    
                    FilesView.Items.Add(item);
                }
                
                
                

            }
            this.DataContext = this;
            

            
        }
        

        
            
        
        
    }
        

}
