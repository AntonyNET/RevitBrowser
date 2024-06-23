using System.Linq;

namespace zRevitFamilyBrowser.Models
{
    /// <summary>
    ///     Сеймейство объектов
    /// </summary>
    public class FamilyDto
    {
        /// <summary>
        /// Название семейства
        /// </summary>
        public string Name { get; set; }

        public string? ImagePath => Symbols.FirstOrDefault(x => string.IsNullOrEmpty(x.ImagePath) == false)?.ImagePath;

        /// <summary>
        ///     Объекты семейства
        /// </summary>
        public FamilySymbolDto[] Symbols { get; set; }
    }
}