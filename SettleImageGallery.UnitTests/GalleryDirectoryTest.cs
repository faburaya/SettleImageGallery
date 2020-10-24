using System;
using System.IO;
using FileSystemUtils;
using Xunit;
using Moq;
using System.Text.RegularExpressions;
using Castle.Components.DictionaryAdapter;

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
            string dirPath = AsCrossPlatformPath("./home/Galerie");
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
                    obj => obj.MoveFile(
                        It.Is<string>(val => val == fromPath),
                        It.Is<string>(val =>
                            Regex.IsMatch(Path.GetRelativePath(dirPath, val), $"\\w+_\\d+\\.{fileExt}$")
                        )
                    )
                );
            }
        }

    }// end of class GalleryDirectoryTest

}// end of namespace SettleImageGallery.UnitTests
