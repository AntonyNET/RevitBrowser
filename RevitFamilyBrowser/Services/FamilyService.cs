using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using zRevitFamilyBrowser.Models;
using zRevitFamilyBrowser.ViewModels;
using Document = Autodesk.Revit.DB.Document;

namespace zRevitFamilyBrowser.Services
{
    public static class FamilyService
    {
        public static List<FamilyDto> Families;
        private static readonly Size DefaultFamilySymbolImageSize;
        private static readonly string DefaultFamilySymbolImagePath;

        static FamilyService()
        {
            Families = new List<FamilyDto>();

            DefaultFamilySymbolImageSize = new Size(50, 50);
            DefaultFamilySymbolImagePath = FileService.GetFullPath($"{nameof(Properties.Resources.RevitLogo)}.bmp");

            Properties.Resources.RevitLogo.Save(DefaultFamilySymbolImagePath);
        }

        /// <summary>
        /// Создание изображений для всех элементов во временной папке
        /// </summary>
        public static void CollectFamilyData(Document document)
        {
            Families.Clear();
            
            var allowedFamilyCategoryIds = new[]
            {
                Category.GetCategory(document, BuiltInCategory.OST_MechanicalEquipment).Id,
                Category.GetCategory(document, BuiltInCategory.OST_GenericModel).Id
            };
         
            var families = new FilteredElementCollector(document)
                .OfClass(typeof(Family))
                .Cast<Family>()
                .Where(family => IsFamilyAllowed(family, allowedFamilyCategoryIds))
                .ToList();

            Families.AddRange(GetFamiliesElements(families));
        }

        private static bool IsFamilyAllowed(Family family, ElementId[] allowedFamilyCategoryIds)
        {
            if (family.Name.Contains("Standart")
                || family.Name.Contains("Mullion")
                || family.Name.Contains("Tag"))
            {
                return false;
            }

            return allowedFamilyCategoryIds.Contains(family.FamilyCategoryId);
        }

        private static IEnumerable<FamilyDto> GetFamiliesElements(IEnumerable<Family> families)
        {
            var familyDtos = new List<FamilyDto>();

            foreach (var family in families)
            {
                var familySymbolDtos = family.GetFamilySymbolIds()
                    .Select(familySymbolId => family.Document.GetElement(familySymbolId))
                    .Cast<FamilySymbol>()
                    .Where(familySymbol => familySymbol is not null)
                    .Select(familySymbol => new FamilySymbolViewModel(new FamilySymbolDto
                    {
                        Name = familySymbol.Name,
                        ImagePath = CreateImageForFamilySymbol(familySymbol),
                        FamilySymbol = familySymbol
                    }))
                    .ToArray();

                //Если семейство не содержит никаких символов, то пропускает это семейство, так как отображать будет нечего и не зачем загрязнять UI
                if (familySymbolDtos.Any() == false)
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
                return imageFilePath;

            var image = familySymbol.GetPreviewImage(DefaultFamilySymbolImageSize);

            //не у всех есть картинка, для таких возвращяем дефолтную картинку
            if (image is null)
                return DefaultFamilySymbolImagePath;

            using var file = new FileStream(imageFilePath, FileMode.Create, FileAccess.Write);

            image.Save(file, ImageFormat.Bmp);

            return imageFilePath;
        }
    }
}