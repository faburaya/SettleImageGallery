using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FileSystemUtils;
using Moq;
using Xunit;
using System;

namespace SettleImageGallery.UnitTests
{
    public class GalleryDirectoryTest
    {
        private static readonly string _fileExt = "jpg";

        private static string[] CreateSomeFileNames()
        {
            return new string[] { $"Bild-1.{_fileExt}", $"Bild-2.{_fileExt}" };
        }

        private static string AsCrossPlatformPath(string posixPath)
        {
            return posixPath.Replace('/', Path.DirectorySeparatorChar);
        }

        [Fact]
        public void MoveAllImagesToFlatOrder_WhenFilesAlongSubdirs_ThenThrow()
        {
            string dirPath = AsCrossPlatformPath("./home/Galerie");
            string[] fileNames = CreateSomeFileNames();
            DirectoryNodeInfo[] subdirs = { new DirectoryNodeInfo(null, null, null) };
            var directory = new DirectoryNodeInfo(dirPath, subdirs, fileNames);

            Mock<IFileSystemAccess> fileSystemAccessMock = SetupMockForFileSystemAccess();
            var galleryDirectory = new GalleryDirectory(fileSystemAccessMock.Object);

            Assert.ThrowsAny<ApplicationException>(
                () => galleryDirectory.MoveAllImagesToFlatOrder(directory)
            );
        }

        [Fact]
        public void MoveAllImagesToFlatOrder_WhenDirectoryHasOnlyFiles_ThenNoPrefix()
        {
            string dirPath = AsCrossPlatformPath("./home/Galerie");
            string[] fileNames = CreateSomeFileNames();
            var directory = new DirectoryNodeInfo(dirPath, null, fileNames);

            Mock<IFileSystemAccess> fileSystemAccessMock = SetupMockForFileSystemAccess();
            var galleryDirectory = new GalleryDirectory(fileSystemAccessMock.Object);
            galleryDirectory.MoveAllImagesToFlatOrder(directory);

            VerifyCallsTo(fileSystemAccessMock, dirPath, fileNames, $"^\\w+_\\d+\\.{_fileExt}$");
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
                                          IEnumerable<string> fileNames,
                                          string regex)
        {
            foreach (string fileName in fileNames)
            {
                string fromPath = Path.Join(destDirPath, fileName);
                mock.Verify(
                    obj => obj.MoveFile(
                        It.Is<string>(val => val == fromPath),
                        It.Is<string>(val => Regex.IsMatch(Path.GetRelativePath(destDirPath, val), regex))
                    )
                );
            }
        }

    }// end of class GalleryDirectoryTest

}// end of namespace SettleImageGallery.UnitTests
