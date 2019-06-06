using System;
using System.Collections.Generic;
using System.Linq;
using SalesScreen.CaseInterview.Models;
using SalesScreen.CaseInterview.Services;

namespace SalesScreen.CaseInterview
{
    public class BankAccount
    {
        private readonly int AccountId;
        private AccountInfo _accountInfo;
        private List<Transaction> _transactionInfo;

        public BankAccount(int accountId)
        {
            AccountId = accountId;
        }

        public double GetAvailableFunds()
        {
            return _accountInfo.Balance + _accountInfo.Credit;
        }

        public void GetTransactionInfo(int subtractDays)
        {
            DateTimeOffset dateOffset = DateTimeOffset.Now.AddDays(-subtractDays);
            double total = _transactionInfo.Where(transaction => transaction.Date > dateOffset).Sum(transaction => transaction.Amount);
            int count = _transactionInfo.Where(transaction => transaction.Date > dateOffset).Count();
            Console.WriteLine($"Total sum of transactions for this user: {total}");
            Console.WriteLine("Average transaction sum: {0:0.00}", total / count);
        }

        #region API

        public void FetchAccountInfo()
        {
            _accountInfo = BankService.GetAccountInfo(AccountId);
        }
        public void FetchTransactionInfo()
        {
            _transactionInfo = BankService.GetTransactions(AccountId);
        }

        #endregion
    }
}