using System;
using System.IO;

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
    }
}
