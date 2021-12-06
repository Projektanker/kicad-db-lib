using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace KiCadDbLib.Services.KiCad
{
    public partial class SNode
    {
        private static readonly HashSet<char> _tokenEndChars = new(new[] { '(', ')', ' ', '\t', '\n', '\r' });

        private enum TokenType
        {
            BracketOpen,
            BracketClose,
            String,
            Token,
        }

        public static SNode Parse(string input, int depth = 0)
        {
            var sw = Stopwatch.StartNew();
            var tokens = Tokenize(input).ToArray();
            var timeToken = sw.ElapsedMilliseconds;
            Debug.WriteLine($"Tokenize: {timeToken}ms");

            sw.Restart();
            var level = 0;
            var nodes = new Stack<SNode>();
            var node = EmptyNode();
            foreach (var token in tokens)
            {
                if (ShouldIgnoreToken(token.Key, ref level, depth))
                {
                    continue;
                }

                switch (token.Key)
                {
                    // bracket open
                    case TokenType.BracketOpen:
                        nodes.Push(node);
                        node = EmptyNode();
                        break;

                    // bracket close
                    case TokenType.BracketClose:
                        var childNode = node;
                        node = nodes.Pop();
                        node.Add(childNode);
                        break;

                    // quoted string
                    case TokenType.String when node.Name is null:
                        node.Name = token.Value;
                        node.IsString = true;
                        break;

                    case TokenType.String:
                        node.Add(StringNode(token.Value));
                        break;

                    // token
                    case TokenType.Token when node.Name is null:
                        node.Name = token.Value;
                        break;

                    case TokenType.Token:
                        node.Add(PrimitiveNode(token.Value));
                        break;

                    default:
                        break;
                }
            }

            var timeTree = sw.ElapsedMilliseconds;
            Debug.WriteLine($"Tree: {timeTree}ms");

            return node.Childs.Count > 0
                ? node.Childs[0]
                : node;
        }

        public static SNode EmptyNode()
        {
            return new SNode(Array.Empty<SNode>())
            {
                IsString = false,
            };
        }

        public static SNode PrimitiveNode(string name)
        {
            return new SNode(name)
            {
                IsString = false,
            };
        }

        public static SNode StringNode(string name)
        {
            return new SNode(name)
            {
                IsString = true,
            };
        }

        private static bool ShouldIgnoreToken(TokenType tokenType, ref int level, int depth)
        {
            if (depth == 0)
            {
                return false;
            }

            switch (tokenType)
            {
                // bracket open
                case TokenType.BracketOpen:
                    level++;
                    return level > depth;

                // bracket close
                case TokenType.BracketClose:
                    level--;
                    return level >= depth;

                default:
                    return level > depth;
            }
        }

        private static string ExtractString(ReadOnlySpan<char> input, ref int index)
        {
            var start = index;
            index++;
            while (input[index] != '"' || input[index - 1] == '\\')
            {
                index++;
            }

            index++;
            var sb = new StringBuilder(index - start - 2);
            return sb
                .Append(input[(start + 1)..(index - 1)])
                .Replace("\\\"", "\"")
                .ToString();
        }

        private static string ExtractToken(string input, ref int index)
        {
            var start = index;
            while (!_tokenEndChars.Contains(input[index]))
            {
                index++;
            }

            return input[start..index];
        }

        private static IEnumerable<KeyValuePair<TokenType, string>> Tokenize(string input)
        {
            var whitespace = new HashSet<char>(new[] { ' ', '\t', '\n', '\r' });
            var index = 0;
            while (index < input.Length)
            {
                if (input[index] == '(')
                {
                    yield return KeyValuePair.Create(TokenType.BracketOpen, string.Empty);
                    index++;
                }
                else if (input[index] == ')')
                {
                    yield return KeyValuePair.Create(TokenType.BracketClose, string.Empty);
                    index++;
                }
                else if (input[index] == '"')
                {
                    yield return KeyValuePair.Create(TokenType.String, ExtractString(input, ref index));
                }
                else if (!whitespace.Contains(input[index]))
                {
                    yield return KeyValuePair.Create(TokenType.Token, ExtractToken(input, ref index));
                }
                else
                {
                    index++;
                }
            }
        }
    }
}