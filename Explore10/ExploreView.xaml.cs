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

        public string currDir;

        public ExploreView()
        {
            List<FileItem> _files = new List<FileItem>();
            InitializeComponent();
            
        }
        

        public void FillView(string location) 
        {
            currDir = location;
            //clear for next view
            try { FilesView.Items.Clear(); }
            catch { Debug.WriteLine("Unable to clear view"); }
            AddressBar.Text = location;
            DirectoryInfo dirinfo = new DirectoryInfo(location);
            FileInfo[] files = dirinfo.GetFiles();
            DirectoryInfo[] dirs = dirinfo.GetDirectories();
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
            foreach (DirectoryInfo folder in dirs)
            {
                string FolderName = folder.Name;

                string FolderFullName = folder.FullName;
                if (!FolderFullName.Equals(null))
                {
                    FileItem item = new FileItem();
                    item.Width = 280;
                    item.Height = 280;
                    item.Padding = new Thickness(8);
                    item.Margin = new Thickness(8);
                    item.Background = System.Windows.Media.Brushes.Transparent;
                    item.BorderBrush = System.Windows.Media.Brushes.Transparent;
                    item.FillItem(FolderFullName, FolderName);
                    item.MouseDoubleClick += folder_MouseDoubleClick;

                    FilesView.Items.Add(item);
                }




            }
            this.DataContext = this;
            

            
        }

        private void folder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FileItem item = (FileItem)sender;
            this.FillView(item._filepath);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            this.FillView(currDir);
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {

            string[] paths=currDir.Split(Convert.ToChar("\\"));
            string newPath=paths.Take(paths.Count() - 1).Aggregate((s1, s2) => s1 + "\\" + s2)+"\\";
            this.FillView(newPath);


        }
        

        
            
        
        
    }
        

}
