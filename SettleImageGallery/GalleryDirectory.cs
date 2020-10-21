using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SettleImageGallery
{
    /// <summary>
    /// 
    /// </summary>
    public class GalleryDirectory
    {
        private readonly FileSystemUtils.DirectoryNodeInfo _directoryTree;

        public GalleryDirectory(FileSystemUtils.DirectoryNodeInfo directoryTree)
        {
            this._directoryTree = directoryTree;
        }

        /// <summary>
        /// Sucht die Diagramm (DFS), um die Umbenennungen der Dateien zu planen.
        /// </summary>
        /// <param name="dirInfoTree">Die Diagramm des Dateisystems unter einem Ordner.</param>
        /// <param name="prefix">
        /// Das Präfix für den obersten Ordner in der angegebenen Diagram,
        /// das für alle Dateien unmittelbar darunter angewandt werden muss.
        /// </param>
        /// <returns>Eine Planung mit allen Umbenennungen, die ausgeführt werden müssen. </returns>
        private static List<(string fromPath, string toNewName)>
        PlanFileRenamings(FileSystemUtils.DirectoryNodeInfo dirInfoTree,
                          string prefix)
        {
            if (dirInfoTree.FileNames.Any() && dirInfoTree.Subdirectories.Any())
            {
                throw new ApplicationException($"Ein Ordner enthält gleichzeitig Dateien und Ordner, was unmöglich macht, zu bestimmen, wo die Dateien eingeordnet werden können. Bitte verlegen Sie sie unter einem Ordner, der nur Dateien enthält. - \"{dirInfoTree.FullPath}\"");
            }

            var renamings = new List<(string fromPath, string toPath)>();

            int fileCount = dirInfoTree.FileNames.Count();
            if (fileCount > 0) // nur Dateien:
            {
                if (fileCount > ushort.MaxValue)
                {
                    throw new ApplicationException($"Die Anzahl von Dateien im Ordner ist schlicht zu groß. - \"{dirInfoTree.FullPath}\" enthält {fileCount} Dateien");
                }

                var nameGenerator = new NamingUtils.NameGenerator();
                string nameRoot = nameGenerator.GetRandomWord();
                string[] suffixes = NamingUtils.NameGenerator.CreateOrderedListOfPrintedNumbers((ushort)fileCount);

                int idx = 0;
                foreach (string fileName in dirInfoTree.FileNames)
                {
                    string fromPath = Path.Join(dirInfoTree.FullPath, fileName);
                    string toNewName = $"{prefix}{nameRoot}_{suffixes[idx++]}.{Path.GetExtension(fileName)}";
                    renamings.Add((fromPath, toNewName));
                }
            }
            else // nur Ordner:
            {
                int subdirCount = dirInfoTree.Subdirectories.Count();
                if (subdirCount > ushort.MaxValue)
                {
                    throw new ApplicationException($"Die Anzahl von Ordner ist schlicht zu groß. - \"{dirInfoTree.FullPath}\" enthält {subdirCount} Ordner");
                }

                string[] indices = NamingUtils.NameGenerator.CreateOrderedListOfPrintedNumbers((ushort)subdirCount);

                int idx = 0;
                foreach (var subdir in dirInfoTree.Subdirectories)
                {
                    string newPrefix = $"{prefix}{indices[idx++]}_";
                    renamings.AddRange(PlanFileRenamings(subdir, newPrefix));
                }
            }

            return renamings;
        }
    }
}
