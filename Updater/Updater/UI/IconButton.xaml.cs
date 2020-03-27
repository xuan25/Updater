using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Updater.UI
{
    /// <summary>
    /// Interaction logic for IconButton.xaml
    /// </summary>
    public partial class IconButton : UserControl
    {
        public delegate void ClickedHendler(object sender, MouseButtonEventArgs e);
        public event ClickedHendler Clicked;

        public Geometry IconGeometry
        {
            get
            {
                return (Geometry)GetValue(IconGeometryProperty);
            }
            set
            {
                SetValue(IconGeometryProperty, value);
            }
        }
        public static readonly DependencyProperty IconGeometryProperty = DependencyProperty.Register("IconGeometry", typeof(Geometry), typeof(IconButton), new FrameworkPropertyMetadata(null));

        public new Color Background
        {
            get
            {
                return (Color)GetValue(BackgroundProperty);
            }
            set
            {
                SetValue(BackgroundProperty, value);
            }
        }
        public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Color), typeof(IconButton), new FrameworkPropertyMetadata(Color.FromArgb(0x00, 0x00, 0x00, 0x00)));

        public new Color Foreground
        {
            get
            {
                return (Color)GetValue(ForegroundProperty);
            }
            set
            {
                SetValue(ForegroundProperty, value);
            }
        }
        public new static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Color), typeof(IconButton), new FrameworkPropertyMetadata(Color.FromArgb(0xff, 0xff, 0xff, 0xff)));

        public Color HoveredBackground
        {
            get
            {
                return (Color)GetValue(HoveredBackgroundProperty);
            }
            set
            {
                SetValue(HoveredBackgroundProperty, value);
            }
        }
        public static readonly DependencyProperty HoveredBackgroundProperty = DependencyProperty.Register("HoveredBackground", typeof(Color), typeof(IconButton), new FrameworkPropertyMetadata(Color.FromArgb(0x22, 0x00, 0x00, 0x00)));


        public Color HoveredForeground
        {
            get
            {
                return (Color)GetValue(HoveredForegroundProperty);
            }
            set
            {
                SetValue(HoveredForegroundProperty, value);
            }
        }
        public static readonly DependencyProperty HoveredForegroundProperty = DependencyProperty.Register("HoveredForeground", typeof(Color), typeof(IconButton), new FrameworkPropertyMetadata(Color.FromArgb(0xff, 0xff, 0xff, 0xff)));

        public Color DisplayedBackground
        {
            get
            {
                return (Color)GetValue(DisplayedBackgroundProperty);
            }
            private set
            {
                SetValue(DisplayedBackgroundProperty, value);
            }
        }
        public static readonly DependencyProperty DisplayedBackgroundProperty = DependencyProperty.Register("DisplayedBackground", typeof(Color), typeof(IconButton), new FrameworkPropertyMetadata(Color.FromArgb(0x00, 0x00, 0x00, 0x00)));


        public Color DisplayedForeground
        {
            get
            {
                return (Color)GetValue(DisplayedForegroundProperty);
            }
            private set
            {
                SetValue(DisplayedForegroundProperty, value);
            }
        }
        public static readonly DependencyProperty DisplayedForegroundProperty = DependencyProperty.Register("DisplayedForeground", typeof(Color), typeof(IconButton), new FrameworkPropertyMetadata(Color.FromArgb(0xff, 0xff, 0xff, 0xff)));


        public IconButton()
        {
            InitializeComponent();

            DisplayedBackground = Background;
            DisplayedForeground = Foreground;
        }

        private void UserControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.CaptureMouse();
        }

        private void UserControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.ReleaseMouseCapture();
            if (this.IsMouseOver)
                Clicked?.Invoke(this, e);
        }
    }

    public class SolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;

            SolidColorBrush solidColorBrush = new SolidColorBrush(color);
            return solidColorBrush;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
