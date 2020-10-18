using System;
using Xunit;

namespace FileSystemUtils.UnitTests
{
    using FileSystemUtils;

    public class FiltersTest
    {
        [Theory]
        [InlineData(@"\directory\file.xxx", Filters.FileExtension.Jpeg, false)]
        [InlineData(@"\directory\file.xxx", Filters.FileExtension.Png, false)]
        [InlineData(@"\directory\file.jpg", Filters.FileExtension.Jpeg, true)]
        [InlineData(@"\directory\file.jpeg", Filters.FileExtension.Jpeg, true)]
        [InlineData(@"\directory\file.JPEG", Filters.FileExtension.Jpeg, true)]
        [InlineData(@"\directory\file.png", Filters.FileExtension.Png, true)]
        [InlineData(@"\directory\file.Png", Filters.FileExtension.Png, true)]
        public void IsFile_FromExtension(string filePath, Filters.FileExtension extension, bool expected)
        {
            Assert.Equal(expected, Filters.IsFile(filePath, extension));
        }

        [Theory]
        [InlineData(@"\directory\file.xxx", Filters.FileType.Picture, false)]
        [InlineData(@"\directory\file.jpg", Filters.FileType.Picture, true)]
        [InlineData(@"\directory\file.jpeg", Filters.FileType.Picture, true)]
        [InlineData(@"\directory\file.PNG", Filters.FileType.Picture, true)]
        public void IsFile_FromType(string filePath, Filters.FileType type, bool expected)
        {
            Assert.Equal(expected, Filters.IsFile(filePath, type));
        }
    }
}
