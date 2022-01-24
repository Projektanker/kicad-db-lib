using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using KiCadDbLib.Services.KiCad;
using Xunit;

namespace KiCad.UnitTest
{
    public class SNodeTest
    {
        [Fact]
        public void Echo()
        {
            // Arrange
            var input = "((data \"quoted data\" \"\\\"quote\\\"\" 123 4.5) (data (!@# (4.5) \"(more\" \"data)\")))";

            // Act
            var sNode = SNode.Parse(input);
            var output = sNode.ToString();

            // Assert
            Assert.Equal(input, output);
        }

        [Fact]
        public void SerializeTest()
        {
            // Arrange
            var root = new SNode(
                new SNode(
                    "data",
                    new SNode("quoted data"),
                    new SNode("123"),
                    new SNode("4.5", Array.Empty<SNode>())
                ),
                new SNode(
                    "data",
                    new SNode(
                        new SNode("!@#"),
                        new SNode("(more"),
                        new SNode("data)"),
                        new SNode("quote", isString: true)
                    )
                )
            );

            var expectedOutput = "((data \"quoted data\" 123 (4.5)) (data (!@# \"(more\" \"data)\" \"quote\")))";

            // Act
            var output = root.ToString();

            // Assert
            Assert.Equal(expectedOutput, output);
        }

        [Theory]
        [InlineData("", "\"\"")]
        [InlineData("data", "data")]
        [InlineData("with space", "\"with space\"")]
        [InlineData("string", "\"string\"", true)]
        [InlineData("123", "123")]
        [InlineData("4.5", "4.5")]
        [InlineData("!@#", "!@#")]
        [InlineData("(bracket", "\"(bracket\"")]
        [InlineData("bracket)", "\"bracket)\"")]
        [InlineData(" \"hello\" world ", "\" \\\"hello\\\" world \"")]
        public void Serialize_Primitive_Node(string name, string expectedOutput, bool isString = false)
        {
            // Arrange
            var node = new SNode(name, isString: isString);

            // Act
            var output = node.ToString();

            // Assert
            output.Should().Be(expectedOutput);
        }

        [Theory]
        [InlineData("(test)", "test")]
        [InlineData("(1 2)", "1", "2")]
        [InlineData("(data \"quoted data\" 123 4.5)", "data", "quoted data", "123", "4.5")]
        [InlineData("(\"quoted data\" \"(more\")", "quoted data", "(more")]
        public void Serialize_Non_Primitive_Node(string expectedOutput, string key, params string[] childs)
        {
            // Arrange
            var childNodes = childs.Select(child => new SNode(child)).ToArray();
            var node = new SNode(key, childNodes);

            // Act
            var output = node.ToString();

            // Assert
            output.Should().Be(expectedOutput);
        }

        [Theory]
        [InlineData("(test)", "test")]
        [InlineData("(1 2)", "1", "2")]
        [InlineData("(data 123 4.5)", "data", "123", "4.5")]
        public void Parse_Non_Primitive_Node(string input, string key, params string[] childs)
        {
            // Arrange
            var childNodes = childs.Select(child => new SNode(child)).ToArray();
            var expectedNode = new SNode(key, childNodes);

            // Act
            var node = SNode.Parse(input);

            // Assert
            node.Should().BeEquivalentTo(expectedNode);
        }

        [Theory]
        [InlineData("(\"test\")", "test")]
        [InlineData("(\" \\\"hello\\\" world \")", " \"hello\" world ")]
        [InlineData("(\"\")", "")]
        [InlineData("((\"with space\")", "with space")]
        [InlineData("(\"(bracket\")", "(bracket")]
        [InlineData("(\"bracket)\")", "bracket)")]
        public void Parse_String_Node(string input, string key)
        {
            // Arrange
            var expectedNode = new SNode(key, Array.Empty<SNode>())
            {
                IsString = true,
            };

            // Act
            var node = SNode.Parse(input);

            // Assert
            node.Should().BeEquivalentTo(expectedNode);
        }

        [Fact]
        public void Parse()
        {
            // Arrange
            var input = "((data \"quoted \\\" data\" 123 (4.5))\n (data(!@# \"(more\" \"data)\")))";

            var expectedNode = new SNode(
                new SNode(
                    "data",
                    new SNode("quoted \" data", isString: true),
                    new SNode("123"),
                    new SNode("4.5", Array.Empty<SNode>())
                ),
                new SNode(
                    "data",
                    new SNode(
                        "!@#",
                        new SNode("(more", isString: true),
                        new SNode("data)", isString: true)
                    )
                )
            );

            // Act
            var node = SNode.Parse(input);

            // Assert
            node.Should().BeEquivalentTo(expectedNode);
        }

        [Theory]
        [InlineData(0, "(level1 (level2 (level3)))", "(level1 (level2 (level3)))")]
        [InlineData(1, "(level1 (level2 (level3)))", "(level1)")]
        [InlineData(2, "(level1 (level2 (level3)))", "(level1 (level2))")]
        [InlineData(3, "(level1 (level2 (level3)))", "(level1 (level2 (level3)))")]
        [InlineData(4, "(level1 (level2 (level3)))", "(level1 (level2 (level3)))")]
        [InlineData(1, "(level1 1.1 (level2 2.1 2.2) 1.2)", "(level1 1.1 1.2)")]
        public void Parse_With_Depth(int depth, string input, string expectedOutput)
        {
            // Arrange
            var expectedNode = SNode.Parse(expectedOutput);

            // Act
            var node = SNode.Parse(input, depth);

            // Assert
            node.Should().BeEquivalentTo(expectedNode);
        }

        [Fact]
        public void Clone()
        {
            // Arrange
            var node = new SNode(
                new SNode(
                    "data",
                    new SNode("quoted \" data"),
                    new SNode("123"),
                    new SNode("4.5")
                ),
                new SNode(
                    "data",
                    new SNode(
                        "!@#",
                        new SNode("(more"),
                        new SNode("data)")
                    )
                )
            );

            // Act
            var clone = node.Clone();

            // Assert
            clone.Should().BeEquivalentTo(node);
        }

        [Fact]
        public async Task Parse_And_Clone_KiCadSymAsync()
        {
            // Arrange
            const string libraryName = "Device";
            await KicadDownloader.DownloadSymbolFile(libraryName);
            var input = File.ReadAllText($"{libraryName}.kicad_sym");

            // Act
            var node = SNode.Parse(input);

            // Assert
            node.Childs.Should().HaveCountGreaterThan(0);
        }
    }
}