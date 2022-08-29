using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BankWPF.Core
{
    /// <summary>
    /// The View Model for a login screen
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The user's nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        public bool LoginIsRunning { get; set; }

        /// <summary>
        /// A flag indicating if any error has occured during logging in
        /// </summary>
        public bool ErrorOccured { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// The command to register for a new account
        /// </summary>
        public ICommand RegisterCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            // Create commands
            LoginCommand = new RelayParameterizedCommand(async (parameter) => await LoginAsync(parameter));
            RegisterCommand = new RelayCommand(async () => await RegisterAsync());
        }

        #endregion

        #region Procedures

        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
        /// <returns></returns>
        public async Task LoginAsync(object parameter)
        {
            // This function will run only if LoginIsRunning is false
            await RunCommandAsync(() => LoginIsRunning, async () =>
            {
                await Task.Delay(1000);

                /*
                    IMPORTANT: Never store unsecure password in variable like this
                    string accountPassword = (parameter as IHavePassword).SecurePassword.Unsecure();
                    We have to pass it to function without variables
                */

                // Try to log in
                bool isAccountFound = LogIntoAccount(this.Nickname, (parameter as IHavePassword).SecurePassword.Unsecure());

                // If account was found
                if (isAccountFound)
                {
                    // Go to main page
                    IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Main);
                }
                else
                {
                    // Account wasnt found, output error
                    ErrorOccured = true;
                }
            });
        }

        /// <summary>
        /// Takes the user to the register page
        /// </summary>
        /// <returns></returns>
        public async Task RegisterAsync()
        {
            // Go to register page
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Register);

            await Task.Delay(1);
        } 

        #endregion

        #region Helpers

        /// <summary>
        /// Try to log in with user's data, if failed returns false
        /// </summary>
        /// <param name="login">Account login</param>
        /// <param name="password">Account password</param>
        /// <returns></returns>
        private bool LogIntoAccount(string login, string password)
        {
            // Set webservice's url and parameters we want to send
            string URI = "http://stacjapogody.lo2przemysl.edu.pl/bank/loggingin/index.php";
            string myParameters = $"login={ login }&password={ password }";

            string result = string.Empty;

            // Send request to webservice
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                result = wc.UploadString(URI, myParameters);
            }

            // If user wasnt found, return false
            if (result == "Not found")
                return false;

            // We have user's data, lets put this into variables
            string[] accountArray = result.Split('/');
            int id = Int32.Parse(accountArray[0]);
            string name = accountArray[1];
            int balance = Int32.Parse(accountArray[2]);

            // Make new instance of BankAccount with user's data
            BankAccount.UserAccount = new BankAccount(balance, id, name);

            // Download transactions history
            BankAccount.UserAccount.DownloadTransactionsHistory(id);

            // Return that we have found user
            return true;

        }

        #endregion
    }
}
