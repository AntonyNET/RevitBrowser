using System.IO;

namespace zRevitFamilyBrowser.Services
{
    /// <summary>
    ///     Сервис для работы с файлами
    /// </summary>
    public class FileService
    {
        /// <summary>
        ///     Путь до временной папки с изображениями семейств
        /// </summary>
        private static readonly string TemporaryImageFolder;

        /// <summary>
        ///     Запрещенные в имени файлов символы. Используются для замены не валидных символов на валидные, так как семейства
        ///     иногда содержат запрещенные символы
        /// </summary>
        private static readonly char[] InvalidFileNameChars;

        static FileService()
        {
            InvalidFileNameChars = Path.GetInvalidFileNameChars();

            TemporaryImageFolder = Path.Combine(Path.GetTempPath(), "FamilyBrowser");

            if (Directory.Exists(TemporaryImageFolder) == false)
                Directory.CreateDirectory(TemporaryImageFolder);
        }

        /// <summary>
        ///     Получение пути до временной папки с мзображениями семейств
        /// </summary>
        public static string GetTempDirectoryPath()
        {
            return TemporaryImageFolder;
        }

        /// <summary>
        ///     Получение полного пути файла по названию файла
        /// </summary>
        /// <remarks>полный путь состоит из временной папки и переданного имени файла</remarks>
        /// <param name="filename">имя файла, для которого необходимо получить полный путь</param>
        /// <returns>полный путь до файла</returns>
        public static string GetFullPath(string filename)
        {
            foreach (var invalidFileNameChar in InvalidFileNameChars)
                filename = filename.Replace(invalidFileNameChar, ' ');

            return Path.Combine(GetTempDirectoryPath(), filename);
        }
    }
}