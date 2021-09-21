using Microsoft.Win32;
using ReaderForLanguageLearners.Interfaces;
using ReaderForLanguageLearners.Models;
using ReaderForLanguageLearners.Utils;
using System.Collections.ObjectModel;

namespace ReaderForLanguageLearners.ViewModels
{
    class LibraryViewModel
    {
        private ILibraryModel _libraryModel = LibraryModel.Instance;
        private RelayCommand _addBookComand;

        public ObservableCollection<IBookData> Books => _libraryModel.Books;

        public RelayCommand AddBookComand
        {
            get => _addBookComand ??= new RelayCommand(_ =>
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "FB2 books (*.fb2)|*.fb2",
                    Multiselect = true
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (string bookFilePath in openFileDialog.FileNames)
                    {
                        _libraryModel.AddBook(bookFilePath);
                    }
                }
            });
        }
    }
}
