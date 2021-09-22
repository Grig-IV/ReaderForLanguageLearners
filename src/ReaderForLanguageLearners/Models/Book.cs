using ReaderForLanguageLearners.Interfaces;
using System;

namespace ReaderForLanguageLearners.Models
{
    public record Book : IBook
    {
        public string Source { get; init; }

        public string Title { get; init; }

        public string Authors { get; init; }

        public string Language { get; init; }

        public DateTime Date { get; init; }
    }
}
