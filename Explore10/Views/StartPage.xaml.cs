using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            foreach (var di in System.IO.DriveInfo.GetDrives())
            {
                if (!di.IsReady) continue;
                var hPanel = new StackPanel {Orientation = Orientation.Horizontal};
                var vPanel = new StackPanel {Orientation = Orientation.Vertical};
                var label = new TextBlock
                {
                    Text = di.VolumeLabel,
                    FontSize = 22
                };
                var name = new TextBlock
                {
                    Text = di.Name,
                    FontSize = 40,
                    Margin = new Thickness {Right = 10},
                    Name = "Path"
                };
                hPanel.Children.Add(name);
                var space = new TextBlock
                {
                    Text = $"{Helpers.PrettyByte(di.AvailableFreeSpace)} free of {Helpers.PrettyByte(di.TotalSize)}"
                };
                var driveFilled = new ProgressBar
                {
                    Minimum = 0,
                    Maximum = di.TotalSize,
                    Value = (di.TotalSize - di.AvailableFreeSpace),
                    Height = 8,
                    Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x30, 0x91, 0xDD))
                };
                var convertFromString = ColorConverter.ConvertFromString("White");
                if (convertFromString != null)
                    driveFilled.Background = new SolidColorBrush((Color) convertFromString);
                        //it really shouldn't be this hard
                vPanel.Children.Add(label);
                vPanel.Children.Add(driveFilled);
                vPanel.Children.Add(space);
                hPanel.Children.Add(vPanel);
                hPanel.Width = 200;
                hPanel.Height = 50;
                hPanel.AddHandler(StackPanel.MouseDownEvent, new MouseButtonEventHandler(OpenDrive));
                hPanel.Margin = new Thickness(10);
                Drives.Items.Add(hPanel);
            }
        }

        private void OpenDrive(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount < 2) return;
            StackPanel panel = (StackPanel) sender;
            string path = "";
            foreach (
                var textBlock in
                    panel.Children.OfType<FrameworkElement>().Where(child => child.Name == "Path").OfType<TextBlock>())
            {
                path = textBlock.Text;
            }
            var view = new ExploreView();
            view.FillView(path);
            var parentWindow = (MainWindow) Window.GetWindow(this);
            if (parentWindow == null) return;
            var tab = (TabItem) parentWindow.tabDynamic.SelectedItem;
            tab.Header = path; // because it won't be too long. I hope...
            tab.Content = view;
        }

    }
}
