using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using SalesScreen.CaseInterview.Models;
using SalesScreen.CaseInterview.Services;

namespace SalesScreen.CaseInterview
{
    public class BankAccount
    {
        private readonly int AccountId;
        private AccountInfo _accountInfo;
        private List<Transaction> _transactionInfo;
        private List<Category> _categoryInfo;
        private List<CategoryMonthlyBudget> _monthlyBudgetInfo;

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
        public double GetMonthlyTransactionsForCategory(int month, int category)
        {
            double totalSumOfAllTransactions = 0;
            foreach (var transaction in _transactionInfo)
            {
                if (month == transaction.Date.Month)
                {
                    if (transaction.CategoryId == category + 1)
                    {
                        totalSumOfAllTransactions += transaction.Amount;
                    }
                }
            }
            return totalSumOfAllTransactions;
        }

        public void GetCategories()
        {
            foreach (var category in _categoryInfo)
            {
                Console.WriteLine(category.Name);
            }
        }

        public void GetMonthlyBudgetForUser(int amountOfMonths)
        {
            List<Category> categoryList = new List<Category>();
            foreach (var Category in _categoryInfo)
            {
                categoryList.Add(Category);
            }
            /* A full month is defined by the last month, so 
            the way I understood it is that once the month is completed,
            the transactions are added. I tried to simulate the example
            that is listed in the readme.md on github, and that's why
            the months are listed in the order that they are.
            */
            for (int monthIndex = amountOfMonths; monthIndex > 0; monthIndex--)
            {
                DateTime date = DateTime.Now.AddMonths(-monthIndex);
                int month = date.Month;
                double expenditure = 0;
                var budgetCategory = "";
                double difference = 0;
                foreach (var budgetItem in _monthlyBudgetInfo)
                {
                    expenditure = GetMonthlyTransactionsForCategory(month, budgetItem.CategoryId);
                    budgetCategory = categoryList[budgetItem.CategoryId - 1].Name;
                    difference = budgetItem.Amount - expenditure;
                    Console.Write(date.ToString("MMMM", new CultureInfo("en-GB")));
                    if (difference < 0)
                    {
                        Console.Write($" - {budgetCategory} : {difference}\n");
                    }
                    else
                    {
                        Console.Write($" - {budgetCategory} : +{difference}\n");
                    }
                }
            }
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

        public void FetchCategoryInfo()
        {
            _categoryInfo = BankService.GetCategories();
        }

        public void FetchMontlhyBudgetForUser()
        {
            _monthlyBudgetInfo = BankService.GetCategoryMonthlyBudgets(AccountId);
        }

        #endregion
    }
}