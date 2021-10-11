using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OnLoanApprovedForExternal
{
    public class Loan
    {
        public int CreditorAssigned_ID { get; set; }
        public int LoanApplication_Status { get; set; }
        public string LoanApplication_BankerComment { get; set; }
        public int External_ID { get; set; }
        public string CreditorCallBackURL { get; set; }


       
    }
}
