using System.IO;
using System.Linq;
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
                    new SNode("4.5")
                ),
                new SNode(
                    "data",
                    new SNode(
                        new SNode("!@#"),
                        new SNode(
                            new SNode("4.5")
                        ),
                        new SNode("(more"),
                        new SNode("data)"),
                        new SNode("\"quote\"")
                    )
                )
            );

            var expectedOutput = "((data \"quoted data\" 123 4.5) (data (!@# (4.5) \"(more\" \"data)\" \"\\\"quote\\\"\")))";

            // Act
            var output = root.ToString();

            // Assert
            Assert.Equal(expectedOutput, output);
        }

        [Theory]
        [InlineData("", "\"\"")]
        [InlineData("data", "data")]
        [InlineData("quoted data", "\"quoted data\"")]
        [InlineData("123", "123")]
        [InlineData("4.5", "4.5")]
        [InlineData("!@#", "!@#")]
        [InlineData("(more", "\"(more\"")]
        [InlineData("data)", "\"data)\"")]
        [InlineData("\"", "\"\\\"\"")]
        [InlineData(" \"hello\" world ", "\" \\\"hello\\\" world \"")]
        public void Serialize_Primitive_Node(string name, string expectedOutput)
        {
            // Arrange
            var node = new SNode(name);

            // Act
            var output = node.ToString();

            // Assert
            output.Should().Be(expectedOutput);
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
        [InlineData("\"\\\"\"", "\"")]
        [InlineData("\" \\\"hello\\\" world \"", " \"hello\" world ")]
        public void Parse_Primitive_Node(string input, string expectedName)
        {
            // Arrange
            var expectedNode = new SNode(expectedName);

            // Act
            var node = SNode.Parse(input);

            // Assert
            node.Should().BeEquivalentTo(expectedNode);
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
        [InlineData("(data \"quoted data\" 123 4.5)", "data", "quoted data", "123", "4.5")]
        [InlineData("(\"quoted data\" \"(more\")", "quoted data", "(more")]
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

        [Fact]
        public void Parse()
        {
            // Arrange
            var input = "((data \"quoted \\\" data\" 123 4.5)\n (data(!@# (4.5) \"(more\" \"data)\")))";

            var expectedNode = new SNode(
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
                        new SNode("4.5", isPrimitive: false),
                        new SNode("(more"),
                        new SNode("data)")
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
                        new SNode("4.5", isPrimitive: false),
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
        public void Parse_And_Clone_KiCadSym()
        {
            // Arrange
            var input = File.ReadAllText("Device.kicad_sym");

            // Act
            var node = SNode.Parse(input);
            var clone = node.Clone();

            // Assert
            node.Childs.Should().HaveCountGreaterThan(0);
            clone.Should().BeEquivalentTo(node);
        }
    }
}