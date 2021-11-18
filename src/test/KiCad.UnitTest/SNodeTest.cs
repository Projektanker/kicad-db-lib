using System;
using System.IO;
using System.Linq;
using FluentAssertions;
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

        [Fact]
        public void Parse_KiCadSym()
        {
            // Arrange
            var input = File.ReadAllText("Device.kicad_sym");

            // Act
            SNode node = new SNode("not set");
            Action action = () => node = SNode.Parse(input);

            // Assert
            action.Should().NotThrow();
            node.Childs.Should().HaveCountGreaterThan(0);
        }
    }
}