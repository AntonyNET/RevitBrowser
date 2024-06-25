using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace zRevitFamilyBrowser.Events
{
    public class SingleInstallEvent : IExternalEventHandler
    {
        private readonly FamilySymbol _familySymbol;

        public SingleInstallEvent(FamilySymbol familySymbol)
        {
            _familySymbol = familySymbol;
        }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                uiapp.ActiveUIDocument.PostRequestForElementTypePlacement(_familySymbol);
            }
            catch (Exception e)
            {
                TaskDialog.Show("Ошибка", e.Message);
            }
        }

        public string GetName()
        {
            return "External Event";
        }
    }
}