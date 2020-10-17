using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSystemUtils
{
    /// <summary>
    /// Wenn das Dateisystem als ein Diagramm betrachtet wird, dann stellt jede Ordner
    /// einen Knoten dar, unter dem andere Ordner und Dateien sich befinden.
    /// Diese Klasse steht stellvertretend für einen Ordner und die Objekten halten
    /// nur die unverzichtbaren Daten, mit denen man die echten Dateien in Dateisystem
    /// zugreifen kann.
    /// Dadurch darf man mit Daten über das Dateisystem arbeiten, ohne dass das Dateisystem
    /// tatsächlich zugegriffen wird, was zum Testen sehr günstig ist.
    /// </summary>
    public class DirectoryNodeInfo
    {
        public string FullPath { get; private set; }

        /// <summary>
        /// Alle Ordner darunter, aufsteigend nach Namen sortiert.
        /// </summary>
        public IEnumerable<DirectoryNodeInfo> Subdirectories { get; private set; }

        /// <summary>
        /// Die Namen aller Dateien darunter, aufsteigend sortiert.
        /// </summary>
        public IEnumerable<string> FileNames { get; private set; }

        public DirectoryNodeInfo(string fullPath,
                                 IEnumerable<DirectoryNodeInfo> subdirectories,
                                 IEnumerable<string> fileNames)
        {
            this.FullPath = fullPath;
            this.Subdirectories = subdirectories;
            this.FileNames = fileNames;
        }

        /// <summary>
        /// Greift das Dateisystem zu, um die ganze Diagramm zu bilden.
        /// </summary>
        /// <param name="directoryPath">
        /// Das vollständige Verzeichnis für den Ordner, der als Startpunkt zum Laden gilt.
        /// </param>
        /// <param name="fileFilter">
        /// Eine Funktion, die filtert, welche Dateien berücksichtigt werden.
        /// </param>
        /// <returns>
        /// Ein <see cref="DirectoryNodeInfo">-Objekt, das Startpunkt der Diagramm ist,
        /// welche die Struktur im Dateisystem wiederspiegelt.
        /// </returns>
        static public DirectoryNodeInfo LoadTreeFrom(string directoryPath, Func<FileInfo, bool> fileFilter = null)
        {
            if (fileFilter == null)
            {
                fileFilter = (FileInfo) => true;
            }

            var fsysDirInfo = new DirectoryInfo(directoryPath);
            if (!fsysDirInfo.Exists)
                throw new ArgumentException($"Kein gültiges Verzeichnis für einen Ordner! {directoryPath}");

            return new DirectoryNodeInfo(
                directoryPath,
                // Ordner (durch Rekursion):
                from dir in fsysDirInfo.GetDirectories()
                orderby dir.Name
                select LoadTreeFrom(dir.FullName, fileFilter),
                // Dateien:
                from file in fsysDirInfo.GetFiles()
                where fileFilter(file)
                orderby file.Name
                select file.Name);
        }
    }
}
