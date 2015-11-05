using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;


namespace Explore10
{
    /// <summary>
    /// Interaction logic for ExploreView.xaml
    /// </summary>
        public partial class ExploreView : UserControl
    {

        public string currDir;
        public string prevDir;
        public string nextDir;
        public ExploreView()
        {
            List<FileItem> _files = new List<FileItem>();
            StartPage page = new StartPage();
            InitializeComponent();
            
        }
        

        public void FillView(string location) 
        {
            prevDir = currDir;
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
                    item.Width = 160;
                    item.Height = 160;
                    item.Padding= new Thickness(8);
                    item.Margin = new Thickness(8);
                    item.Background = System.Windows.Media.Brushes.Transparent;
                    item.BorderBrush = System.Windows.Media.Brushes.Transparent;
                    item.MouseDoubleClick += openFile;
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
                    item.Width = 160;
                    item.Height = 160;
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
            prevDir = item._filepath;
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

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(nextDir);
            if (nextDir != null && nextDir != "")
            {
                try { this.FillView(nextDir); }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                
                
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            nextDir = currDir;
            Debug.WriteLine(prevDir);
            if (prevDir != null && prevDir != "")
            {
                try { this.FillView(prevDir); }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }


            }
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            prevDir = currDir;
            if (!AddressBar.Text.Equals(null) && AddressBar.Text!="")
            {
                string path= AddressBar.Text;
                if (Directory.Exists(path))
                {
                    FillView(path);
                }
                else if (File.Exists(path))
                {
                    Process.Start(path);

                }
            }
        }
        private void openFile(object sender, RoutedEventArgs e)
        {
            FileItem item = (FileItem)sender;
            Process.Start(item._filepath);
        }






    }
        

}
