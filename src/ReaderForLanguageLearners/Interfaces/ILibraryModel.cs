using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReaderForLanguageLearners.Interfaces
{
    interface ILibraryModel
    {
        ObservableCollection<IBook> Books { get; }

        void AddBooksAsync(IEnumerable<string> bookFilePaths);
    }
}
