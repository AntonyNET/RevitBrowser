using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using zRevitFamilyBrowser.Models;
using zRevitFamilyBrowser.Services;

namespace zRevitFamilyBrowser.ViewModels
{
    public class DockPanelViewModel : ObservableObject
    {
        public List<FamilyDto> Families { get; set; }

        public DockPanelViewModel()
        {
            Families = FamilyService.Families;
        }
    }
}