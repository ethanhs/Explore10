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
using System.Diagnostics;

namespace Explore10
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : UserControl
    {
        
        public StartPage()
        {
            InitializeComponent();
            
        }
        public void FillDrives()
        {
            foreach (System.IO.DriveInfo di in System.IO.DriveInfo.GetDrives())
            {
                if (di.IsReady)
                {
                    StackPanel hPanel = new StackPanel();
                    hPanel.Orientation = Orientation.Horizontal;
                    StackPanel vPanel = new StackPanel();
                    vPanel.Orientation = Orientation.Vertical;
                    TextBlock Label = new TextBlock();
                    Label.Text = di.VolumeLabel;
                    Label.FontSize = 22; //quick hack TODO: add images?
                    TextBlock Name = new TextBlock();
                    Name.Text = di.Name;
                    Name.FontSize = 40;
                    Name.Width = 60;
                    Name.Margin = new Thickness { Right = 10 };
                    Name.Name = "Path";
                    hPanel.Children.Add(Name);
                    TextBlock Space = new TextBlock();
                    Space.Text = string.Format("{0} free of {1}", PrettyByte(di.AvailableFreeSpace), PrettyByte(di.TotalSize));
                    ProgressBar DriveFilled = new ProgressBar();
                    DriveFilled.Minimum = 0;
                    DriveFilled.Maximum = di.TotalSize;
                    DriveFilled.Value = (di.TotalSize - di.AvailableFreeSpace);
                    DriveFilled.Height = 8;
                    DriveFilled.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x30, 0x91, 0xDD));
                    DriveFilled.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White")); //it really shouldn't be this hard
                    vPanel.Children.Add(Label);
                    vPanel.Children.Add(DriveFilled);
                    vPanel.Children.Add(Space);
                    hPanel.Children.Add(vPanel);
                    hPanel.Width = 200;
                    hPanel.Height = 50;
                    hPanel.AddHandler(StackPanel.MouseDownEvent, new MouseButtonEventHandler(OpenDrive));
                    hPanel.Margin = new Thickness(10);
                    Drives.Items.Add(hPanel);
                }
            }
        }
        private void OpenDrive(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                StackPanel panel = (StackPanel)sender;
                string path = "";
                foreach (object child in panel.Children)
                {
                    if (child is FrameworkElement)
                    {
                        if ((child as FrameworkElement).Name == "Path")
                        {
                            path = (child as TextBlock).Text;
                        }
                    }
                }
                ExploreView view = new ExploreView();
                view.FillView(path);
                MainWindow parentWindow = (MainWindow)Window.GetWindow(this);
                TabItem tab = (TabItem)parentWindow.tabDynamic.SelectedItem;
                tab.Header = path; // because it won't be too long. I hope...
                tab.Content = view;
            }
        }
        
        static readonly string[] suffixes =
        { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            static string PrettyByte(long value)
           {
                if (value == 0) { return "0.0 bytes"; }

                int mag = (int)Math.Log(value, 1024);
                decimal adjustedSize = (decimal)value / (1L << (mag * 10));

                return string.Format("{0:n1} {1}", adjustedSize, suffixes[mag]);
            }
        }
}
