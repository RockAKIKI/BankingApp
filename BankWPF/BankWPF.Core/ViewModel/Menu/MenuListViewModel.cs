using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BankWPF.Core
{
    /// <summary>
    /// A view model for the overview menu list
    /// </summary>
    public class MenuListViewModel : BaseViewModel
    {
        /// <summary>
        /// The menu list items for the list
        /// </summary>
        public static ObservableCollection<MenuListItemViewModel> Items { get; set; }  
    }
}
