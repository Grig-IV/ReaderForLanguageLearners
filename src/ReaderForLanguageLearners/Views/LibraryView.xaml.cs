using ReaderForLanguageLearners.Interfaces;
using ReaderForLanguageLearners.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ReaderForLanguageLearners.Views
{
    public partial class LibraryView : Window
    {
        public LibraryView()
        {
            InitializeComponent();
            this.DataContext = new LibraryViewModel();
        }

        private void ListViewItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var book = (IBook)(sender as ListViewItem).DataContext;
            var bookReader = new BookReaderView(book.Source);
            bookReader.Show();
        }
    }
}
