using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System.Xaml;

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
        [JsonIgnore]
        public string? ImagePath { get; set; }

        /// <summary>
        ///     Ссылка на объект типа семейства
        /// </summary>
        [JsonIgnore]
        public FamilySymbol FamilySymbol { get; set; }

        /// <summary>
        ///     Название семейства
        /// </summary>
        public string Family => FamilySymbol.Family.Name;

        /// <summary>
        ///     Добавлен ли символ в избранное
        /// </summary>
        [JsonIgnore]
        public bool IsFavorite { get; set; }
    }
}