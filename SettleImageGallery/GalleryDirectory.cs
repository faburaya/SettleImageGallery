using FileSystemUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SettleImageGallery
{
    /// <summary>
    /// Verwendet die anderen kleinen Komponenten, um die Arbeit
    /// der Anwedung "SettleImageGallery" durchzuführen.
    /// </summary>
    public class GalleryDirectory
    {
        private NamingUtils.NameGenerator _nameGenerator;

        private FileSystemUtils.IFileSystemAccess _fileSystemAccess;

        /// <summary>
        /// Errichtet ein neues Objekt der Klasse <see cref="GalleryDirectory"/>.
        /// </summary>
        /// <param name="injectedFileSystemAccess">
        /// Eine Implementierung von der Schnittstelle <see cref="IFileSystemAccess"/>.
        /// </param>
        public GalleryDirectory(FileSystemUtils.IFileSystemAccess injectedFileSystemAccess)
        {
            this._fileSystemAccess = injectedFileSystemAccess;
            this._nameGenerator = new NamingUtils.NameGenerator();
        }

        /// <summary>
        /// Findet alle Dateien, die Bilder sind und unter dem angegebenen Ordner
        /// direkt oder indirekt stehen, und setzt sie nebeneinander unter diesem
        /// Ordner. Die Dateien werden umbennant, sodass die damalige hierarchische
        /// Ordnung aufbewahrt werden kann.
        /// </summary>
        /// <param name="directoryPath">
        /// Das Verzeichnis des Ordners, wo sich die Bildergalerie befindet.
        /// </param>
        public void MoveAllImagesToFlatOrder(string directoryPath)
        {
            DirectoryNodeInfo dirInfoTree = DirectoryNodeInfo.LoadTreeFrom(
                directoryPath,
                (file) => Filters.IsFile(file.Name, Filters.FileType.Picture)
            );
            MoveAllImagesToFlatOrder(dirInfoTree);
        }

        public void MoveAllImagesToFlatOrder(FileSystemUtils.DirectoryNodeInfo dirInfoTree)
        {
            int count = 0;
            var moves = ListFileMovesToExecute(dirInfoTree);
            foreach (var (fromPath, toPath) in moves)
            {
                if (_fileSystemAccess.MoveFile(fromPath, toPath))
                {
                    ++count;
                }
            }

            Console.WriteLine($"{count} aus {moves.Count} konnten erfolgreich verschoben werden.");
        }

        /// <summary>
        /// Listet alle Bewegungen von Dateien auf, die ausgeführt werden müssen.
        /// </summary>
        /// <param name="dirInfoTree">
        /// Die Diagramm des Dateisystems unter einem Order,
        /// der alle Dateien enthält, die verschoben werden müssen.</param>
        /// <returns>Eine Liste mit allen auszuführenden Bewegungen.</returns>
        private List<(string from, string to)> ListFileMovesToExecute(FileSystemUtils.DirectoryNodeInfo dirInfoTree)
        {
            var destinationByOrigin = new Dictionary<string, string>(); // hilt bei der Erkennung eines Zusammenstoßes
            var renamings = PlanFileRenamings(dirInfoTree, "");
            var moves = new List<(string from, string to)>(renamings.Count);

            foreach ((string fromPath, string toNewName) in renamings)
            {
                if (!destinationByOrigin.TryAdd(toNewName, fromPath))
                {
                    string coincidentFile = destinationByOrigin[toNewName];
                    throw new ApplicationException($"Unerwarteter Fehler: zwei Dateien müssen zum gleichen Namen umbennant werden! - {fromPath} und {coincidentFile} werden {toNewName}");
                }

                moves.Add((fromPath, Path.Join(dirInfoTree.FullPath, toNewName)));
            }

            return moves;
        }

        /// <summary>
        /// Sucht die Diagramm (DFS), um die Umbenennungen der Dateien zu planen.
        /// </summary>
        /// <param name="dirInfoTree">Die Diagramm des Dateisystems unter einem Ordner.</param>
        /// <param name="prefix">
        /// Das Präfix für den obersten Ordner in der angegebenen Diagram,
        /// das für alle Dateien unmittelbar darunter angewandt werden muss.
        /// </param>
        /// <returns>Eine Planung mit allen Umbenennungen, die ausgeführt werden müssen.</returns>
        private List<(string fromPath, string toNewName)>
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

                string nameRoot = _nameGenerator.GetRandomWord();
                string[] suffixes = NamingUtils.NameGenerator.CreateOrderedListOfPrintedNumbers((ushort)fileCount);

                int idx = 0;
                foreach (string fileName in dirInfoTree.FileNames)
                {
                    string fromPath = Path.Join(dirInfoTree.FullPath, fileName);
                    string toNewName = $"{prefix}{nameRoot}_{suffixes[idx++]}{Path.GetExtension(fileName)}";
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

    }// end of class GalleryDirectory

}// end of namespace SettleImageGallery
