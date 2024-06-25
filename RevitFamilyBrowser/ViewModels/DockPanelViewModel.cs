using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using zRevitFamilyBrowser.Models;
using zRevitFamilyBrowser.Services;

namespace zRevitFamilyBrowser.ViewModels
{
    public class DockPanelViewModel : INotifyPropertyChanged
    {
        public List<FamilyDto> Families { get; set; }

        public DockPanelViewModel()
        {
            Families = FamilyService.Families;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}