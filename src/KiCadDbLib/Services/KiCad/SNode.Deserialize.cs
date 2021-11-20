using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace KiCadDbLib.Services.KiCad
{
    public partial class SNode
    {
        private static readonly Regex _tokenizer = new(
            "(?:(?<bo>\\()|(?<bc>\\))|(?<q>\\\".*?(?<!\\\\)\\\")|(?<s>[^()\\s]+))",
            RegexOptions.IgnorePatternWhitespace);

        public static SNode Parse(string input)
        {
            var tokens = Tokenize(input);

            var nodes = new Stack<SNode>();
            var node = new SNode(isPrimitive: true);
            foreach (var token in tokens)
            {
                switch (token.Key)
                {
                    // bracket open
                    case "bo":
                        nodes.Push(node);
                        node = new SNode(isPrimitive: false);
                        break;

                    // bracket close
                    case "bc":
                        var childNode = node;
                        node = nodes.Pop();
                        node.Add(childNode);
                        break;

                    // quoted string
                    case "q" when node.Name is null:
                        node.Name = UnescapeName(token.Value[1..^1]);
                        break;

                    case "q":
                        node.Add(UnescapedNode(token.Value[1..^1]));
                        break;

                    // string
                    case "s" when node.Name is null:
                        node.Name = UnescapeName(token.Value);
                        break;

                    case "s":
                        node.Add(UnescapedNode(token.Value));
                        break;

                    default:
                        break;
                }
            }

            return node.Childs.Count > 0
                ? node.Childs[0]
                : node;
        }

        private static IEnumerable<KeyValuePair<string, string>> Tokenize(string input)
        {
            input = input.Trim(' ');
            var match = _tokenizer.Match(input);
            while (match.Success)
            {
                yield return ExtractTokenValuePair(match);
                match = match.NextMatch();
            }
        }

        private static KeyValuePair<string, string> ExtractTokenValuePair(Match match)
        {
            var group = match.Groups.Values.Skip(1).First(group => group.Success);
            return KeyValuePair.Create(group.Name, group.Value);
        }

        private static string UnescapeName(string name)
        {
            return name.Replace("\\\"", "\"", StringComparison.Ordinal);
        }

        private static SNode UnescapedNode(string name)
        {
            var unescaped = UnescapeName(name);
            return new SNode(unescaped);
        }
    }
}