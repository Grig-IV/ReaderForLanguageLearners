using ReaderForLanguageLearners.Models;
using ReaderForLanguageLearners.ViewModels;
using System.Windows;

namespace ReaderForLanguageLearners.Views
{
    public partial class LibraryView : Window
    {
        public LibraryView()
        {
            InitializeComponent();
            this.DataContext = new LibraryViewModel();
        }
    }
}
