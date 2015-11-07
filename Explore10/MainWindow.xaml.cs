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
        private List<TabItem> _tabItems;
        private TabItem _tabAdd;
        public MainWindow()
        {
            
                InitializeComponent();

                // initialize tabItem array
                _tabItems = new List<TabItem>();
                
                // add a tabItem with + in header 
                _tabAdd = new TabItem();
                //get image for header and setup add button
                _tabAdd.Style = new Style();
                _tabAdd.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x30, 0x30, 0x30));
                _tabAdd.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x30, 0x30, 0x30));
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(@"pack://application:,,,/Explore10;component/Images/appbar.add.png");
                bitmap.EndInit();
                Image plus = new Image();
                plus.Source = bitmap;
                plus.Width = 25;
                plus.Height = 25;
                _tabAdd.Width = 35;
                _tabAdd.Header = plus;
                _tabAdd.MouseLeftButtonUp += new MouseButtonEventHandler(tabAdd_MouseLeftButtonUp);
                _tabItems.Add(_tabAdd);
                
                // add first tab
                //this.AddTabItem();
                
                // bind tab control
                tabDynamic.DataContext = _tabItems;

                tabDynamic.SelectedIndex = 0;
                
            
        }
        

        private TabItem AddTabItem()
        {
            int count = _tabItems.Count;
            // create new tab item
            TabItem tab = new TabItem();
            tab.Header = string.Format("C:\\", count);
            tab.Name = string.Format("tab{0}", count);
            tab.HeaderTemplate = tabDynamic.FindResource("TabHeader") as DataTemplate;



            ExploreView view = new ExploreView();
            view.Name = "Explore";
            view.FillView("C:\\");
            tab.Content = view;

            // insert tab item right before the last (+) tab item
            _tabItems.Insert(count-1, tab);
            return tab;
        }

        private void tabAdd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //remove binding
            tabDynamic.DataContext = null;
            TabItem tab = this.AddTabItem();
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
            TabItem tab = tabDynamic.SelectedItem as TabItem;
            if (tab == null) return;

            if (tab.Equals(_tabAdd))
            {
                // clear binding
                tabDynamic.DataContext = null;

                TabItem newTab = this.AddTabItem();

                // bind tab
                tabDynamic.DataContext = _tabItems;

                // select new tab
                tabDynamic.SelectedItem = newTab;
            }
            else
            {
                //....
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string tabName = (sender as Button).CommandParameter.ToString();

            var item = tabDynamic.Items.Cast<TabItem>().Where(i => i.Name.Equals(tabName)).SingleOrDefault();

            TabItem tab = item as TabItem;

            if (tab != null)
            {
                if (_tabItems.Count < 3)
                {
                    this.Close();
                }
                else
                {
                    // get selected tab
                    TabItem selectedTab = tabDynamic.SelectedItem as TabItem;

                    // clear tab control binding
                    tabDynamic.DataContext = null;

                    _tabItems.Remove(tab);

                    // bind tab control
                    tabDynamic.DataContext = _tabItems;

                    // select previously selected tab. if that is removed then select first tab
                    if (selectedTab == null || selectedTab.Equals(tab))
                    {
                        selectedTab = _tabItems[0];
                    }
                    tabDynamic.SelectedItem = selectedTab;
                }
            }
        }
        
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NewWindow(object sender, RoutedEventArgs e)
        {
            var info = new System.Diagnostics.ProcessStartInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            System.Diagnostics.Process.Start(info);
        }
        private void CMD(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo cmd = new ProcessStartInfo();
            cmd.FileName = "cmd";
            ExploreView view = (ExploreView)tabDynamic.SelectedContent;
            cmd.Arguments = "/k cd " + view.currDir;
            //If you want to make it open an admin command prompt, uncomment:
            //cmd.Verb = "runas";
            Process P = Process.Start(cmd);
        }

        private void PowerShell(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo PowerShell = new ProcessStartInfo();
            PowerShell.FileName = "powershell";
            ExploreView view = (ExploreView)tabDynamic.SelectedContent;
            PowerShell.Arguments = "-NoExit -Command cd " + view.currDir;
            //If you want to make it open an admin command prompt, uncomment:
            //cmd.Verb = "runas";
            Process P = Process.Start(PowerShell);
        }


        private void StarMenu(object sender, RoutedEventArgs e)
        {
            if (!(Star.Height == 100))
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
            if (!(OneDrive.Height == 100))
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
