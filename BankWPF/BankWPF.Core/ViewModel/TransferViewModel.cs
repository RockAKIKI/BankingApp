using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

namespace BankWPF.Core
{
    /// <summary>
    /// The View Model for a transfer page
    /// </summary>
    public class TransferViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The user's input value
        /// </summary>
        public string InputValue { get; set; }

        /// <summary>
        /// The user to give money to
        /// </summary>
        public string InputUser { get; set; }

        /// <summary>
        /// Current user's balance converted to string to display
        /// </summary>
        public string BalanceString { get; set; } = "0";

        /// <summary>
        /// A flag indicating if the transfer command is running
        /// </summary>
        public bool TransferIsRunning { get; set; }
        
        /// <summary>
        /// A flag indicating if the not integer number error should be shown
        /// </summary>
        public bool ErrorNotInteger { get; set; }

        /// <summary>
        /// A flag indicating if the not enough money error should be shown
        /// </summary>
        public bool ErrorNotEnoughMoney { get; set; }

        /// <summary>
        /// A flag indicating if the wrong value error should be shown
        /// </summary>
        public bool ErrorWrongValue { get; set; }

        /// <summary>
        /// A flag indicating if the user was not found
        /// </summary>
        public bool ErrorUserNotFound { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to deposit the value
        /// </summary>
        public ICommand TransferCommand { get; set; }

        /// <summary>
        /// The command to change current page to the main page
        /// </summary>
        public ICommand ToMainPageCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransferViewModel()
        {
            // Create commands
            TransferCommand = new RelayParameterizedCommand(async (parameter) => await TransferAsync(parameter));
            ToMainPageCommand = new RelayCommand(async () => await ChangeToMainPageAsync());

            // Set BalanceString to actual account's balance after some delay
            Task.Run(async () =>
            {
                await Task.Delay(300);
                BalanceString = BankAccount.UserAccount.Balance.ToString();
            });
        }

        #endregion

        #region Procedures

        /// <summary>
        /// Attempts to transfer money to specified user
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
        /// <returns></returns>
        public async Task TransferAsync(object parameter)
        {
            // This function will run only if TransferIsRunning is false
            await RunCommandAsync(() => TransferIsRunning, async () =>
            {
                await Task.Delay(500);

                // Reset error messages
                ErrorNotInteger = false;
                ErrorNotEnoughMoney = false;
                ErrorWrongValue = false;
                ErrorUserNotFound = false;

                int valueToTransfer = 0;

                // Try to parse user's input from textbox to integer
                if (!Int32.TryParse(this.InputValue, out valueToTransfer))
                {
                    // If failed, output error and return
                    ErrorNotInteger = true;
                    return;
                }
                // If value is equal or less than 0, output error and return
                if (valueToTransfer <= 0)
                {
                    // Can't transfer negative or null value
                    ErrorWrongValue = true;
                    return;
                }
                // If user balance is 0, output error and return
                if (BankAccount.UserAccount.Balance == 0)
                {
                    // Can't do anything with 0 balance
                    ErrorNotEnoughMoney = true;
                    return;
                }

                // Transfer value - listen for output
                string output = BankAccount.UserAccount.Transfer(valueToTransfer, InputUser);

                // If output isnt Success, then user was not found
                if (output != "Success") ErrorUserNotFound = true;

                // TODO: Inform user about successful operation

                // Update new balance
                BalanceString = BankAccount.UserAccount.Balance.ToString();
            });
        }

        /// <summary>
        /// Takes the user to the main page
        /// </summary>
        /// <returns></returns>
        public async Task ChangeToMainPageAsync()
        {
            // Go to transfer page
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Main);

            await Task.Delay(1);
        }

        #endregion
    }
}
