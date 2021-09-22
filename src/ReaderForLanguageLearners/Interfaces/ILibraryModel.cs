using ReaderForLanguageLearners.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReaderForLanguageLearners.Interfaces
{
    interface ILibraryModel
    {
        ObservableCollection<IBook> Books { get; }

        void AddBooks(IEnumerable<string> bookFilePaths);
    }
}
