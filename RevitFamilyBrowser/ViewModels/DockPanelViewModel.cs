using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using zRevitFamilyBrowser.Messages;
using zRevitFamilyBrowser.Models;
using zRevitFamilyBrowser.Services;

namespace zRevitFamilyBrowser.ViewModels
{
    public class DockPanelViewModel : ObservableObject
    {
        public List<FamilyDto> Families { get; set; }
        public ObservableCollection<FamilySymbolViewModel> FavoriteSymbols { get; set; }

        public DockPanelViewModel()
        {
            Families = FamilyService.Families;
            FavoriteSymbols = new ObservableCollection<FamilySymbolViewModel>();

            WeakReferenceMessenger.Default.Register<DockPanelViewModel, FamilySymbolAddedToFavorite>(this, (r, m) => r.Receive(m));
            WeakReferenceMessenger.Default.Register<DockPanelViewModel, FamilySymbolRemovedFromFavorite>(this, (r, m) => r.Receive(m));
        }

        public void Receive(FamilySymbolAddedToFavorite message)
        {
            FavoriteSymbols.Add(message.Value);
        }

        public void Receive(FamilySymbolRemovedFromFavorite message)
        {
            FavoriteSymbols.Remove(message.Value);
        }
    }
}