namespace FileSystemUtils
{
    /// <summary>
    /// Vermittelt den Zugriff auf das Dateisystem, sodass man ihn durch ein Mock
    /// ersetzen darf und Teste durchführt, ohne tatsächliche Dateien zu berühren.
    /// </summary>
    public interface IFileSystemAccess
    {
        bool MoveFile(string fromPath, string toPath);

        void RecursivelyRemoveEmptyDirectories(string directoryPath);
    }
}
