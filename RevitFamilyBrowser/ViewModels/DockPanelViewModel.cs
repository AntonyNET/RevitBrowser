using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using zRevitFamilyBrowser.Messages;
using zRevitFamilyBrowser.Models;
using zRevitFamilyBrowser.Services;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace zRevitFamilyBrowser.ViewModels
{
    public class DockPanelViewModel : ObservableObject
    {
        public List<FamilyDto> Families { get; set; }
        public ObservableCollection<FamilySymbolViewModel> FavoriteSymbols { get; set; }
        
        public ICommand SaveFavoritesCommand { get; set; }
        public ICommand LoadFavoritesCommand { get; set; }

        public DockPanelViewModel()
        {
            Families = FamilyService.Families;
            FavoriteSymbols = new ObservableCollection<FamilySymbolViewModel>();
            SaveFavoritesCommand = new RelayCommand(SaveFavorites);
            LoadFavoritesCommand = new RelayCommand(LoadFavorites);

            WeakReferenceMessenger.Default.Register<DockPanelViewModel, FamilySymbolAddedToFavorite>(this, (r, m) => r.Receive(m)); // что значит?
            WeakReferenceMessenger.Default.Register<DockPanelViewModel, FamilySymbolRemovedFromFavorite>(this, (r, m) => r.Receive(m)); // что значит?
        }

        private void LoadFavorites()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "MyFavoriteSet1";
            dlg.DefaultExt = ".json";
            dlg.Filter = "All Files|*.*";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                using (StreamReader file = File.OpenText(dlg.FileName))
                {
                    List<Dictionary<string, string>> fav
                        = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(File.ReadAllText(dlg.FileName));

                    FavoriteSymbols.Clear();

                    foreach (var favoriteItem in fav)
                    {
                        FavoriteSymbols.Add(FamilyService.GetFavoriteFamilySymbolFromProject(favoriteItem));
                    }

                    // TODO:
                    // Упростить определение поиска

                };
            }
        }

        private void SaveFavorites()
        {
            var data = FavoriteSymbols.Select(symb => symb.FamilySymbolDto);

            // Добавлено свойство Family (название семейства)
            // Свойства ImagePath, FamilySymbol, IsFavorite игнорируются при сериализации
            string json = JsonConvert.SerializeObject(data);

            // Запись в файл
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "MyFavoriteSet1";
            dlg.DefaultExt = ".json";
            dlg.Filter = "All Files|*.*";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                File.WriteAllText(dlg.FileName, json);
            }
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