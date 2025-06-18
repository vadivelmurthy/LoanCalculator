using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProbationDaysApp
{
    public partial class LoanDetailsForm : Form
    {
        private readonly LoanDetailsModel _loan;

        // Controls assumed: rtbSummary, txtExtraPayment, btnSeeImpact, rtbImpact

        public LoanDetailsForm(LoanDetailsModel loan)
        {
            if (loan == null) throw new ArgumentNullException(nameof(loan));
            InitializeComponent();
            _loan = loan;

            this.Load += LoanDetailsForm_Load;
            btnSeeImpact.Click += BtnSeeImpact_Click;
        }

        private void LoanDetailsForm_Load(object sender, EventArgs e)
        {
            FillAndColorLoanSummary(rtbSummary);
            rtbImpact.Clear();
        }

        /// <summary>
        /// Populates the loan summary RichTextBox with colored loan details.
        /// </summary>
        private void FillAndColorLoanSummary(RichTextBox rtb)
        {
            rtb.Clear();
            rtb.Font = new Font("Consolas", 10);

            void AppendColored(string text, Color color, bool bold = false)
            {
                int start = rtb.TextLength;
                rtb.AppendText(text);
                rtb.Select(start, text.Length);
                rtb.SelectionColor = color;
                if (bold)
                    rtb.SelectionFont = new Font(rtb.Font.FontFamily, rtb.Font.Size, FontStyle.Bold);
                // Reset selection color/font
                rtb.Select(rtb.TextLength, 0);
                rtb.SelectionColor = rtb.ForeColor;
                if (bold)
                    rtb.SelectionFont = rtb.Font;
            }

            string padLine(string label, string value) => $"{label,-26}: {value}\n";

            AppendColored("========================================\n", Color.DarkCyan, true);
            AppendColored("         HOME LOAN DETAILS SUMMARY      \n", Color.DarkBlue, true);
            AppendColored("========================================\n", Color.DarkCyan, true);

            AppendColored(padLine("Original Loan Amount", $"€{_loan.OriginalLoanAmount:N2}"), Color.Black);
            AppendColored(padLine("Current Loan Amount", $"€{_loan.CurrentLoanAmount:N2}"), Color.Black);
            AppendColored(padLine("Fixed EMI", $"€{_loan.MonthlyEmi:N2}"), Color.Black);
            AppendColored(padLine("Start Date", $"{_loan.StartDate:dd MMM yyyy}"), Color.Black);

            AppendColored("------------------------------------------\n", Color.Gray);

            AppendColored(padLine("Months Paid", $"{_loan.MonthsPaid:N0}"), Color.Teal);
            AppendColored(padLine("Months Remaining", $"{_loan.MonthsRemaining:N0}"), Color.Teal);

            AppendColored("------------------------------------------\n", Color.Gray);

            // Green for total paid
            AppendColored(padLine("Total Paid", $"€{_loan.TotalPaid:N2}"), Color.MediumSeaGreen, true);


            string subPad(string label, string value) => $"{label,-23}: {value}\n";

            // Green for principal paid; Red for interest paid
            AppendColored(subPad("└ Principal Paid", $"€{_loan.TotalPrincipalPaid:N2}"), Color.SeaGreen);
            AppendColored(subPad("└ Interest Paid", $"€{_loan.TotalInterestPaid:N2}"), Color.IndianRed);


            // Violet for future fixed total
            AppendColored("------------------------------------------\n", Color.Gray);
            AppendColored(padLine($"Remaining @{_loan.AnnualInterestRate:F2}%", $"€{_loan.FutureFixedTotal:N2}"),
                Color.MediumVioletRed, true);

            rtb.AppendText("\n");

            var tableHeader = "----------- VARIABLE RATE PROJECTIONS -----------\n";
            var tableColumns = String.Format("{0,-8}{1,-15}{2,-18}\n", "Rate(%)", "EMI (Euro)", "Total Repayable");
            var tableSep = "--------------------------------------------------\n";

            int tstart = rtb.TextLength;
            rtb.AppendText(tableHeader + tableColumns + tableSep);
            rtb.Select(tstart, (tableHeader + tableColumns + tableSep).Length);
            rtb.SelectionFont = new Font(rtb.Font.FontFamily, rtb.Font.Size, FontStyle.Bold);
            rtb.SelectionColor = Color.Navy;

            // How many months remain after fixed period?
            int monthsAfterFixed = (_loan.MonthsPaid + _loan.MonthsRemaining) - 60;
            // But usually: int monthsAfterFixed = totalTermMonths - 60;
            // Here use original loan term minus fixed period:
            int totalTermMonths = _loan.MonthsPaid + _loan.MonthsRemaining; // or store separately
            monthsAfterFixed = Math.Max(0, totalTermMonths - 60);

            // What will be outstanding principal after first 60 payments?
            decimal futurePrincipal =
               GetPrincipalAfterPayments(_loan.OriginalLoanAmount,
                                         _loan.AnnualInterestRate,
                                         Math.Min(60, totalTermMonths),
                                         _loan.MonthlyEmi);

            foreach (var rate in _loan.VariableRates)
            {
                decimal newEmi = GetEMI(futurePrincipal, (double)rate / 100d / 12d, monthsAfterFixed);
                decimal totalRepayable = newEmi * monthsAfterFixed;

                string row = String.Format("{0,-8:0.00}{1,-15:N2}{2,-18:N2}\n",
                  rate, newEmi, totalRepayable);

                var clr = (rate == _loan.VariableRates.Min() ? Color.Green :
                         rate == _loan.VariableRates.Max() ? Color.Red : Color.DarkSlateBlue);
                AppendColored(row, clr);
            }

            AppendColored(tableSep + "\n", Color.Navy, true);
        }

        static decimal GetEMI(decimal principal, double monthlyRate, int nper)
        {
            if (monthlyRate <= 0 || nper <= 0) return 0m;
            double p = (double)principal;
            double emi = p * monthlyRate * Math.Pow(1 + monthlyRate, nper) / (Math.Pow(1 + monthlyRate, nper) - 1d);
            return (decimal)emi;
        }

        private void BtnSeeImpact_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtExtraPayment.Text.Trim(), out var extra) || extra < 0 || extra > 300)
            {
                MessageBox.Show("Enter a valid extra payment (max €300 per month).");
                return;
            }


            const int FIXED_MONTHS = 60; // Fixed period duration
            int totalTermMonths = _loan.MonthsPaid + _loan.MonthsRemaining;

            // Simulate first part: pay EMI+extra for up to FIXED_MONTHS or as many as left in loan
            int monthsToSimulateFixedPeriod =
                Math.Min(FIXED_MONTHS - _loan.MonthsPaid,
                         _loan.MonthsRemaining);

            decimal futurePrincipal =
               GetPrincipalAfterExtraPayments(_loan.CurrentLoanAmount,
                                              _loan.AnnualInterestRate,
                                              monthsToSimulateFixedPeriod,
                                              _loan.MonthlyEmi,
                                              extra);

            // Calculate baseline (no-extra) future principal
            decimal baselinePrincipal =
                GetPrincipalAfterExtraPayments(_loan.CurrentLoanAmount,
                                               _loan.AnnualInterestRate,
                                               monthsToSimulateFixedPeriod,
                                               _loan.MonthlyEmi,
                                               0m); // no extra!

            int monthsLeftAfterFixed =
               totalTermMonths - FIXED_MONTHS;

            rtbImpact.Clear();
            rtbImpact.Font = new Font("Consolas", 10); // Match summary font

            // Header
            AppendColored(rtbImpact,
                $"After paying an additional €{extra:N2} per month during fixed period ({FIXED_MONTHS} months @ {_loan.AnnualInterestRate:F2}%):\n",
                Color.DarkBlue, true);

            AppendColored(rtbImpact,
                $"Principal left after {FIXED_MONTHS} payments: €{futurePrincipal:N2}\n\n",
                Color.MediumVioletRed, true);

            AppendColored(rtbImpact, "Variable Rate Projections:\n\n", Color.Teal, true);

            // Table headers
            string tableHeader = String.Format("{0,-8}{1,-13}{2,-18}{3,-18}{4,-18}\n",
                "Rate(%)", "New EMI", "Tenure", "Total Repayable", "Savings vs No-Extra");
            string tableSep = "-----------------------------------------------------------------------------\n";

            AppendColored(rtbImpact, tableHeader, Color.Navy, true);
            AppendColored(rtbImpact, tableSep, Color.Gray);

            if (monthsLeftAfterFixed <= 0 || futurePrincipal <= 0m)
            {
                AppendColored(rtbImpact,
                    $"Loan paid off during fixed period!\n",
                    Color.SeaGreen,
                    true);
                return;
            }

            var minRate = _loan.VariableRates.Min();
            var maxRate = _loan.VariableRates.Max();

            foreach (var rate in _loan.VariableRates)
            {
                double monthlyVarRate = (double)rate / 100d / 12d;

                // WITH EXTRA payment scenario:
                decimal emiWithExtra = GetEMI(futurePrincipal, monthlyVarRate, monthsLeftAfterFixed);
                int tenureWithExtra = GetMonthsToRepay(futurePrincipal, emiWithExtra, (double)rate);
                decimal totalWithExtra = emiWithExtra * tenureWithExtra;

                string tenureStrWithExtra = $"{tenureWithExtra} mo ({tenureWithExtra / 12}y{tenureWithExtra % 12}m)";

                // NO EXTRA payment scenario:
                decimal emiNoExtra = GetEMI(baselinePrincipal, monthlyVarRate, monthsLeftAfterFixed);
                int tenureNoExtra = GetMonthsToRepay(baselinePrincipal, emiNoExtra, (double)rate);
                decimal totalNoExtra = emiNoExtra * tenureNoExtra;

                decimal savings = totalNoExtra - totalWithExtra;

                bool best = rate == minRate;
                bool worst = rate == maxRate;

                Color clrRow = best ? Color.Green : worst ? Color.Red : Color.Black;

                string rowLine =
                    String.Format("{0,-8:0.00}{1,-13:N2}{2,-18}{3,-18:N2}{4,-18:N2}\n",
                        rate,
                        emiWithExtra,
                        tenureStrWithExtra,
                        totalWithExtra,
                        savings);

                AppendColored(rtbImpact, rowLine, clrRow);
            }
            txtExtraPaymentPostFixed.Enabled = true;
            btnSeeImpactPostFixed.Enabled = true;
        }

        private void btnSeeImpactPostFixed_Click_1(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtExtraPaymentPostFixed.Text.Trim(), out var postExtra) || postExtra < 0)
            {
                MessageBox.Show("Enter a valid extra payment for post-fixed period.");
                return;
            }

            // Use principal at end of full fixed term
            const int FIXED_MONTHS = 60;

            decimal futurePrincipal =
                GetPrincipalAfterExtraPayments(_loan.CurrentLoanAmount,
                                                _loan.AnnualInterestRate,
                                                FIXED_MONTHS - _loan.MonthsPaid, // Remaining full fixed term
                                                _loan.MonthlyEmi,
                                                decimal.Parse(txtExtraPayment.Text.Trim()));

            int totalTermMonths = _loan.MonthsPaid + _loan.MonthsRemaining;

            int monthsLeftAfterFullFixed = totalTermMonths - FIXED_MONTHS;

            rtbImpact.Font = new Font("Consolas", 10);

            AppendColored(rtbImpact,
                 $"\n--- IMPACT OF EXTRA PAYMENT AFTER FIXED PERIOD ---\n",
                 Color.Teal, true);

            AppendColored(rtbImpact,
                $"After paying an additional €{txtExtraPayment.Text.Trim()} per month during ALL remaining fixed period and then €{postExtra:N2} per month during VARIABLE period:\n\n",
                Color.DarkBlue, true);

            AppendColored(rtbImpact,
                $"Principal at start of variable rate: €{futurePrincipal:N2}\n\n",
                Color.MediumVioletRed);

            AppendColored(rtbImpact,
                $"Principal left after {FIXED_MONTHS} payments: €{futurePrincipal:N2}\n\n",
                Color.MediumVioletRed, true);

            // Now variable rate projections but apply 'postExtra' in each variable-rate scenario:
            foreach (var rate in _loan.VariableRates)
            {
                double monthlyVarRate = (double)rate / 100d / 12d;

                decimal emiWithPostExtra =
                    GetEMI(futurePrincipal, monthlyVarRate, monthsLeftAfterFullFixed) + postExtra;

                int tenureWithPostExtra =
                    GetMonthsToRepay(futurePrincipal, emiWithPostExtra, (double)rate);

                decimal totalWithPostExtra =
                    emiWithPostExtra * tenureWithPostExtra;

                string tenureStrWithPost =
                    $"{tenureWithPostExtra} mo ({tenureWithPostExtra / 12}y{tenureWithPostExtra % 12}m)";

                decimal emiNoPostExtra = GetEMI(futurePrincipal, monthlyVarRate, monthsLeftAfterFullFixed);
                int tenureNoPostExtra = GetMonthsToRepay(futurePrincipal, emiNoPostExtra, (double)rate);
                decimal totalNoPostExtra = emiNoPostExtra * tenureNoPostExtra;

                decimal savingsVsBaseline = totalNoPostExtra - totalWithPostExtra;

                string rowLine =
                    String.Format("{0,-8:0.00}{1,-13:N2}{2,-18}{3,-18:N2}{4,-18:N2}\n",
                        rate,
                        emiWithPostExtra,
                        tenureStrWithPost,
                        totalWithPostExtra,
                        savingsVsBaseline);

                AppendColored(rtbImpact, rowLine, Color.Black);
            }
        }
        void AppendColored(RichTextBox rtb, string text, Color color, bool bold = false)
        {
            int pos = rtb.TextLength;
            rtb.AppendText(text);
            rtb.Select(pos, text.Length);
            rtb.SelectionColor = color;
            rtb.SelectionFont = new Font(rtb.Font.FontFamily, rtb.Font.Size,
                bold ? FontStyle.Bold : FontStyle.Regular);
            rtb.Select(rtb.TextLength, 0); // Reset selection
        }

        // Helper: Calculate outstanding principal after N payments at given rate/emi
        private decimal GetPrincipalAfterPayments(decimal initialPrincipal, double annualRate, int emiCount, decimal monthlyEmi)
        {
            double balance = (double)initialPrincipal;
            double monthlyRate = annualRate / 12.0 / 100.0;
            for (int i = 0; i < emiCount; i++)
            {
                double interest = balance * monthlyRate;
                double principalPaid = (double)monthlyEmi - interest;
                balance -= principalPaid;
                if (balance < 1e-2) break; // Loan paid off
            }
            return (decimal)balance;
        }

        // Simulate repayment with extra payment for N months
        private decimal GetPrincipalAfterExtraPayments(decimal initialPrincipal, double annualRate, int emiCount, decimal monthlyEmi, decimal extra)
        {
            double balance = (double)initialPrincipal;
            double monthlyRate = annualRate / 12.0 / 100.0;
            double payment = (double)monthlyEmi + (double)extra;

            for (int i = 0; i < emiCount; i++)
            {
                double interest = balance * monthlyRate;
                double principalPaid = payment - interest;
                balance -= principalPaid;
                if (balance < 1e-2) break;
            }
            return (decimal)Math.Max(0, balance);
        }

        // Calculate tenure needed to repay 'principal' with given EMI at a given variable rate
        private int GetMonthsToRepay(decimal principal, decimal emi, double annualRate)
        {
            double P = (double)principal;
            double R = annualRate / 12.0 / 100.0;
            double E = (double)emi;

            if (R == 0)
                return P > 0 ? (int)Math.Ceiling(P / E) : 0;

            // Formula: N = [ln(E)-ln(E - P*R)]/ln(1+R)
            var top = Math.Log(E) - Math.Log(E - P * R);
            var bottom = Math.Log(1 + R);

            int monthsNeeded = top > 0 && bottom > 0 ? (int)Math.Ceiling(top / bottom) : int.MaxValue;

            return Math.Max(monthsNeeded, 0);
        }

        private (int NewMonths, decimal NewTotalRepaid, decimal InterestSaved)
        CalculateRecurringExtraPaymentImpact(decimal principal, decimal emi, double annualRate, int maxMonths, decimal extraPerMonth)
        {
            double balance = (double)principal;
            double totalPaid = 0;
            double paidInterest = 0;
            double monthlyRate = annualRate / 12.0 / 100.0;
            double totalMonthly = (double)emi + (double)extraPerMonth;

            int nMonths = 0;

            while (balance > 1e-2 && nMonths < maxMonths * 2)
            {
                double interestForMonth = balance * monthlyRate;
                double principalForMonth = Math.Min(totalMonthly - interestForMonth, balance);
                if (principalForMonth <= 0)
                    break;

                balance -= principalForMonth;
                paidInterest += interestForMonth;
                totalPaid += interestForMonth + principalForMonth;
                nMonths++;

                if (balance < 1e-2) break;// allow for rounding errors
            }

            decimal oldRepaymentLeft = emi * maxMonths;
            decimal newRepaymentLeft = (decimal)totalPaid;

            return (
                  NewMonths: nMonths,
                  NewTotalRepaid: newRepaymentLeft,
                  InterestSaved: oldRepaymentLeft - newRepaymentLeft
             );
        }
    }
}
