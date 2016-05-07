using System.Windows;
using MahApps.Metro.Controls;
using ReVersion.ViewModels.MarkDown;

namespace ReVersion.Views
{
    /// <summary>
    /// Interaction logic for MarkDownWindow.xaml
    /// </summary>
    public partial class MarkDownWindow : MetroWindow
    {
        internal MarkDownWindow(MarkDownViewModel viewModel)
        {
            InitializeComponent();

            this.DataContext = viewModel;
            Closing += viewModel.WindowClosing;
        }
    }
}
