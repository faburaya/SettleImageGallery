using System;
using System.IO;
using System.Linq;

namespace FileSystemUtils
{
    public class FileSystemAccess : IFileSystemAccess
    {
        public bool MoveFile(string fromPath, string toPath)
        {
            try
            {
                new FileInfo(fromPath).MoveTo(toPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FEHLER: Datei \"{fromPath}\" konnte nicht an \"{toPath}\" verschoben werden: {ex.Message}");
                return false;
            }
            return true;
        }

        public void RecursivelyRemoveEmptyDirectories(string directoryPath)
        {
            var dirInfo = new DirectoryInfo(directoryPath);
            if (!dirInfo.Exists)
            {
                throw new ApplicationException($"Das angegebene Verzeichnis ist entweder ungültig oder verweist nicht auf einen Ordner! - {directoryPath}");
            }

            RecursivelyRemoveEmptyDirectories(dirInfo);
        }

        private void RecursivelyRemoveEmptyDirectories(DirectoryInfo dirInfo)
        {
            foreach (var subdir in dirInfo.EnumerateDirectories())
            {
                RecursivelyRemoveEmptyDirectories(subdir);
            }

            if (dirInfo.EnumerateFiles().Any())
            {
                Console.WriteLine($"Warnung: der Ordner {dirInfo.Name} konnte nicht gelöscht werden, weil er nicht leer ist! - {dirInfo.FullName}");
                return;
            }

            dirInfo.Delete(false);
        }

    }// end of class FileSystemAccess

}// end of namespace FileSystemUtils
