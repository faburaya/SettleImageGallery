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
        [InlineData(@"\directory\file.gifa", Filters.FileExtension.Gif, false)]
        [InlineData(@"\directory\wepb", Filters.FileExtension.Webp, false)]
        [InlineData(@"\directory.webp\file", Filters.FileExtension.Webp, false)]
        [InlineData(@"\directory\file.jpg", Filters.FileExtension.Jpeg, true)]
        [InlineData(@"\directory\file.jpeg", Filters.FileExtension.Jpeg, true)]
        [InlineData(@"\directory\file.JPEG", Filters.FileExtension.Jpeg, true)]
        [InlineData(@"\directory\file.png", Filters.FileExtension.Png, true)]
        [InlineData(@"\directory\file.Png", Filters.FileExtension.Png, true)]
        [InlineData(@"\directory\file.gif", Filters.FileExtension.Gif, true)]
        [InlineData(@"\directory\file.GIF", Filters.FileExtension.Gif, true)]
        [InlineData(@"\directory\file.webp", Filters.FileExtension.Webp, true)]
        [InlineData(@"\directory\file.Webp", Filters.FileExtension.Webp, true)]
        [InlineData(@"\directory\file.mp4", Filters.FileExtension.Mp4, true)]
        [InlineData(@"\directory\file.MP4", Filters.FileExtension.Mp4, true)]
        [InlineData(@"\directory\file.wmv", Filters.FileExtension.Wmv, true)]
        [InlineData(@"\directory\file.WMV", Filters.FileExtension.Wmv, true)]
        public void IsFile_FromExtension(string filePath, Filters.FileExtension extension, bool expected)
        {
            Assert.Equal(expected, Filters.IsFile(filePath, extension));
        }

        [Theory]
        [InlineData(@"\directory\file.xxx", Filters.FileType.Picture, false)]
        [InlineData(@"\directory\file.xxx", Filters.FileType.Video, false)]
        [InlineData(@"\directory\file.xxx", Filters.FileType.Media, false)]
        [InlineData(@"\directory\file.jpg", Filters.FileType.Picture, true)]
        [InlineData(@"\directory\file.jpg", Filters.FileType.Video, false)]
        [InlineData(@"\directory\file.jpg", Filters.FileType.Media, true)]
        [InlineData(@"\directory\file.gif", Filters.FileType.Picture, true)]
        [InlineData(@"\directory\file.gif", Filters.FileType.Video, false)]
        [InlineData(@"\directory\file.gif", Filters.FileType.Media, true)]
        [InlineData(@"\directory\file.webp", Filters.FileType.Picture, true)]
        [InlineData(@"\directory\file.webp", Filters.FileType.Video, false)]
        [InlineData(@"\directory\file.webp", Filters.FileType.Media, true)]
        [InlineData(@"\directory\file.jpeg", Filters.FileType.Picture, true)]
        [InlineData(@"\directory\file.jpeg", Filters.FileType.Video, false)]
        [InlineData(@"\directory\file.jpeg", Filters.FileType.Media, true)]
        [InlineData(@"\directory\file.PNG", Filters.FileType.Picture, true)]
        [InlineData(@"\directory\file.PNG", Filters.FileType.Video, false)]
        [InlineData(@"\directory\file.PNG", Filters.FileType.Media, true)]
        [InlineData(@"\directory\file.mp4", Filters.FileType.Picture, false)]
        [InlineData(@"\directory\file.mp4", Filters.FileType.Video, true)]
        [InlineData(@"\directory\file.mp4", Filters.FileType.Media, true)]
        [InlineData(@"\directory\file.WMV", Filters.FileType.Picture, false)]
        [InlineData(@"\directory\file.WMV", Filters.FileType.Video, true)]
        [InlineData(@"\directory\file.WMV", Filters.FileType.Media, true)]
        public void IsFile_FromType(string filePath, Filters.FileType type, bool expected)
        {
            Assert.Equal(expected, Filters.IsFile(filePath, type));
        }
    }
}
