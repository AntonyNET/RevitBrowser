using System.IO;

namespace zRevitFamilyBrowser.Services
{
    public class FileService
    {
        private static string _temporaryImageFolder;
        private static char[] _invalidFileNameChars;

        static FileService()
        {
            _invalidFileNameChars = Path.GetInvalidFileNameChars();
        }

        public static string GetTempDirectoryPath()
        {
            if (_temporaryImageFolder is not null)
                return _temporaryImageFolder;

            _temporaryImageFolder = Path.Combine(Path.GetTempPath(), "FamilyBrowser"); // папка temp?

            if (Directory.Exists(_temporaryImageFolder) == false)
                Directory.CreateDirectory(_temporaryImageFolder);

            return _temporaryImageFolder;
        }

        public static string GetFullPath(string filename)
        {
            foreach (var invalidFileNameChar in _invalidFileNameChars)
                filename = filename.Replace(invalidFileNameChar, ' ');

            return Path.Combine(GetTempDirectoryPath(), filename);
        }
    }
}