using System;
using System.Text.RegularExpressions;
using Xunit;

namespace NamingUtils.UnitTests
{
    public class NameGeneratorTest
    {
        [Fact]
        public void CreateOrderedListOfNumericalPrefixes_WhenCountIsZero_ThenThrow()
        {
            Assert.ThrowsAny<ArgumentException>(() => NamingUtils.NameGenerator.CreateOrderedListOfNumericalPrefixes(0));
        }

        [Fact]
        public void CreateOrderedListOfNumericalPrefixes_WhenCountIsOne()
        {
            Assert.Collection(
                NamingUtils.NameGenerator.CreateOrderedListOfNumericalPrefixes(1),
                prefix => Assert.Equal("05_", prefix));
        }

        [Theory]
        [InlineData(2, new string[] { "02_", "07_" })]
        [InlineData(3, new string[] { "02_", "05_", "08_" })]
        [InlineData(4, new string[] { "012_", "037_", "062_", "087_" })]
        [InlineData(8, new string[] { "006_", "018_", "030_", "042_", "054_", "066_", "078_", "090_" })]
        public void CreateOrderedListOfNumericalPrefixes_WhenCountIsSmall(ushort givenCount, string[] expectedPrefixes)
        {
            var actualPrefixes = NamingUtils.NameGenerator.CreateOrderedListOfNumericalPrefixes(givenCount);
            Assert.Equal(expectedPrefixes, actualPrefixes);
        }

        [Theory]
        [InlineData(20, @"\d{4}_")]
        [InlineData(50, @"\d{4}_")]
        [InlineData(90, @"\d{5}_")]
        public void CreateOrderedListOfNumericalPrefixes_WhenCountIsBig(ushort givenCount, string patternOfPrefix)
        {
            var prefixes = NamingUtils.NameGenerator.CreateOrderedListOfNumericalPrefixes(givenCount);
            Assert.Equal(givenCount, prefixes.Length);
            Assert.All(prefixes, prefix => Regex.IsMatch(prefix, patternOfPrefix));
        }
    }
}
