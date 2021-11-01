using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace KiCad.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var input = "((data \"quoted data\" 123 4.5) (data (!@# (4.5) \"(more\" \"data)\")))";

            var sExpr = new SExpression();
            // Act
            var sNode = sExpr.Deserialize(input);
            var output = sExpr.Serialize(sNode);

            // Assert
            Assert.Equal(input, output);
        }

        [Fact]
        public void SerializeTest()
        {
            // Arrange
            var root = new SNode(
                new SNode(
                    new SNode("data"),
                    new SNode("quoted data"),
                    new SNode("123"),
                    new SNode("4.5")
                ),
                new SNode(
                    new SNode("data"),
                    new SNode(
                        new SNode("!@#"),
                        new SNode(
                            new SNode("4.5")
                        ),
                        new SNode("(more"),
                        new SNode("data)")
                    )
                )
            );

            var expectedOutput = "((data \"quoted data\" 123 4.5) (data (!@# (4.5) \"(more\" \"data)\")))";

            var sExpr = new SExpression();

            // Act
            var output = sExpr.Serialize(root);

            // Assert
            Assert.Equal(expectedOutput, output);
        }

        [Theory]
        [InlineData("\"\"", "")]
        [InlineData("data", "data")]
        [InlineData("\"quoted data\"", "quoted data")]
        [InlineData("123", "123")]
        [InlineData("4.5", "4.5")]
        [InlineData("!@#", "!@#")]
        [InlineData("\"(more\"", "(more")]
        [InlineData("\"data)\"", "data)")]
        [InlineData("\\\"", "\"")]
        [InlineData("\" \\\"hello\\\" world \"", " \"hello\" world ")]
        public void DeserializeSingleNameNode(string input, string expectedName)
        {
            // Arrange
            var expectedNode = new SNode(expectedName);
            var sExpr = new SExpression();

            // Act
            var node = sExpr.Deserialize(input);

            // Assert
            node.Should().BeEquivalentTo(expectedNode);
        }

        [Theory]
        [InlineData("(1 2)", "1", "2")]
        [InlineData("(data \"quoted data\" 123 4.5)", "data", "quoted data", "123", "4.5")]
        [InlineData("(\"quoted data\" \"(more\")", "quoted data", "(more")]
        public void DeserializeMultiNameNode(string input, params string[] expectedNames)
        {
            // Arrange
            var expectedNode = new SNode();
            foreach (var name in expectedNames)
            {
                expectedNode.Add(new SNode(name));
            }

            var sExpr = new SExpression();

            // Act
            var node = sExpr.Deserialize(input);

            // Assert
            node.Should().BeEquivalentTo(expectedNode);
        }

        [Fact]
        public void DeserializeTest()
        {
            // Arrange
            var input = "((data \"quoted \\\" data\" 123 4.5)\n (data(!@# (4.5) \"(more\" \"data)\")))";

            var expectedNode = new SNode(
                new SNode(
                    new SNode("data"),
                    new SNode("quoted \" data"),
                    new SNode("123"),
                    new SNode("4.5")
                ),
                new SNode(
                    new SNode("data"),
                    new SNode(
                        new SNode("!@#"),
                        new SNode(
                            new SNode("4.5")
                        ),
                        new SNode("(more"),
                        new SNode("data)")
                    )
                )
            );

            var sExpr = new SExpression();

            // Act
            var node = sExpr.Deserialize(input);

            // Assert
            node.Should().BeEquivalentTo(expectedNode);
        }

        [Fact]
        public void DeserializeKiCadSym()
        {
            // Arrange
            var input = File.ReadAllText("Device.kicad_sym");

            var sExpr = new SExpression();

            // Act
            SNode node = new SNode("not set");
            Action action = () => node = sExpr.Deserialize(input);

            // Assert
            action.Should().NotThrow();
            node.Childs.Should().HaveCountGreaterThan(0);
        }
    }

    public class SNode
    {
        private readonly List<SNode> _childs = new();

        public SNode()
        {
        }

        public SNode(string name)
        {
            Name = name;
        }

        public SNode(params SNode[] childs)
        {
            _childs.AddRange(childs);
        }

        public string? Name { get; }
        public IReadOnlyList<SNode> Childs => _childs.AsReadOnly();

        public void Add(SNode node)
        {
            _childs.Add(node);
        }

        public override string ToString()
        {
            if (Name is not null)
            {
                return Name;
            }
            else
            {
                return _childs.Count.ToString();
            }
        }
    }

    public class SExpression
    {
        private static readonly Regex _tokenizer = new(
            "(?:(?<bo>\\()|(?<bc>\\))|(?<q>\\\".*?(?<!\\\\)\\\")|(?<s>[^()\\s]+))",
            RegexOptions.IgnorePatternWhitespace);

        public SNode Deserialize(string input)
        {
            input = input.Trim(' ');
            var matches = _tokenizer.Matches(input);
            var tokens = matches
                .Select(ExtractTokenValuePair)
                .ToArray();

            var nodes = new Stack<SNode>();
            var node = new SNode();
            foreach (var token in tokens)
            {
                switch (token.Key)
                {
                    // bracket open
                    case "bo":
                        nodes.Push(node);
                        node = new SNode();
                        break;

                    // bracket close
                    case "bc":
                        var childNode = node;
                        node = nodes.Pop();
                        node.Add(childNode);
                        break;

                    // quoted string
                    case "q":
                        node.Add(UnescapedNode(token.Value[1..^1]));
                        break;

                    // string
                    case "s":
                        node.Add(UnescapedNode(token.Value));
                        break;

                    default:
                        break;
                }
            }

            return node.Childs[0];
        }

        public string Serialize(SNode node)
        {
            var sb = new StringBuilder();
            Serialize(node, sb);
            return sb.ToString();
        }

        private static KeyValuePair<string, string> ExtractTokenValuePair(Match match)
        {
            var group = match.Groups.Values.Skip(1).First(group => group.Success);
            return KeyValuePair.Create(group.Name, group.Value);
        }

        private static SNode UnescapedNode(string input)
        {
            var unescaped = input.Replace("\\\"", "\"");
            return new SNode(unescaped);
        }

        private static void Serialize(SNode node, StringBuilder sb)
        {
            sb.Append('(');

            var first = true;
            foreach (var child in node.Childs)
            {
                if (!first)
                {
                    sb.Append(' ');
                }
                else
                {
                    first = false;
                }

                if (child.Childs.Count > 0)
                {
                    Serialize(child, sb);
                }
                else
                {
                    SerializeItem(child, sb);
                }
            }

            sb.Append(')');
        }

        private static void SerializeItem(SNode node, StringBuilder sb)
        {
            if (node.Name is null)
            {
                sb.Append("()");
                return;
            }

            var name = node.Name.Replace("\"", "\\\"");
            if (name.IndexOfAny(new char[] { ' ', '"', '(', ')' }) != -1 || node.Name.Length == 0)
            {
                sb.Append('"').Append(node.Name).Append('"');
                return;
            }

            sb.Append(name);
        }
    }
}