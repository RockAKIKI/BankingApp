using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;

namespace BankWPF.Core
{
    /// <summary>
    /// The design-time data for a <see cref="MenuListViewModel"/>
    /// </summary>
    public class MenuListDesignModel : MenuListViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static MenuListDesignModel Instance => new MenuListDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MenuListDesignModel()
        {
            Items = new ObservableCollection<MenuListItemViewModel>();
        }

        private void AddTransactionsToList(string result)
        {
            // Split rows
            string[] rowArray = result.Split('|');

            // Get the amount of transactions (-1 from last empty row)
            int amount = rowArray.GetLength(0) - 1;

            for (int i = 1; i < amount; i++)
            {
                // Prepare data
                // Split original row
                string[] dataArray = rowArray[i].Split('/');
                // The transaction's payment way
                string TransactionMethod = dataArray[0];
                // The transaction's value (positive or negative - depends on payment way)
                string TransactionValue = TransactionMethod == "Deposit" ? dataArray[1] : "-" + dataArray[1];
                // The transaction's message
                string TransactionMessage = dataArray[2];
                // The transaction's date
                string TransactionDate = dataArray[3];

                Items.Add(
                    new MenuListItemViewModel
                    {
                        Value = TransactionValue,
                        DWLetter = TransactionMethod == "Deposit" ? "D" : "W",
                        Message = TransactionMessage,
                        ColorStringRGB = TransactionMethod == "Deposit" ? "00d405" : "fe4503",
                        Date = TransactionDate
                    }
                    );
            }
        }

        #endregion
    }
}
