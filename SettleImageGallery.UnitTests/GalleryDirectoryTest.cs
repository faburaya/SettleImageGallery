using System;
using System.IO;
using FileSystemUtils;
using Xunit;
using Moq;

namespace SettleImageGallery.UnitTests
{
    public class GalleryDirectoryTest
    {
        private static string AsCrossPlatformPath(string posixPath)
        {
            return posixPath.Replace('/', Path.DirectorySeparatorChar);
        }

        [Fact]
        public void MoveAllImagesToFlatOrder_OnlyFilesOnTopLevel()
        {
            string dirPath = AsCrossPlatformPath("./home/Gallerie");
            string fileExt = "jpg";
            var fileNames = new string[] { $"eins.{fileExt}", $"zwei.{fileExt}" };
            var directory = new DirectoryNodeInfo(dirPath, null, fileNames);

            var fileSystemAccessMock = new Mock<IFileSystemAccess>(MockBehavior.Strict);
            fileSystemAccessMock.Setup(
                obj => obj.MoveFile(It.IsAny<string>(), It.IsAny<string>())
            ).Returns(true);

            var galleryDirectory = new GalleryDirectory(fileSystemAccessMock.Object);
            galleryDirectory.MoveAllImagesToFlatOrder(directory);

            foreach (string fileName in fileNames)
            {
                string fromPath = Path.Join(dirPath, fileName);
                fileSystemAccessMock.Verify(
                    obj => obj.MoveFile(It.Is<string>(val => (val == fromPath)),
                                        It.IsRegex($"\\w+_\\d+\\.{fileExt}")),
                    Times.Once);
            }
        }
    }
}
