using System.Windows.Documents;
using System.Xml;
using System.Windows;

namespace ReaderForLanguageLearners.ViewModels
{
    class BookReaderViewModel
    {
        public BookReaderViewModel(string bookFilePath)
        {
            var bookXml = new XmlDocument();
            bookXml.Load(bookFilePath);

            BookDocument = CreateBookDocument(bookXml["FictionBook"]["body"]);
        }

        public FlowDocument BookDocument { get; }

        private FlowDocument CreateBookDocument(XmlElement fb2Body)
        {
            var bookDocument = new FlowDocument();

            foreach (XmlElement child in fb2Body)
            {
                switch (child.LocalName)
                {
                    case "image":
                        bookDocument.Blocks.Add(GetImage(child));
                        break;
                    case "title":
                        bookDocument.Blocks.Add(GetTitle(child));
                        break;
                    case "epigraph":
                        bookDocument.Blocks.Add(GetEpigraph(child));
                        break;
                    case "section":
                        bookDocument.Blocks.Add(GetSection(child));
                        break;
                }
            }

            return bookDocument;
        }

        private BlockUIContainer GetImage(XmlElement fb2Image)
        {
            return new();
        }

        private Section GetTitle(XmlElement fb2Title)
        {
            var title = new Section();
            foreach (XmlElement child in fb2Title)
            {
                switch (child.LocalName)
                {
                    case "p":
                        title.Blocks.Add(GetParagraph(child));
                        break;
                    case "empty-line":
                        title.Blocks.Add(new Paragraph(new LineBreak()));
                        break;
                }
            }
            return title;
        }
        
        private Section GetEpigraph(XmlElement fb2Epigraph)
        {
            var epigraph = new Section();
            foreach (XmlNode child in fb2Epigraph)
            {
                switch (child.LocalName)
                {
                    case "p":
                        epigraph.Blocks.Add(GetParagraph(child));
                        break;
                    case "poem":
                        epigraph.Blocks.Add(GetPoem(child));
                        break;
                    case "city":
                        epigraph.Blocks.Add(GetCity(child));
                        break;
                    case "empty-line":
                        epigraph.Blocks.Add(new Paragraph(new LineBreak()));
                        break;
                    case "text-author":
                        epigraph.Blocks.Add(GetAuthor(child));
                        break;
                }
            }
            return epigraph;
        }
        
        private Section GetSection(XmlElement fb2Section)
        {
            var section = new Section();

            foreach (XmlElement child in fb2Section)
            {
                switch (child.LocalName)
                {
                    case "title":
                        section.Blocks.Add(GetTitle(child));
                        break;
                    case "epigraph":
                        section.Blocks.Add(GetEpigraph(child));
                        break;
                    case "image":
                        section.Blocks.Add(GetImage(child));
                        break;
                    case "annotation":
                        section.Blocks.Add(GetAnnotation(child));
                        break;
                    case "section":
                        section.Blocks.Add(GetSection(child));
                        break;
                    case "p":
                        section.Blocks.Add(GetParagraph(child));
                        break;
                    case "poem":
                        section.Blocks.Add(GetPoem(child));
                        break;
                    case "subtitle":
                        section.Blocks.Add(GetSubtitle(child));
                        break;
                    case "cite":
                        section.Blocks.Add(GetCite(child));
                        break;
                    case "empty-line":
                        section.Blocks.Add(new Paragraph(new LineBreak()));
                        break;
                    case "table":
                        section.Blocks.Add(GetTable(child));
                        break;
                }
            }

            return section;
        }
    
        private Paragraph GetParagraph(XmlNode fb2Paragraph)
        {
            var paragraph = new Paragraph();
            foreach (XmlNode child in fb2Paragraph)
            {
                paragraph.Inlines.Add(GetInline(child));
            }
            return paragraph;
        }

        private Inline GetInline(XmlNode fb2Inline)
        {
            return new Run();
        }
    }
}
