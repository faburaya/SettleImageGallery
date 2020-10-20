using System;
using System.Text.RegularExpressions;
using Xunit;

namespace NamingUtils.UnitTests
{
    public class NameGeneratorTest
    {
        [Fact]
        public void CreateOrderedListOfPrintedNumbers_WhenCountIsZero_ThenThrow()
        {
            Assert.ThrowsAny<ArgumentException>(() => NamingUtils.NameGenerator.CreateOrderedListOfPrintedNumbers(0));
        }

        [Fact]
        public void CreateOrderedListOfPrintedNumbers_WhenCountIsOne()
        {
            Assert.Collection(
                NamingUtils.NameGenerator.CreateOrderedListOfPrintedNumbers(1),
                prefix => Assert.Equal("05", prefix));
        }

        [Theory]
        [InlineData(2, new string[] { "02", "07" })]
        [InlineData(3, new string[] { "02", "05", "08" })]
        [InlineData(4, new string[] { "012", "037", "062", "087" })]
        [InlineData(8, new string[] { "006", "018", "030", "042", "054", "066", "078", "090" })]
        public void CreateOrderedListOfPrintedNumbers_WhenCountIsSmall(ushort givenCount, string[] expectedPrefixes)
        {
            var actualPrefixes = NamingUtils.NameGenerator.CreateOrderedListOfPrintedNumbers(givenCount);
            Assert.Equal(expectedPrefixes, actualPrefixes);
        }

        [Theory]
        [InlineData(20, @"\d{4}")]
        [InlineData(50, @"\d{4}")]
        [InlineData(90, @"\d{5}")]
        public void CreateOrderedListOfPrintedNumbers_WhenCountIsBig(ushort givenCount, string patternOfPrefix)
        {
            var prefixes = NamingUtils.NameGenerator.CreateOrderedListOfPrintedNumbers(givenCount);
            Assert.Equal(givenCount, prefixes.Length);
            Assert.All(prefixes, prefix => Regex.IsMatch(prefix, patternOfPrefix));
        }
    }
}
