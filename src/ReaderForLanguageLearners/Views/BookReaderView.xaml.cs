using ReaderForLanguageLearners.ViewModels;
using System.Windows;

namespace ReaderForLanguageLearners.Views
{
    public partial class BookReaderView : Window
    {
        public BookReaderView(string bookFilePath)
        {
            InitializeComponent();
            this.DataContext = new BookReaderViewModel(bookFilePath);
        }
    }
}
