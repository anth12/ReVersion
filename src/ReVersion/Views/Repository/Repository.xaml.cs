using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using ReVersion.ViewModels.Home;

namespace ReVersion.Views.Repository
{
    /// <summary>
    ///     Interaction logic for Repository.xaml
    /// </summary>
    public partial class Repository : UserControl
    {
        public Repository()
        {
            InitializeComponent();
        }

        private void SplitButton_OnClick(object sender, RoutedEventArgs e)
        {
            ((RepositoryViewModel)DataContext).ButtonOptions[((SplitButton)sender).SelectedIndex]
                .Value.Execute(null);
            
        }
    }
}