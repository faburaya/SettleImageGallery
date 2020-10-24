using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using FileSystemUtils;
using Moq;
using Xunit;

namespace SettleImageGallery.UnitTests
{
    public class GalleryDirectoryTest
    {
        private static readonly string _fileExt = "jpg";

        [Fact]
        public void MoveAllImagesToFlatOrder_OnlyFilesOnTopLevel()
        {
            string dirPath = AsCrossPlatformPath("./home/Galerie");
            var fileNames = new string[] { $"eins.{_fileExt}", $"zwei.{_fileExt}" };
            var directory = new DirectoryNodeInfo(dirPath, null, fileNames);

            Mock<IFileSystemAccess> fileSystemAccessMock = SetupMockForFileSystemAccess();
            var galleryDirectory = new GalleryDirectory(fileSystemAccessMock.Object);
            galleryDirectory.MoveAllImagesToFlatOrder(directory);
            VerifyCallsTo(fileSystemAccessMock, dirPath, fileNames);
        }

        private static Mock<IFileSystemAccess> SetupMockForFileSystemAccess()
        {
            var mock = new Mock<IFileSystemAccess>(MockBehavior.Strict);

            mock.Setup(
                obj => obj.MoveFile(It.IsAny<string>(), It.IsAny<string>())
            ).Returns(true);

            return mock;
        }

        private static void VerifyCallsTo(Mock<IFileSystemAccess> mock,
                                          string destDirPath,
                                          IEnumerable<string> fileNames)
        {
            foreach (string fileName in fileNames)
            {
                string fromPath = Path.Join(destDirPath, fileName);
                mock.Verify(
                    obj => obj.MoveFile(
                        It.Is<string>(val => val == fromPath),
                        It.Is<string>(val =>
                            Regex.IsMatch(Path.GetRelativePath(destDirPath, val), $"^\\w+_\\d+\\.{_fileExt}$")
                        )
                    )
                );
            }
        }

        private static string AsCrossPlatformPath(string posixPath)
        {
            return posixPath.Replace('/', Path.DirectorySeparatorChar);
        }

    }// end of class GalleryDirectoryTest

}// end of namespace SettleImageGallery.UnitTests
