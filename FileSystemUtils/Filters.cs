using System;

namespace FileSystemUtils
{
    public static class Filters
    {
        public enum FileExtension
        {
            Jpeg,
            Png,
            // andere...
        }

        /// <summary>
        /// Überprüft, ob der Name der angegebene Datei eine spezifische Dateiedung enthält.
        /// Es wird nicht überprüft, ob die Datei tatsächlich existiert.
        /// </summary>
        /// <param name="filePath">Das Verzeichnis der angegebenen Datei.</param>
        /// <param name="extension">Die Dateiendung zu überprüfen.</param>
        /// <returns>Ob die Datei die Dateiendung enthält.</returns>
        public static bool IsFile(string filePath, FileExtension extension)
        {
            string filePathInLowerCase = filePath.ToLower();

            switch (extension)
            {
                case FileExtension.Jpeg:
                    return filePathInLowerCase.EndsWith(".jpeg")
                        || filePathInLowerCase.EndsWith(".jpg");

                case FileExtension.Png:
                    return filePathInLowerCase.EndsWith(".png");

                default:
                    throw new NotImplementedException("Dateiendung noch nicht unterstüzt!");
            };
        }

        public enum FileType
        {
            Picture,
            // andere...
        }

        /// <summary>
        /// Überprüft, ob die angegebene Datei (allein wegen ihrer Dateiendung)
        /// zu einem Typ gehört.
        /// Es wird nicht überprüft, ob die Datei tatsächlich existiert.
        /// </summary>
        /// <param name="filePath">Das Verzeichnis der angegebenen Datei.</param>
        /// <param name="type">Der Typ zu überprüfen.</param>
        /// <returns>Ob die Datei zu dem Typ gehört.</returns>
        public static bool IsFile(string filePath, FileType type)
        {
            switch (type)
            {
                case FileType.Picture:
                    return IsFile(filePath, FileExtension.Jpeg)
                        || IsFile(filePath, FileExtension.Png);
                default:
                    throw new NotImplementedException("Dateityp noch nicht unterstüzt!");
            };
        }

    }// end of class Filters
}
