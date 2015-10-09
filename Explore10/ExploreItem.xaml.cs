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
using System.Drawing;
namespace Explore10
{
    /// <summary>
    /// Interaction logic for ExploreItem.xaml
    /// </summary>
    public partial class ExploreItem : UserControl
    {
        public Bitmap ImgSource = new Bitmap(@"pack://application:,,,/Explore10;component/Images/null.png");
        public ExploreItem()
        {
            InitializeComponent();
        }
        public Bitmap ImageSource
        {
            get
            {
                return ImgSource;
            }
            set
            {
                if (value!=null)
                {
                    ImgSource = value;
                }
            }
        }
    }
}
