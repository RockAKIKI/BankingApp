using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace BankWPF.Core
{
    /// <summary>
    /// A class that will handle account data
    /// </summary>
    public class BankAccount
    {
        #region Singleton

        /// <summary>
        /// The user's account instance
        /// </summary>
        public static BankAccount UserAccount { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Current balance in our account
        /// </summary>
        public int Balance { get; set; } = 0;

        /// <summary>
        /// Account ID
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Account name - login/nickname
        /// </summary>
        public string Name { get; private set; }

        #endregion

        #region Private Structs

        /// <summary>
        /// A pair of balance + id to return by methods
        /// </summary>
        private struct BalanceIdPair
        {
            public int balance;
            public int id;

            public BalanceIdPair(int i, int b)
            {
                balance = b;
                id = i;
            }
        }

        #endregion

        #region Constructor

        public BankAccount(int b, int n, string name)
        {
            Balance = b;
            Number = n;
            Name = name;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deposits the specified value
        /// </summary>
        /// <param name="value">The value user wants to deposit</param>
        public void Deposit(int value)
        {
            // Add value to balance
            Balance += value;

            // Upload new transaction to the database
            UploadNewTransaction(Number, "Deposit", value, "Deposit");

            // Update current balance to the database
            UpdateBalance(Number, Balance);

            // Update side menu list
            UpdateTransactionsList();
        }

        /// <summary>
        /// Withdraw the specified value
        /// </summary>
        /// <param name="value">The value user wants to withdraw</param>
        public void Withdraw(int value)
        {
            // Check if value is greater than balance, if yes, withdraw the whole cash
            if (value > Balance) value = Balance;

            // Substract value from balance
            Balance -= value;

            // Upload new transaction to the database
            UploadNewTransaction(Number, "Withdraw", value, "Withdraw");

            // Update current balance to database
            UpdateBalance(Number, Balance);

            // Update side menu list
            UpdateTransactionsList();
        }

        /// <summary>
        /// Transfer the specified value
        /// </summary>
        /// <param name="value">The value user wants to transfer</param>
        /// <param name="login">The user to transfer to</param>
        public string Transfer(int value, string login)
        {
            // Check if value is greater than balance, if yes, transfer the whole cash
            if (value > Balance) value = Balance;

            // Try to transfer money to specified account
            if (!TransferMoneyTo(value, login)) return "User not found";

            // Substract value from balance
            Balance -= value;

            // Update current balance to database
            UpdateBalance(Number, Balance);

            // Update side menu list
            UpdateTransactionsList();

            // Successful operation
            return "Success";
        }


        public void DownloadTransactionsHistory(int number)
        {
            // Set webservice's url and parameters we want to send
            string URI = "http://stacjapogody.lo2przemysl.edu.pl/bank/showhistory/index.php?";
            string myParameters = $"id={ number }";

            string result = string.Empty;
            // Send request to webservice
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                result = wc.UploadString(URI, myParameters);
            }

            // If something went wrong, output error
            if (result == "Found: 0") return;

            // Add transactions to list
            AddTransactionsToList(result);
        }

        #endregion

        #region Private Helpers

        private void UploadNewTransaction(int id,string paymentway, int value, string message)
        {
            // Set webservice's url and parameters we want to send
            string URI = "http://stacjapogody.lo2przemysl.edu.pl/bank/savetohistory/index.php?";
            string myParameters = $"id={ id }&depOrWit={ paymentway }&value={ value }&message={ message }";

            string result = string.Empty;
            // Send request to webservice
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                result = wc.UploadString(URI, myParameters);
            }

            // If something went wrong, output error
            if (result != "Succeed") Debugger.Break();
        }

        /// <summary>
        /// Balance in database updater
        /// </summary>
        private void UpdateBalance(int id, int balance)
        {
            // Set webservice's url and parameters we want to send
            string URI = "http://stacjapogody.lo2przemysl.edu.pl/bank/uploaddata/index.php?";
            string myParameters = $"id={ id }&balance={ balance }";

            string result = string.Empty;
            // Send request to webservice
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                result = wc.UploadString(URI, myParameters);
            }

            // If something went wrong, output error
            if (result != "Succeed") Debugger.Break();
        }

        /// <summary>
        /// Balance in database updater
        /// </summary>
        private bool TransferMoneyTo(int value, string login)
        {
            // Get user balance and id by its login
            var userData = DownloadUserBalance(login);

            // If id equals 0, user not found - return false
            if (userData.id == 0) return false;

            // Add value to specified user balance
            userData.balance += value;

            // Upload new transaction to the database about user's withdraw
            UploadNewTransaction(Number, "Withdraw", value, login);

            // Upload new transaction to the database about specified user deposit
            UploadNewTransaction(userData.id, "Deposit", value, Name);

            // Upload new balance
            UpdateBalance(userData.id, userData.balance);

            // Successful operation, return true
            return true;
        }

        private BalanceIdPair DownloadUserBalance(string login)
        {
            // Set webservice's url and parameters we want to send
            string URI = "http://stacjapogody.lo2przemysl.edu.pl/bank/downloadbalance/index.php?";
            string myParameters = $"login={ login }";

            string result = string.Empty;
            // Send request to webservice
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                result = wc.UploadString(URI, myParameters);
            }

            // If something went wrong, output error
            if (result == "Not found") Debugger.Break();

            // Split the result to balance and id
            string[] resultArray = result.Split('/');

            // Try to convert result balance to integer number
            int balance = 0;
            Int32.TryParse(resultArray[0], out balance);

            // Try to convert result id to integer number
            int id = 0;
            Int32.TryParse(resultArray[1], out id);

            // Make new pair of results
            var pair = new BalanceIdPair(id, balance);

            // Return the pair of balance and id
            return pair;
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

                MenuListViewModel.Items.Add(
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

        private void UpdateTransactionsList()
        {
            // Clear the list
            MenuListViewModel.Items.Clear();

            // Redownload data
            DownloadTransactionsHistory(Number);
        }

        #endregion
    }
}
