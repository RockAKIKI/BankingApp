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
    /// The View Model for a main page
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The user's input value
        /// </summary>
        public string InputValue { get; set; }

        /// <summary>
        /// Current user's balance converted to string to display
        /// </summary>
        public string BalanceString { get; set; } = "0";

        /// <summary>
        /// A flag indicating if the deposit command is running
        /// </summary>
        public bool DepositIsRunning { get; set; }

        /// <summary>
        /// A flag indicating if the withdraw command is running
        /// </summary>
        public bool WithdrawIsRunning { get; set; }

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

        #endregion

        #region Commands

        /// <summary>
        /// The command to deposit the value
        /// </summary>
        public ICommand DepositCommand { get; set; }

        /// <summary>
        /// The command to withdraw the value
        /// </summary>
        public ICommand WithdrawCommand { get; set; }

        /// <summary>
        /// The command to change the current page to transfer page
        /// </summary>
        public ICommand ToTransferPageCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainViewModel()
        {
            // Create commands
            DepositCommand = new RelayParameterizedCommand(async (parameter) => await DepositAsync(parameter));
            WithdrawCommand = new RelayParameterizedCommand(async (parameter) => await WithdrawAsync(parameter));
            ToTransferPageCommand = new RelayCommand(async () => await ChangeToTransferPageAsync());

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
        /// Attempts to deposit money to the user's account
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
        /// <returns></returns>
        public async Task DepositAsync(object parameter)
        {
            // This function will run only if DepositIsRunning is false
            await RunCommandAsync(() => DepositIsRunning, async () =>
            {
                await Task.Delay(500);

                // Reset error messages
                ErrorNotInteger = false;
                ErrorNotEnoughMoney = false;
                ErrorWrongValue = false;

                int valueToDeposit = 0;

                // Try to parse user's input from textbox to integer
                if (!Int32.TryParse(this.InputValue, out valueToDeposit))
                {
                    // If failed, output error and return
                    ErrorNotInteger = true;
                    return;
                }
                // If value is equal or less than 0, output error and return
                if (valueToDeposit <= 0)
                {
                    // Can't transfer negative or null value
                    ErrorWrongValue = true;
                    return;
                }

                // Deposit value
                BankAccount.UserAccount.Deposit(valueToDeposit);

                // Update new balance
                BalanceString = BankAccount.UserAccount.Balance.ToString();

                // TODO: Inform user about successful operation
            });
        }

        /// <summary>
        /// Attempts to withdraw money from user's account
        /// </summary>
        /// <returns></returns>
        public async Task WithdrawAsync(object parameter)
        {
            // This function will run only if WithdrawIsRunning is false
            await RunCommandAsync(() => WithdrawIsRunning, async () =>
            {
                await Task.Delay(500);

                // Reset error messages
                ErrorNotInteger = false;
                ErrorNotEnoughMoney = false;
                ErrorWrongValue = false;

                int valueToWithdraw = 0;

                // Try to parse user's input from textbox to integer
                if (!Int32.TryParse(this.InputValue, out valueToWithdraw))
                {
                    // If failed, output error and return
                    ErrorNotInteger = true;
                    return;
                }
                // If value is equal or less than 0, output error and return
                if (valueToWithdraw <= 0)
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

                // Withdraw value
                BankAccount.UserAccount.Withdraw(valueToWithdraw);

                // Update new balance
                BalanceString = BankAccount.UserAccount.Balance.ToString();

                // TODO: Inform user about successful operation
            });
        }

        /// <summary>
        /// Takes the user to the transfer page
        /// </summary>
        /// <returns></returns>
        public async Task ChangeToTransferPageAsync()
        {
            // Go to transfer page
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Transfer);

            await Task.Delay(1);
        }

        #endregion
    }
}
