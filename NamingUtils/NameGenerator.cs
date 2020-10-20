using System;
using System.Linq;

namespace NamingUtils
{
    /// <summary>
    /// Hilft bei der Beschaffung von zuvälligen Namen für,
    /// zum Beispiel, Ordner und Dateien.
    /// </summary>
    public class NameGenerator
    {
        private Random _randomNumberGenerator;

        public NameGenerator()
        {
            _randomNumberGenerator = new Random(DateTime.Now.Millisecond);
        }

        private static readonly string[] availableWords = {
            "Spur", "Sonne", "Weg",
            "Haut", "Frieden", "Meer",
            "Wasser", "Sand", "Farbig",
            "Raum", "Ausblick", "Liebe",
            "Allein", "Land", "Boden",
            "Wand", "Richtig", "Schön"
        };

        public string GetRandomWord()
        {
            var randomIdx = (int)Math.Floor(_randomNumberGenerator.NextDouble() * availableWords.Length);
            return availableWords[randomIdx];
        }

        /// <summary>
        /// Erzeugt eine Liste von als String ausdruckte Nummer,
        /// die für die Erzeugung von Namen verwendet werden kann.
        /// </summary>
        /// <param name="count">Die eingegebene Größe der Liste.</param>
        /// <returns>Eine aufsteigend geordnete Liste von Nummer.</returns>
        public static string[] CreateOrderedListOfPrintedNumbers(ushort count)
        {
            if (count == 0)
                throw new ArgumentOutOfRangeException("Die eingegebene Größe der Liste muss großer als null sein!");

            var list = new string[count];
            var indexes = CreateIndexesFor(count);

            ushort highestIndex = indexes.Last();
            string formatForNumber = GetFormatStringForNumberUpTo(highestIndex);

            for (int idx = 0; idx < count; ++idx)
            {
                list[idx] = indexes[idx].ToString(formatForNumber);
            }

            return list;
        }
        
        /// <summary>
        /// Schlägt einen Bereich für den Index vor,
        /// der als Präfix in den Namen verwendet wird.
        /// </summary>
        /// <param name="count">Wie viele Objekten müssen von dem Index eingeordnet werden.</param>
        /// <returns>Der Bereich des Indexes, als eine maximale Anzahl von Objekten.</returns>
        private static ushort GuessIndexRange(ushort count)
        {
            var indexRange = (long)Math.Pow(10, Math.Round(Math.Log10(count) + 1));
            if (indexRange < ushort.MaxValue)
                return (ushort)indexRange;
            else
                throw new ArgumentOutOfRangeException("Die Anzahl von Objekten einzuordnen war so groß, dass der Bereich für den Index eine Grenze überschritten hat.");
        }

        /// <summary>
        /// Schafft eine Liste von Indexen, die genug Abstand voneinander halten.
        /// </summary>
        /// <param name="count">Die Größe der Liste.</param>
        /// <returns>Die geschaffene Liste.</returns>
        private static ushort[] CreateIndexesFor(ushort count)
        {
            ushort indexRange = GuessIndexRange(count);
            var interval = indexRange / count;
            var indexes = new ushort[count];
            indexes[0] = (ushort)Math.Round(interval / 2.0);

            for (int idx = 1; idx < count; ++idx)
                indexes[idx] = (ushort)(indexes[idx - 1] + interval);

            return indexes;
        }

        /// <summary>
        /// Bietet einen String für Formatierung an,
        /// der für die Verwandlung einer Nummer in String angewandt werden kann.
        /// </summary>
        /// <param name="number">Die eingegebene Nummer.</param>
        /// <returns>Ein String für Formatierung, der eine Nummer mit führenden Nullen druckt.</returns>
        private static string GetFormatStringForNumberUpTo(ushort number)
        {
            var numDigits = (byte)(Math.Round(Math.Log10(number) + 0.5) + 1);

            if (numDigits < 0 || numDigits > 9)
                throw new ArgumentOutOfRangeException("Die eingegebene Nummer ist großer als der von der Logik erwartete Wert.");

            char charForNumOfDigits = (char)('0' + numDigits);
            return $"D{charForNumOfDigits}";
        }
    }
}
