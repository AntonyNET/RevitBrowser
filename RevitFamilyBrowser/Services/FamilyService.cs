using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using zRevitFamilyBrowser.Models;
using Document = Autodesk.Revit.DB.Document;

namespace zRevitFamilyBrowser.Services
{
    public static class FamilyService
    {
        public static List<FamilyDto> Families;
        private static readonly Size DefaultFamilySymbolImageSize; // нужно начинать название с _?
        private static readonly string DefaultFamilySymbolImagePath;
        public static FamilySymbolDto? SelectedFamilySymbol { get; set; }

        static FamilyService()
        {
            Families = new List<FamilyDto>();

            DefaultFamilySymbolImageSize = new Size(50, 50);
            DefaultFamilySymbolImagePath = FileService.GetFullPath($"{nameof(Properties.Resources.RevitLogo)}.bmp"); // всегда используется полный путь?

            Properties.Resources.RevitLogo.Save(DefaultFamilySymbolImagePath); // зачем?
        }

        /// <summary>
        /// Создание изображений для всех элементов во временной папке
        /// </summary>
        public static void CollectFamilyData(Document document)
        {
            Families.Clear();

            // фильтр для мультикатегории
            ElementMulticategoryFilter mcFilter = new ElementMulticategoryFilter(new List<BuiltInCategory>
            {
                BuiltInCategory.OST_MechanicalEquipment,
                BuiltInCategory.OST_GenericModel
            });

            var col = new FilteredElementCollector(document)
                .WherePasses(mcFilter)
                .WhereElementIsElementType()
                .ToList();

            col.RemoveAt(0);

            var fSymb = col.Cast<FamilySymbol>()
                .Select(symb => symb.Family.Id)
                .Distinct()
                .ToList();

            List<Family> families = new List<Family>();
            foreach (var item in fSymb)
            {
                families.Add((Family)document.GetElement(item));
            }

            Families.AddRange(GetFamiliesElements(families));
        }

        private static IEnumerable<FamilyDto> GetFamiliesElements(IEnumerable<Family> families)
        {
            var familyDtos = new List<FamilyDto>();

            foreach (var family in families)
            {
                // удалить?
                if (family.Name.Contains("Standart") ||
                    family.Name.Contains("Mullion") ||
                    family.Name.Contains("Tag"))
                    continue;

                var familySymbolDtos = family.GetFamilySymbolIds()
                    .Select(familySymbolId => family.Document.GetElement(familySymbolId))
                    .Cast<FamilySymbol>()
                    .Where(familySymbol => familySymbol is not null)
                    .Select(familySymbol => new FamilySymbolDto
                    {
                        Name = familySymbol.Name,
                        ImagePath = CreateImageForFamilySymbol(familySymbol),
                        FamilySymbol = familySymbol
                    })
                    .ToArray();

                if (familySymbolDtos.Any() == false) // зачем?
                    continue; 

                var familyDto = new FamilyDto
                {
                    Name = family.Name ?? "NO FAMILY NAME",
                    Symbols = familySymbolDtos
                };

                familyDtos.Add(familyDto);
            }

            return familyDtos;
        }

        private static string? CreateImageForFamilySymbol(FamilySymbol familySymbol)
        {
            var imageFilePath = FileService.GetFullPath($"{familySymbol.Name}.bmp");

            if (File.Exists(imageFilePath))
                return imageFilePath; //?

            var image = familySymbol.GetPreviewImage(DefaultFamilySymbolImageSize);

            //не у всех есть картинка, для таких возвращяем дефолтную картинку
            if (image is null)
                return DefaultFamilySymbolImagePath;

            using var file = new FileStream(imageFilePath, FileMode.Create, FileAccess.Write); // для чего using?

            image.Save(file, ImageFormat.Bmp);

            return imageFilePath;
        }
    }
}