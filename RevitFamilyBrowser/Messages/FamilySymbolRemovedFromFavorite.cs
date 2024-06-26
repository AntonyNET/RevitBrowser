using CommunityToolkit.Mvvm.Messaging.Messages;
using zRevitFamilyBrowser.ViewModels;

namespace zRevitFamilyBrowser.Messages
{
    public class FamilySymbolRemovedFromFavorite : ValueChangedMessage<FamilySymbolViewModel>
    {
        public FamilySymbolRemovedFromFavorite(FamilySymbolViewModel familySymbolViewModel) : base(familySymbolViewModel) 
        { 
        }
    }
}