using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace zRevitFamilyBrowser.Models
{
    public static class Tools
    {

        private static string GetFamiliesElements(FilteredElementCollector elementCollector)
        {
            var temp = string.Empty;

            foreach (var element in elementCollector)
            {
                if (element.Name.Contains("Standart") ||
                    element.Name.Contains("Mullion") ||
                    element.Name.Contains("Tag")) 
                    continue;
                
                var family = element as Family;
                temp += element.Name;

                ISet<ElementId> familySymbolId = family.GetFamilySymbolIds();

                foreach (ElementId id in familySymbolId)
                {
                    var symbol = family.Document.GetElement(id) as FamilySymbol;
                    if (symbol != null) temp += "#" + symbol.Name;
                }
                temp += "\n";
            }
            return temp;
        }

        public static void CollectFamilyData(Document document)
        {
            Properties.Settings.Default.CollectedData = string.Empty;

            var worksets = new FilteredWorksetCollector(document)
                .OfKind(WorksetKind.FamilyWorkset)
                .ToWorksets();

            if (worksets.Count == 0)
            {
                var elementCollector = new FilteredElementCollector(document).OfClass(typeof(Family));

                Properties.Settings.Default.CollectedData = GetFamiliesElements(elementCollector);
            }
            else
            {
                var temp = string.Empty;

                foreach (var workset in worksets)
                {
                    if (workset.IsEditable)
                    {
                        var elementWorksetFilter = new ElementWorksetFilter(workset.Id, false);
                        var elementCollector = new FilteredElementCollector(document).OfClass(typeof(Family)).WherePasses(elementWorksetFilter);

                        temp += GetFamiliesElements(elementCollector);
                    }
                }

                Properties.Settings.Default.CollectedData = temp;
            }
        }
    }
}
