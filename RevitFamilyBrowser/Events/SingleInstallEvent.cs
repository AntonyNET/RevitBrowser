using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace zRevitFamilyBrowser.Events
{
    public class SingleInstallEvent : IExternalEventHandler
    {
        public void Execute(UIApplication uiapp)
        {
            FamilySymbol historySymbol = null; //TODO
            uiapp.ActiveUIDocument.PostRequestForElementTypePlacement(historySymbol);
        }

        public string GetName()
        {
            return "External Event";
        }
    }
}