using System;
using System.Text;
using System.Xml;

namespace FontAwesome.Avalonia.Generator
{
    internal sealed class StylesGenerator : IDisposable
    {
        private readonly XmlWriter _xmlWriter;

        private static readonly XmlWriterSettings _xmlWriterSettings = new XmlWriterSettings()
        {
            Indent = true,
            IndentChars = new string(' ', 4),
            OmitXmlDeclaration = true,
        };

        public StylesGenerator(StringBuilder output)
        {
            _xmlWriter = XmlWriter.Create(output, _xmlWriterSettings);
            _xmlWriter.WriteStartElement("Styles", "https://github.com/avaloniaui");
            _xmlWriter.WriteAttributeString("xmlns", "x", null, "http://schemas.microsoft.com/winfx/2006/xaml");
        }

        private void WriteSetter(string property, string value)
        {
            _xmlWriter.WriteStartElement("Setter");
            _xmlWriter.WriteAttributeString("Property", property);
            _xmlWriter.WriteAttributeString("Value", value);
            _xmlWriter.WriteEndElement();
        }

        public void AddStyle(string selector, params (string property, string value)[] setter)
        {
            _xmlWriter.WriteStartElement("Style");
            _xmlWriter.WriteAttributeString("Selector", selector);
            foreach ((string property, string value) in setter)
            {
                WriteSetter(property, value);
            }
            _xmlWriter.WriteEndElement();
        }

        public void Done()
        {
            _xmlWriter.Close();
        }

        public void Dispose()
        {
            ((IDisposable)_xmlWriter).Dispose();
        }
    }
}