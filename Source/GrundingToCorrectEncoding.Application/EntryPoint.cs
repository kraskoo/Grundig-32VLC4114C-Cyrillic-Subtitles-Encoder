namespace GrundingToCorrectEncoding.Application
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class EntryPoint
    {
        private static char[] cyrillicSymbols;

        public static void Main()
        {
            Initialize();
            Save(ConvertToEncodedTable(ReadFile(), out string newFileName), newFileName);
        }

        private static string ConvertToEncodedTable(string fileText, out string newFileName)
        {
            var copied = new char[cyrillicSymbols.Length];
            Array.Copy(cyrillicSymbols, copied, copied.Length);
            copied[48 + 0] = '\u00C0'; // А
            copied[48 + 1] = (char)1118; // Б
            copied[48 + 2] = (char)1030; // В
            copied[48 + 3] = (char)1110; // Г
            copied[48 + 4] = 'D'; // Д
            copied[48 + 5] = 'E'; // Е
            copied[48 + 8] = (char)1105; // И
            copied[48 + 10] = (char)1108; // K
            copied[48 + 12] = (char)1112; // М
            copied[48 + 13] = (char)1029; // Н
            copied[48 + 14] = (char)1109; // О
            copied[48 + 15] = (char)1111; // П
            for (int i = 0; i < 16; i++)
            {
                ShiftToLeft(copied);
            }

            var shiftedSymbols = cyrillicSymbols.Zip(
                                    copied,
                                    (o, c) => new KeyValuePair<char, char>(o, c))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            shiftedSymbols['Ж'] = shiftedSymbols['ж'];
            shiftedSymbols['З'] = shiftedSymbols['з'];
            shiftedSymbols['Л'] = shiftedSymbols['л'];
            shiftedSymbols['Й'] = shiftedSymbols['й'];
            var output = new StringBuilder();
            foreach (var @char in fileText)
            {
                output.Append(
                    shiftedSymbols.ContainsKey(@char) ?
                        shiftedSymbols[@char] :
                        @char);
            }

            Console.Write("Enter path for encoded file: ");
            newFileName = Console.ReadLine();
            return output.ToString();
        }

        private static void Save(string fileText, string newFileName)
        {
            File.WriteAllText(newFileName, fileText, new CyrillicEncoding());
        }

        private static string ReadFile()
        {
            Console.Write("Enter path to subtitle file: ");
            var file = Console.ReadLine();
            return File.ReadAllText(file, Encoding.UTF8);
        }

        private static void ShiftToLeft<T>(T[] array)
        {
            var last = array[array.Length - 1];
            for (int i = array.Length - 1; i >= 1; i--)
            {
                array[i] = array[i - 1];
            }

            array[0] = last;
        }

        private static void Initialize()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("bg-BG");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("bg-BG");
            cyrillicSymbols = "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюя".ToCharArray();
        }
    }
}