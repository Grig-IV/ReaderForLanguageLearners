using System.Windows.Documents;
using System.Xml;
using System.Windows;
using System;

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
            if (fb2Inline.LocalName == "#text") return new Run(fb2Inline.InnerText);

            Inline inline;
            if (fb2Inline.ChildNodes.Count == 1 && fb2Inline.FirstChild.LocalName == "#text")
            {
                inline = new Run(fb2Inline.InnerText);
            }
            else
            {
                var span = new Span();
                foreach (XmlNode child in fb2Inline)
                {
                    span.Inlines.Add(GetInline(child));
                }
                inline = span;
            }

            switch (fb2Inline.LocalName)
            {
                case "strong":
                    inline.FontWeight = FontWeights.DemiBold;
                    break;
                case "emphasis":
                    inline.FontStyle = FontStyles.Italic;
                    break;
                case "style":
                    throw new NotImplementedException();
                case "strikethrough":
                    inline.TextDecorations = TextDecorations.Strikethrough;
                    break;
                case "sub":
                    throw new NotImplementedException();
                case "sup":
                    throw new NotImplementedException();
                case "code":
                    throw new NotImplementedException();
                case "iamge":
                    throw new NotImplementedException();
            }

            return inline;
        }

        private BlockUIContainer GetImage(XmlElement fb2Image)
        {
            throw new NotImplementedException();
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

        private Section GetSubtitle(XmlNode fb2Subtitle)
        {
            var section = new Section();

            var paragraph = new Paragraph();
            foreach (XmlNode child in fb2Subtitle)
            {
                paragraph.Inlines.Add(GetInline(child));
            }
            section.Blocks.Add(paragraph);

            return section;
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
                        epigraph.Blocks.Add(GetCite(child));
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
       
        private Section GetPoem(XmlNode fb2Poem)
        {
            throw new NotImplementedException();
        }

        private Section GetCite(XmlNode fb2Cite)
        {
            var cite = new Section();
            foreach (XmlElement child in fb2Cite)
            {
                switch (child.LocalName)
                {
                    case "p":
                        cite.Blocks.Add(GetParagraph(child));
                        break;
                    case "subtitle":
                        throw new NotImplementedException();
                    case "empty-line":
                        cite.Blocks.Add(new Paragraph(new LineBreak()));
                        break;
                    case "poem":
                        cite.Blocks.Add(GetPoem(child));
                        break;
                    case "table":
                        cite.Blocks.Add(GetTable(child));
                        break;
                    case "text-author":
                        throw new NotImplementedException();    
                }
            }
            return cite;
        }

        private Section GetAuthor(XmlNode fb2Author)
        {
            throw new NotImplementedException();
        }

        private Section GetAnnotation(XmlNode fb2Annotation)
        {
            throw new NotImplementedException();
        }

        private Section GetTable(XmlNode fb2Table)
        {
            throw new NotImplementedException();
        }
    }
}
