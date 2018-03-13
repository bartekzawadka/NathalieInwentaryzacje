using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;

namespace NathalieInwentaryzacje.Views.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy NDropDownButton.xaml
    /// </summary>
    public partial class NDropDownButton : DropDownButton
    {
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string),
            typeof(DropDownButton));

        public static DependencyProperty IsAccentedProperty =
            DependencyProperty.Register("IsAccented", typeof(bool), typeof(DropDownButton));

        public static DependencyProperty ModernIconProperty =
            DependencyProperty.Register("ModernIcon", typeof(PackIconModernKind), typeof(DropDownButton));

        public static DependencyProperty ContentOrientationProperty =
            DependencyProperty.Register("ContentOrientation", typeof(Orientation), typeof(NDropDownButton));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public bool IsAccented
        {
            get => (bool)GetValue(IsAccentedProperty);
            set => SetValue(IsAccentedProperty, value);
        }

        public PackIconModernKind ModernIcon
        {
            get => (PackIconModernKind)GetValue(ModernIconProperty);
            set => SetValue(ModernIconProperty, value);
        }

        public Orientation ContentOrientation
        {
            get => (Orientation) GetValue(ContentOrientationProperty);
            set => SetValue(ContentOrientationProperty, value);
        }

        public NDropDownButton()
        {
            InitializeComponent();
            Loaded += NDropDownButton_Loaded;
        }

        private void NDropDownButton_Loaded(object sender, RoutedEventArgs e)
        {
            IconControl.Visibility = ModernIcon == PackIconModernKind.None ? Visibility.Collapsed : Visibility.Visible;
            LoadStyle();
        }

        private void LoadStyle()
        {
            var style = Application.Current.Resources["SquareButtonStyle"] as Style;

            if (IsAccented)
                style = Application.Current.Resources["AccentedSquareButtonStyle"] as Style;

            var ss = new Style(typeof(Button), style);

            ss.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(1, 1, 1, 1)));

            CurrentButton.ButtonStyle = ss;
        }
    }
}
