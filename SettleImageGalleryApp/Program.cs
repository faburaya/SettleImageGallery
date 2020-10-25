using System;
using System.IO;
using System.Linq;

using FileSystemUtils;

namespace SettleImageGallery
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Falsche Verwendung! Bitte: SettleImageGallery Verzeichnis_für_Galerie");
            }

            IFileSystemAccess fileSystemAccess = new FileSystemAccess();
            var gallery = new GalleryDirectory(fileSystemAccess);
            string galleryDirPath = args[1];
            gallery.MoveAllImagesToFlatOrder(galleryDirPath);
            RemoveEmptySubdirectories(galleryDirPath);
        }

        private static void RemoveEmptySubdirectories(string directoryPath)
        {
            var dirInfo = new DirectoryInfo(directoryPath);
            if (!dirInfo.Exists)
            {
                throw new ApplicationException($"Das angegebene Verzeichnis ist entweder ungültig oder verweist nicht auf einen Ordner! - {directoryPath}");
            }

            foreach (var subdir in dirInfo.EnumerateDirectories())
            {
                RecursivelyRemoveEmptySubdirectories(subdir);
            }
        }

        private static void RecursivelyRemoveEmptySubdirectories(DirectoryInfo dirInfo)
        {
            foreach (var subdir in dirInfo.EnumerateDirectories())
            {
                RecursivelyRemoveEmptySubdirectories(subdir);
            }

            if (dirInfo.EnumerateFiles().Any())
            {
                Console.WriteLine($"Warnung: der Ordner {dirInfo.Name} konnte nicht gelöscht werden, weil er nicht leer ist! - {dirInfo.FullName}");
                return;
            }

            dirInfo.Delete(false);
        }

    }// Ende der Klasse Program

}// Ende des Namensraum SettleImageGallery
