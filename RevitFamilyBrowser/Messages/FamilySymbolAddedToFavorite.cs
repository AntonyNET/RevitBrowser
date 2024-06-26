using CommunityToolkit.Mvvm.Messaging.Messages;
using zRevitFamilyBrowser.ViewModels;

namespace zRevitFamilyBrowser.Messages
{
    public class FamilySymbolAddedToFavorite : ValueChangedMessage<FamilySymbolViewModel>
    {
        public FamilySymbolAddedToFavorite(FamilySymbolViewModel familySymbolViewModel) : base(familySymbolViewModel) 
        { 
        }
    }
}