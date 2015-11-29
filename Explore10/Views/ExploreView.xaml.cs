using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;
using GongSolutions.Shell;
using IWshRuntimeLibrary;
using File = System.IO.File;

namespace Explore10
{
    /// <summary>
    /// Interaction logic for ExploreView.xaml
    /// </summary>
        public partial class ExploreView : UserControl
    {

        public string CurrDir;
        public string PrevDir;
        public string NextDir;
        private List<string> Selected = new List<string>();
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
                item.Click += Item_Click;
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
                item.Click += Item_Click;
                FilesView.Items.Add(item);

            }
            DataContext = this;
        }

        private void Item_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            FileItem item = (FileItem)sender;
            ShellItem myFile = new ShellItem(new Uri(item.Filepath));
            ShellContextMenu ctxmenu = new ShellContextMenu(myFile);
            ctxmenu.ShowContextMenu(null, Helpers.GetMousePosition());
            
            
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
            if (item.IsLink())
            {
                try
                {
                    
                    WshShell shell = new WshShell();
                    IWshShortcut link =
                        (IWshShortcut) shell.CreateShortcut(item.Filepath);
                    FillView(link.TargetPath);

                }
                catch
                {
                    MessageBox.Show("That link seems broken, if it works in Explorer, please file a bug");
                }


            }
            else
            {
                Process.Start(item.Filepath);
            }
            
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
        private void Item_Click(object sender, RoutedEventArgs e)
        {
            FileItem item = (FileItem) sender;
            //add item to the selected if not already, else remove
            if (!Selected.Contains(item.Filepath))
            {
                Selected.Add(item.Filepath);
                item.Background = new SolidColorBrush(Color.FromRgb(0x1D,0x99,0xCC));
            }
            else
            {
                Selected.Remove(item.Filepath);
                item.Background = Brushes.Transparent;
            }


        }

        private void ExploreView_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {

                //copy selected to clipboard
                Clipboard.Clear();
                Clipboard.SetData(DataFormats.FileDrop, Selected.ToArray());
                Helpers.Cut = false;

            }
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                //copy selected to clipboard
                var files = (string[]) Clipboard.GetData(DataFormats.FileDrop);
                foreach (var file in files)
                {
                    if (Helpers.Cut)
                    {
                        //if cut, move it!
                        try
                        {
                            File.Move(file, CurrDir + Path.GetFileName(file));
                        }
                        catch (System.UnauthorizedAccessException)
                        {
                            MessageBox.Show("Need administrator rights to move that file", "Explore10");
                        }
                        
                    }
                    else
                    {
                        try
                        {
                            File.Copy(file, CurrDir + Path.GetFileName(file));
                        }
                        catch (System.UnauthorizedAccessException)
                        {
                            MessageBox.Show("Need administrator rights to copy that file", "Explore10");
                        }
                        
                    }
                }
                FillView(CurrDir); //refresh view to reflect changes

            }
            else if (e.Key == Key.X && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                //cut selected to clipboard
                Clipboard.Clear();
                Clipboard.SetData(DataFormats.FileDrop, Selected.ToArray());
                Helpers.Cut = true;
            }
        }
    }
       

}
