using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using zRevitFamilyBrowser.Models;
using Autodesk.Revit.UI;
using zRevitFamilyBrowser.Events;

namespace zRevitFamilyBrowser.ViewModels
{
    public class FamilySymbolViewModel : ObservableObject
    {
        private readonly ExternalEvent _externalEvent;
        public FamilySymbolDto FamilySymbolDto { get; set; }
        public ICommand RemoveFromFavoriteCommand { get; set; }
        public ICommand AddToFavoriteCommand { get; set; }
        public ICommand InstallSymbolCommand { get; set; }

        public bool CanAddToFavorite => FamilySymbolDto.IsFavorite == false;

        public FamilySymbolViewModel(FamilySymbolDto familySymbolDto)
        {
            FamilySymbolDto = familySymbolDto;
            RemoveFromFavoriteCommand = new RelayCommand(RemoveFromFavorite);
            AddToFavoriteCommand = new RelayCommand(AddToFavorite);
            InstallSymbolCommand = new RelayCommand(InstallSymbol);

            var singleInstallEvent = new SingleInstallEvent(FamilySymbolDto.FamilySymbol);
            _externalEvent = ExternalEvent.Create(singleInstallEvent);
        }

        private void RemoveFromFavorite()
        {
            FamilySymbolDto.IsFavorite = false;
            OnPropertyChanged(nameof(FamilySymbolDto));
            OnPropertyChanged(nameof(CanAddToFavorite));
        }

        private void AddToFavorite()
        {
            FamilySymbolDto.IsFavorite = true;
            OnPropertyChanged(nameof(FamilySymbolDto));
            OnPropertyChanged(nameof(CanAddToFavorite));
        }

        private void InstallSymbol()
        {
            _externalEvent.Raise();
        }
    }
}