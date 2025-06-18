using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbationDaysApp
{
    public class LoanDetailsModel
    {
        public decimal OriginalLoanAmount { get; set; }
        public decimal CurrentLoanAmount { get; set; }
        public decimal MonthlyEmi { get; set; }
        public DateTime StartDate { get; set; }
        public int MonthsPaid { get; set; }
        public int MonthsRemaining { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalPrincipalPaid { get; set; }
        public decimal TotalInterestPaid { get; set; }
        public decimal FutureFixedTotal { get; set; }
        public decimal[] VariableRates { get; set; } = Array.Empty<decimal>();
        public decimal RemainingPrincipal { get; set; }
        // Optionally, add the interest rate if needed for calculations:
        public double AnnualInterestRate { get; set; }
    }
}
