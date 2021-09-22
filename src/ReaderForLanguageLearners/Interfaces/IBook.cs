using System;

namespace ReaderForLanguageLearners.Interfaces
{
    interface IBook
    {
        string Source { get; }
        string Title { get; }
        string Authors { get; }
        string Language { get; }
        DateTime Date { get; }
    }
}
