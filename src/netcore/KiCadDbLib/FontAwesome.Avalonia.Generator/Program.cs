using System;
using System.Text;

namespace FontAwesome.Avalonia.Generator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            StringBuilder sb = new StringBuilder();
            FontAwesomeStylesGenerator.Generate(sb);
            Console.Write(sb);
        }
    }
}