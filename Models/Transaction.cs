using System;

namespace SalesScreen.CaseInterview.Models
{
    public class Transaction
    {
        //TODO: Define the class properties
        public int Id { get; set; }
        public float Amount { get; set; }
        public int CategoryId { get; set; }
        public DateTime Date { get; set; }
    }
}