namespace BankWPF.Core
{
    /// <summary>
    /// A view model for each menu list item
    /// </summary>
    public class MenuListItemViewModel : BaseViewModel
    {
        /// <summary>
        /// The value deposited or withdrawed in transaction
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The message added by user to transaction
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The 'D' or 'W' letter indicates if it was Deposit or Withdraw procedure
        /// </summary>
        public string DWLetter { get; set; }

        /// <summary>
        /// The RGB values (in hex) for the background/foreground color of the LetterPicture/Value
        /// By default, green (00d405) for deposit or red (fe4503) for withdraw transactions
        /// </summary>
        public string ColorStringRGB { get; set; } 

        /// <summary>
        /// The date the transaction was done
        /// </summary>
        public string Date { get; set; }
    }
}
