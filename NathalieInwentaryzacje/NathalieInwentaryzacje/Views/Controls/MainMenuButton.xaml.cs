using System.Windows;
using MahApps.Metro.IconPacks;

namespace NathalieInwentaryzacje.Views.Controls
{
    /// <summary>
    /// Interaction logic for MainMenuButton.xaml
    /// </summary>
    public partial class MainMenuButton
    {
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string),
            typeof(MainMenuButton));

        public static DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(PackIconModernKind), typeof(MainMenuButton));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public PackIconModernKind Icon
        {
            get => (PackIconModernKind)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public MainMenuButton()
        {
            InitializeComponent();
        }
    }
}
