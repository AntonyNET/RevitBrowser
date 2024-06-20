using Autodesk.Revit.DB;

namespace zRevitFamilyBrowser.Models
{
    public class FamilySymbolDto
    {
        /// <summary>
        ///     Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Путь до изображения, если у символа есть изображение, иначе null
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        ///     Ссылка на объект семейства
        /// </summary>
        public FamilySymbol FamilySymbol { get; set; }
    }
}