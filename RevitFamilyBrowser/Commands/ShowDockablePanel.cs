using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using zRevitFamilyBrowser.Definitions;

namespace zRevitFamilyBrowser.Commands
{
    [Transaction(TransactionMode.Manual)]
    class ShowDockablePanel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            DockablePaneId dpid = new DockablePaneId(PanelIds.FamilyBrowserPanelId);
            DockablePane dp = commandData.Application.GetDockablePane(dpid);
            dp.Show();
            return Result.Succeeded;
        }
    }
}
