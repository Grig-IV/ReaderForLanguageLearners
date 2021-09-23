using FB2Library;
using ReaderForLanguageLearners.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace ReaderForLanguageLearners.Models
{
    class LibraryModel : ILibraryModel
    {
        private static LibraryModel _instance;
        private const string DB_FILENAME = "Rll_DB.db";

        static LibraryModel()
        {
            _instance = new LibraryModel();
        }

        private LibraryModel()
        {
            EnsureDbExist();
            Books = new(LoadDbBooks());
        }

        public static LibraryModel Instance => _instance;

        public ObservableCollection<IBook> Books { get; }

        public async void AddBooksAsync(IEnumerable<string> bookFilePaths)
        {
            var createBookTasks = bookFilePaths.Where(s => Books.All(b => b.Source != s))
                                               .Select(s => Task.Run(() => CreateBook(s)));
            var newBooks = await Task.WhenAll<IBook>(createBookTasks);

            using (var connection = new SQLiteConnection($"Data Source={DB_FILENAME}; Mode=ReadWrite"))
            {
                connection.Open();

                foreach (Book book in newBooks)
                {
                    Books.Add(book);
                    var insertBookCommand = GetInsertBookCommand(book, connection);
                    insertBookCommand.ExecuteNonQuery();
                }
            }
        }

        private IBook CreateBook(string bookFilePath)
        {
            var bookXml = System.IO.File.ReadAllText(bookFilePath);
            var bookFB2 = new FB2Reader().ReadAsync(bookXml).Result;
            var titleInfo = bookFB2.TitleInfo;

            return new Book()
            {
                Source = bookFilePath,
                Title = titleInfo.BookTitle.Text,
                Authors = String.Join('~', titleInfo.BookAuthors.Select(a => $"{a.FirstName} {a.MiddleName} {a.LastName}")),
                Language = titleInfo.Language,
                Date = titleInfo.BookDate.DateValue,
            };
        }

        private SQLiteCommand GetInsertBookCommand(IBook book, SQLiteConnection connection)
        {
            var insertBookCommand = new SQLiteCommand
            {
                CommandText = "INSERT INTO Books(Source, Title, Authors, Language, Date) VALUES (@Source, @Title, @Authors, @Language, @Date)",
                Connection = connection,
            };
            insertBookCommand.Parameters.AddWithValue("@Source", book.Source);
            insertBookCommand.Parameters.AddWithValue("@Title", book.Title);
            insertBookCommand.Parameters.AddWithValue("@Authors", book.Authors);
            insertBookCommand.Parameters.AddWithValue("@Language", book.Language);
            insertBookCommand.Parameters.AddWithValue("@Date", book.Date);

            return insertBookCommand;
        }

        private void EnsureDbExist()
        {
            if (System.IO.File.Exists(DB_FILENAME)) return;

            using (var connection = new SQLiteConnection($"Data Source={DB_FILENAME}; Mode=ReadWriteCreate"))
            {
                connection.Open();

                var createBooksTableCommand = new SQLiteCommand
                {
                    CommandText = "CREATE TABLE Books(Source TEXT NOT NULL PRIMARY KEY UNIQUE, Title TEXT, Authors TEXT, Language TEXT, Date TEXT)",
                    Connection = connection,
                };
                createBooksTableCommand.ExecuteNonQuery();
            }
        }
    
        private List<IBook> LoadDbBooks()
        {
            var loadedBooks = new List<IBook>();
            using (var connection = new SQLiteConnection($"Data Source={DB_FILENAME}; Mode=ReadWriteCreate"))
            {
                connection.Open();

                var getAllBooksCommand = new SQLiteCommand("SELECT * FROM Books", connection);
                using (SQLiteDataReader reader = getAllBooksCommand.ExecuteReader())
                {
                    if (!reader.HasRows) return new();

                    while (reader.Read())
                    {
                        var book = new Book
                        {
                            Source = reader.GetString(0),
                            Title = reader.GetString(1),
                            Authors = reader.GetString(2),
                            Language = reader.GetString(3),
                            Date = reader.GetDateTime(4),
                        };
                        loadedBooks.Add(book);
                    }
                }
            }
            return loadedBooks;
        }
    }
}
