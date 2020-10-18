using System;
using System.Collections.Generic;
using System.Linq;

namespace SettleImageGallery
{
    public class GalleryDirectory
    {
        private readonly FileSystemUtils.DirectoryNodeInfo _directoryTree;

        public GalleryDirectory(FileSystemUtils.DirectoryNodeInfo directoryTree)
        {
            this._directoryTree = directoryTree;
        }

        private static List<(string fromPath, string toNewName)>
        PlanFileRenamings(FileSystemUtils.DirectoryNodeInfo dirInfoTree,
                          string prefix)
        {
            if (dirInfoTree.FileNames.Any() && dirInfoTree.Subdirectories.Any())
            {
                throw new ApplicationException($"Ein Ordner enthält gleichzeitig Dateien und Ordner, was unmöglich macht, zu bestimmen, wo die Dateien eingeordnet werden können. Bitte verlegen Sie sie unter einem Ordner, der nur Dateien enthält. - {dirInfoTree.FullPath}");
            }

            var movements = new List<(string fromPath, string toPath)>();

            foreach (var subdir in dirInfoTree.Subdirectories)
            {

            }

            foreach (string fileName in dirInfoTree.FileNames)
            {

            }

            return movements;
        }
    }
}
