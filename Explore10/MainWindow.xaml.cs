using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using MahApps.Metro.Controls;

namespace Explore10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly List<TabItem> _tabItems;
        private readonly TabItem _tabAdd;
        private int tabcount;
        public MainWindow()
        {

            InitializeComponent();

            // initialize tabItem array
            _tabItems = new List<TabItem>();

            // add a tabItem with + in header 
            _tabAdd = new TabItem
            {
                Style = new Style(),
                Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x30, 0x30, 0x30)),
                BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x30, 0x30, 0x30))
            };
            //get image for header and setup add button
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(@"pack://application:,,,/Explore10;component/Images/appbar.add.png");
            bitmap.EndInit();
            var plus = new Image
            {
                Source = bitmap,
                Width = 25,
                Height = 25
            };
            _tabAdd.Width = 35;
            _tabAdd.Header = plus;
            _tabAdd.MouseLeftButtonUp += tabAdd_MouseLeftButtonUp;
            _tabItems.Add(_tabAdd);
            tabcount += 1;
            // add first tab
            this.AddTabItem();

            // bind tab control
            tabDynamic.DataContext = _tabItems;

            tabDynamic.SelectedIndex = 0;


        }


        private TabItem AddTabItem()
        {
            int count = tabcount;
            Debug.WriteLine(count);
            // create new tab item
            var tab = new TabItem
            {
                Header = string.Format("C:\\", count),
                Name = $"tab{count}",
                HeaderTemplate = tabDynamic.FindResource("TabHeader") as DataTemplate
            };



            ExploreView view = new ExploreView {Name = "Explore"};
            view.FillView("C:\\");
            tab.Content = view;

            // insert tab item right before the last (+) tab item
            _tabItems.Insert(_tabItems.Count-1, tab);
            tabcount += 1;
            return tab;
        }

        private void tabAdd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //remove binding
            tabDynamic.DataContext = null;
            var tab = this.AddTabItem();
            // bind tab
            tabDynamic.DataContext = _tabItems;
            // select new tab
            tabDynamic.SelectedItem = tab;
        }

        private void Flyout(object sender, RoutedEventArgs e)
        {

            Places.IsOpen = true;
            
        }

        private void tabDynamic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tab = tabDynamic.SelectedItem as TabItem;
            if (tab == null) return;

            if (tab.Equals(_tabAdd) && tabDynamic.Items.Count!=1)
            {
                //go to the first tab if we are at the end
                tabDynamic.SelectedItem = tabDynamic.Items.GetItemAt(0);
            }
            
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            string tabName = button.CommandParameter.ToString();

            var item = tabDynamic.Items.Cast<TabItem>().SingleOrDefault(i => i.Name.Equals(tabName));

            var tab = item;

            if (tab == null) return;
            if (_tabItems.Count < 3)
            {
                Close();
            }
            else
            {
                // get selected tab
                TabItem selectedTab = tabDynamic.SelectedItem as TabItem;

                // clear tab control binding
                tabDynamic.DataContext = null;

                //get the index for latter
                var index = _tabItems.IndexOf(selectedTab);
                _tabItems.Remove(tab);

                // bind tab control
                tabDynamic.DataContext = _tabItems;

                // select previously selected tab. if that is removed then select first tab
                if (selectedTab == null || selectedTab.Equals(tab))
                {
                    try
                    {
                        selectedTab = _tabItems[index + 1]; //go to the right?
                    }
                    catch
                    {
                        selectedTab = _tabItems[0];
                    }
                }
                tabDynamic.SelectedItem = selectedTab;
            }
        }

        private void Close(object sender, RoutedEventArgs e) => Close();

        private void NewWindow(object sender, RoutedEventArgs e)
        {
            var info = new ProcessStartInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Process.Start(info);
        }
        private void Cmd(object sender, RoutedEventArgs e)
        {
            var cmd = new ProcessStartInfo {FileName = "cmd"};
            var view = (ExploreView)tabDynamic.SelectedContent;
            cmd.Arguments = "/k cd " + view.CurrDir;
            //If you want to make it open an admin command prompt, uncomment:
            //cmd.Verb = "runas";
            Process p = Process.Start(cmd);
        }

        private void PowerShell(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo powerShell = new ProcessStartInfo {FileName = "powershell"};
            try
            {
                ExploreView view = (ExploreView)tabDynamic.SelectedContent;
                powerShell.Arguments = "-NoExit -Command cd " + view.CurrDir;
            }
            catch //if we are in this pc view, the above will fail.
            {
                powerShell.Arguments = "-NoExit -Command cd "+ Environment.SpecialFolder.UserProfile;
            }
            
            //If you want to make it open an admin command prompt, uncomment:
            //cmd.Verb = "runas";
            Process P = Process.Start(powerShell);
        }


        private void StarMenu(object sender, RoutedEventArgs e)
        {
            if (Star.Height != 100)
            {
                OneDrive.Height = 40;
                Home.Height = 40;
                Star.Height = 100;
                Quick.Height = 60;
                OneDrive.Margin = new Thickness { Right = 0, Top = 115, Bottom = 0, Left = 3 };
                Home.Margin = new Thickness { Right = 0, Top = 162, Bottom = 0, Left = 3 };
            }
            else
            {
                Star.Height = 40;
                Quick.Height = 0;
                OneDrive.Margin = new Thickness { Right = 0, Top = 55, Bottom = 0, Left = 3 };
                Home.Margin = new Thickness { Right = 0, Top = 100, Bottom = 0, Left = 3 };
            }
        }
        private void OpenOneDrive(object sender, RoutedEventArgs e)
        {
            if (OneDrive.Height != 100)
            {
                OneDrive.Height = 100;
                Home.Height = 40;
                Star.Height = 40;
                OneDrive.Margin = new Thickness { Right = 0, Top = 55, Bottom = 0, Left = 3 };
                Home.Margin = new Thickness { Right = 0, Top = 162, Bottom = 0, Left = 3 };
            }
            else
            {
                OneDrive.Height = 40;
                OneDrive.Margin = new Thickness { Right = 0, Top = 55, Bottom = 0, Left = 3 };
                Home.Margin = new Thickness { Right = 0, Top = 100, Bottom = 0, Left = 3 };
            }
        }
        private void GoHome(object sender, RoutedEventArgs e)
        {
            Places.IsOpen = false;
            StartPage page = new StartPage();
            page.FillDrives();
            TabItem tab = (TabItem) tabDynamic.SelectedItem;
            tab.Header = "This PC";
            tab.Content = page;
            
            
        }

    }
}
