using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using GongSolutions.Shell;

namespace Explore10
{
    /// <summary>
    /// Interaction logic for ExploreView.xaml
    /// </summary>
        public partial class ExploreView : System.Windows.Controls.UserControl
    {

        public string CurrDir;
        public string PrevDir;
        public string NextDir;
        public ExploreView()
        {
            InitializeComponent();
            
        }
        

        public void FillView(string location) 
        {
            PrevDir = CurrDir;
            CurrDir = location;
            //clear for next view
            try { FilesView.Items.Clear(); }
            catch { Debug.WriteLine("Unable to clear view"); }
            AddressBar.Text = location;
            var dirinfo = new DirectoryInfo(location);
            var files = dirinfo.GetFiles();
            var dirs = dirinfo.GetDirectories();
            foreach (var file in files)
            {
                var fileName = file.Name;

                var fullName = file.FullName;
                var item = new FileItem
                {
                    Width = 160,
                    Height = 160,
                    Padding = new Thickness(8),
                    Margin = new Thickness(8),
                    Background = System.Windows.Media.Brushes.Transparent,
                    BorderBrush = System.Windows.Media.Brushes.Transparent
                };
                item.MouseDoubleClick += openFile;
                item.MouseRightButtonDown += Item_MouseRightButtonDown;
                item.FillItem(fullName, fileName);
                FilesView.Items.Add(item);
            }
            foreach (var folder in dirs)
            {
                var folderName = folder.Name;

                var folderFullName = folder.FullName;

                var item = new FileItem
                {
                    Width = 160,
                    Height = 160,
                    Padding = new Thickness(8),
                    Margin = new Thickness(8),
                    Background = System.Windows.Media.Brushes.Transparent,
                    BorderBrush = System.Windows.Media.Brushes.Transparent
                };
                item.MouseRightButtonDown += Item_MouseRightButtonDown;
                item.FillItem(folderFullName, folderName);
                item.MouseDoubleClick += folder_MouseDoubleClick;

                FilesView.Items.Add(item);

            }
            DataContext = this;
        }

        private void Item_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (SystemParameters.SwapButtons) //right click
            //{
            FileItem item = (FileItem)sender;
            ShellItem myFile = new ShellItem(new Uri(item.Filepath));
            ShellContextMenu ctxmenu = new ShellContextMenu(myFile);
            ctxmenu.ShowContextMenu(null, Helpers.GetMousePosition());
            Debug.WriteLine("no context menu?");
            //}
        }

        private void folder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FileItem item = (FileItem)sender;
            PrevDir = item.Filepath;
            FillView(item.Filepath);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e) => FillView(CurrDir);

        private void Up_Click(object sender, RoutedEventArgs e)
        {

            var paths=CurrDir.Split(Convert.ToChar("\\"));
            var newPath=paths.Take(paths.Count() - 1).Aggregate((s1, s2) => s1 + "\\" + s2)+"\\";
            FillView(newPath);

        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(NextDir);
            if (string.IsNullOrEmpty(NextDir)) return;
            try { FillView(NextDir); }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NextDir = CurrDir;
            Debug.WriteLine(PrevDir);
            if (string.IsNullOrEmpty(PrevDir)) return;
            try { FillView(PrevDir); }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            
            if (AddressBar.Text == "") return;
            PrevDir = CurrDir;
            var path= AddressBar.Text;
            if (Directory.Exists(path))
            {
                FillView(path);
            }
            else if (File.Exists(path))
            {
                Process.Start(path);

            }
        }
        private void openFile(object sender, RoutedEventArgs e)
        {
            FileItem item = (FileItem)sender;
            Process.Start(item.Filepath);
        }

        private void AddressBar_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (AddressBar.Text == "") return;
                PrevDir = CurrDir;
                var path = AddressBar.Text;
                if (Directory.Exists(path))
                {
                    FillView(path);
                }
                else if (File.Exists(path))
                {
                    Process.Start(path);

                }
                else
                {
                    MessageBox.Show("The path entered does not exist", "Explore10"); //TODO replace with metro messagebox
                }
            }
        }
    }
       

}
