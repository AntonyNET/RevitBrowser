using System.Linq;
using zRevitFamilyBrowser.ViewModels;

namespace zRevitFamilyBrowser.Models
{
    /// <summary>
    ///     Сеймейство объектов
    /// </summary>
    public class FamilyDto
    {
        /// <summary>
        ///     Название семейства
        /// </summary>
        public string Name { get; set; }

        public string? ImagePath => Symbols.FirstOrDefault(x => string.IsNullOrEmpty(x.FamilySymbolDto.ImagePath) == false)?.FamilySymbolDto.ImagePath;

        /// <summary>
        ///     Объекты семейства
        /// </summary>
        public FamilySymbolViewModel[] Symbols { get; set; }
    }
}