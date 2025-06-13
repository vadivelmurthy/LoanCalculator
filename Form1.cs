using System.Text;
using System.Windows.Forms;
using ClosedXML.Excel;
using Newtonsoft.Json;

namespace ProbationDaysApp
{
    public partial class Form1 : Form
    {
        private int days = 180;
        private readonly string savePath = "days.txt";
        decimal personnelLoan = 20675m;
        decimal creditLoan = 2015m;
        decimal savings = 450000m;
        decimal loanamount = 180000m;
        decimal originalLoanAmount = 185000m;
        decimal monthlyEmi = 699.14m;
        DateTime startDate = new DateTime(2023, 8, 12);
        decimal[] variableRates = { 3m, 3.5m, 4m, 4.5m, 5m };
        int totalMonths = 35 * 12;
        private readonly string accountsPath = "accounts.json";
        private readonly string summaryPath = "summary.json";
        List<SummaryEntry> summaryLog = new List<SummaryEntry>();

        public Form1()
        {
            InitializeComponent();
            LoadDayValue();
            UpdateTextBox();
            LoadAmounts();      
            UpdateDisplays();   
            LoadSummary();
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {
            textBoxDays.Text = days.ToString();
            richTextBoxSummary.Visible = false;
            richTexthomeLoan.Text = loanamount.ToString();
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SaveDayValue();
            SaveAmounts();
            base.OnFormClosed(e);
        }
        private void btnSummary_Click(object sender, EventArgs e)
        {
            if (summaryLog.Count > 0)
            {
                ShowColoredGroupedSummary();
                richTextBoxSummary.Visible = true;
            }
            else
            {
                MessageBox.Show("No changes to summarize.");
                richTextBoxSummary.Visible = false;
            }
        }

        private void buttonSubtract_Click_1(object sender, EventArgs e)
        {
            if (days > 0)
            {
                days--;
                UpdateTextBox();
                SaveDayValue();
            }
        }
        private void UpdateDisplays()
        {
            richTextBoxPersonnelLoan.Text = personnelLoan.ToString();
            richTextBoxCreditLoan.Text = creditLoan.ToString();
            richTextBoxSavings.Text = savings.ToString();
            richTexthomeLoan.Text = loanamount.ToString();
        }
        private void LoadDayValue()
        {
            if (File.Exists(savePath))
            {
                string content = File.ReadAllText(savePath);
                if (!int.TryParse(content, out days) || days < 0)
                    days = 180;
            }
        }
        private void UpdateTextBox()
        {
            textBoxDays.Text = days.ToString();
        }
        private void SaveDayValue()
        {
            File.WriteAllText(savePath, days.ToString());
        }
        private void btnSubtract_Click(object sender, EventArgs e)
        {
            HandleOperation(false);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            HandleOperation(true);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            personnelLoan = 20675m;
            creditLoan = 2015m;
            savings = 450000m;
            days = 180;
            loanamount = 180000m;

            UpdateDisplays();
            UpdateTextBox();

            summaryLog.Clear();
            richTextBoxSummary.Visible = false;
            SaveAmounts();
        }
        private void SaveAmounts()
        {
            var data = new AccountData
            {
                LoanAmount = loanamount,
                PersonnelLoan = personnelLoan,
                CreditLoan = creditLoan,
                Savings = savings
            };
            File.WriteAllText(accountsPath, JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        private void LoadAmounts()
        {
            if (File.Exists(accountsPath))
            {
                var json = File.ReadAllText(accountsPath);
                try
                {
                    var data = JsonConvert.DeserializeObject<AccountData>(json);
                    if (data != null)
                    {
                        loanamount = data.LoanAmount;
                        personnelLoan = data.PersonnelLoan;
                        creditLoan = data.CreditLoan;
                        savings = data.Savings;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private void SaveSummary()
        {
            File.WriteAllText(summaryPath, JsonConvert.SerializeObject(summaryLog, Formatting.Indented));
        }

        private void LoadSummary()
        {
            if (File.Exists(summaryPath))
            {
                try
                {
                    var json = File.ReadAllText(summaryPath);
                    var loaded = JsonConvert.DeserializeObject<List<SummaryEntry>>(json);
                    if (loaded != null)
                        summaryLog = loaded;
                }
                catch { }
            }
        }

        private void HandleOperation(bool isAdd)
        {
            string[] options = { "Personnel Loan", "Credit Loan", "Savings", "Home Loan" };

            using (var selectForm = new ComboInputDialog(options, isAdd ?
                   "Select Account to Add" :
                   "Select Account to Subtract"))
            {
                if (selectForm.ShowDialog() == DialogResult.OK)
                {
                    string selectedOption = selectForm.SelectedItem;

                    string promptTitle = $"{(isAdd ? "Add" : "Subtract")} Amount";
                    string promptMessage = $"Enter amount to {(isAdd ?
                                           $"add to {selectedOption}" :
                                           $"subtract from {selectedOption}")}:";
                    decimal amt;

                    using (var inputForm = new AmountInputDialog(promptTitle, promptMessage))
                    {
                        if (inputForm.ShowDialog() != DialogResult.OK ||
                            !decimal.TryParse(inputForm.AmountString, out amt) ||
                            amt <= 0)
                        {
                            MessageBox.Show("Invalid amount entered.");
                            return;
                        }
                    }

                    bool success = false;
                    decimal before = 0;
                    decimal after = 0;
                    string currency = "";
                    string actionWord = "";

                    // Section for summary grouping
                    string section =
                        selectedOption == "Home Loan"
                        ? "Home"
                        : selectedOption == "Savings"
                            ? "" // or use another section name for savings if desired
                            : "Loan";

                    if (selectedOption == "Personnel Loan")
                    {
                        if (!isAdd && amt > personnelLoan)
                        {
                            MessageBox.Show("You exceeded the available Personnel Loan amount.");
                            return;
                        }
                        currency = "Euro";
                        before = personnelLoan;
                        personnelLoan += isAdd ? amt : -amt;
                        after = personnelLoan;
                        actionWord = (isAdd ? "added to" : "payment made on");
                        success = true;
                    }
                    else if (selectedOption == "Credit Loan")
                    {
                        if (!isAdd && amt > creditLoan)
                        {
                            MessageBox.Show("You exceeded the available Credit	Loan amount.");
                            return;
                        }
                        currency = "Euro"; before = creditLoan; creditLoan += isAdd ? amt : -amt; after = creditLoan; actionWord = (isAdd ? "added to" : "payment made on"); success = true;
                    }
                    else if (selectedOption == "Savings")
                    {
                        if (!isAdd && amt > savings)
                        {
                            MessageBox.Show("You exceeded the available Savings amount."); return;
                        }
                        currency = "Rupees"; before = savings; savings += isAdd ? amt : -amt; after = savings; actionWord = (isAdd ? "added	to" : "subtracted from"); success = true;
                    }
                    else if (selectedOption == "Home Loan")
                    {
                        if (!isAdd && amt > loanamount)
                        {
                            MessageBox.Show("You exceeded	the available Home	Loan amount."); return;
                        }
                        currency = "Euro"; before = loanamount; loanamount += isAdd ? amt : -amt; after = loanamount; actionWord = (isAdd ? "added	to" : "payment made on"); success = true;
                    }

                    if (success)
                    {
                        AddToSummary(section, selectedOption, before, amt, after, currency, actionWord);
                        UpdateDisplays();
                        SaveAmounts();
                    }
                }
            }
        }
        private void AddToSummary(string section, string account, decimal before, decimal amount, decimal after, string currency, string action)
        {
            summaryLog.Add(new SummaryEntry
            {
                Date = DateTime.Now,
                Section = section,
                Account = account,
                Before = before,
                Amount = amount,
                After = after,
                Currency = currency,
                Action = action
            });
            SaveSummary();
        }
        private void ShowColoredGroupedSummary()
        {
            richTextBoxSummary.Clear();
            var groupedByDate = summaryLog.GroupBy(e => e.Date.Date).OrderBy(g => g.Key);

            foreach (var dayGroup in groupedByDate)
            {
                string dateStr = $"On {dayGroup.Key:MMMM} {GetDayWithSuffix(dayGroup.Key.Day)} {dayGroup.Key.Year}:";
                int headerStart = richTextBoxSummary.TextLength;
                richTextBoxSummary.AppendText(dateStr + Environment.NewLine);
                richTextBoxSummary.Select(headerStart, dateStr.Length);
                richTextBoxSummary.SelectionFont = new Font(richTextBoxSummary.Font, FontStyle.Bold);
                richTextBoxSummary.SelectionColor = Color.DarkBlue;

                // Group by Section ("Loan" or "Home")
                var bySection = dayGroup.GroupBy(e => e.Section);

                foreach (var sectionGrp in bySection)
                {
                    string secHeader =
                        sectionGrp.Key == "Loan" ? "\nLOAN SECTION:\n" :
                        sectionGrp.Key == "Home" ? "\nHOME SECTION:\n" : $"\n{sectionGrp.Key.ToUpper()} SECTION:\n";

                    int secStart = richTextBoxSummary.TextLength;
                    richTextBoxSummary.AppendText(secHeader);
                    richTextBoxSummary.Select(secStart, secHeader.Length);
                    richTextBoxSummary.SelectionFont = new Font(richTextBoxSummary.Font, FontStyle.Bold | FontStyle.Underline);
                    richTextBoxSummary.SelectionColor = Color.MediumVioletRed;

                    foreach (var entry in sectionGrp)
                    {
                        string detail =
        $@"{entry.Account} before operation: {entry.Before:N2} {entry.Currency}
{entry.Amount:N2} {entry.Currency} {entry.Action} {entry.Account}
{entry.Account} after operation: {entry.After:N2} {entry.Currency}

";
                        int startIdx = richTextBoxSummary.TextLength;
                        richTextBoxSummary.AppendText(detail);

                        Color color = Color.Black;
                        if ((entry.Account.Contains("Loan") && entry.Action.Contains("payment made on")) ||
                            (entry.Account == "Savings" && entry.Action.Contains("added to")))
                            color = Color.Green;
                        else if (entry.Account.Contains("Loan") && entry.Action.Contains("added to"))
                            color = Color.Red;
                        else if (entry.Account == "Savings" && entry.Action.Contains("subtracted from"))
                            color = Color.DarkOrange;

                        // Apply coloring for this detail block only
                        richTextBoxSummary.Select(startIdx, detail.Length);
                        richTextBoxSummary.SelectionColor = color;
                        richTextBoxSummary.SelectionFont = new Font(richTextBoxSummary.Font, FontStyle.Regular);

                        // Reset selection for next block
                        richTextBoxSummary.Select(richTextBoxSummary.TextLength, 0);
                        richTextBoxSummary.SelectionColor = richTextBoxSummary.ForeColor;
                        richTextBoxSummary.SelectionFont = new Font(richTextBoxSummary.Font, FontStyle.Regular);
                    }
                }

                // Space between days:
                richTextBoxSummary.AppendText(Environment.NewLine);
            }

            richTextBoxSummary.ScrollToCaret();
        }
        public class ComboInputDialog : Form
        {
            public string SelectedItem { get; private set; }
            ComboBox combo;

            public ComboInputDialog(string[] options, string title)
            {
                this.Text = title ?? "";

                //combo = new ComboBox()
                //{ DataSource = options.ToList(), Dock = DockStyle.Top, DropDownStyle = ComboBoxStyle.DropDownList };

                //var okButton = new Button() { Text = "OK", Dock = DockStyle.Bottom };

                combo = new ComboBox()
                {
                    DataSource = options.ToList(),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Left = 20,
                    Top = 20,
                    Width = 300
                };

                var okButton = new Button()
                {
                    Text = "OK",
                    Left = 120,
                    Top = combo.Bottom + 20, // Place it below the ComboBox with space
                    Width = 80,
                    Height = 30
                };

                okButton.Click += (s, e) => { SelectedItem = combo.SelectedItem.ToString(); this.DialogResult = DialogResult.OK; Close(); };

                Controls.Add(combo); Controls.Add(okButton);

                StartPosition = FormStartPosition.CenterParent; Width = 350; Height = okButton.Bottom + 60; FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false; MinimizeBox = false;

                AcceptButton = okButton; // Enter key triggers OK
            }
        }
        // Simple dialog for entering an amount as text
        public class AmountInputDialog : Form
        {
            TextBox inputTextBox;
            public string AmountString => inputTextBox.Text.Trim();

            public AmountInputDialog(string title, string message)
            {
                this.Text = title ?? "";
                this.Width = 320;
                this.Height = 180;
                this.StartPosition = FormStartPosition.CenterParent;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;

                var lbl = new Label()
                {
                    Text = message ?? "",
                    Left = 20,
                    Top = 15,
                    Width = 260,
                    Height = 30
                };

                inputTextBox = new TextBox()
                {
                    Left = lbl.Left,
                    Top = lbl.Bottom + 10,
                    Width = 260
                };

                var okBtn = new Button()
                {
                    Text = "OK",
                    Width = 80,
                    Height = 30,
                    Left = (this.ClientSize.Width - 80) / 2, // Center horizontally
                    Top = inputTextBox.Bottom + 15
                };

                okBtn.Click += (s, e) => { this.DialogResult = DialogResult.OK; Close(); };

                Controls.Add(lbl);
                Controls.Add(inputTextBox);
                Controls.Add(okBtn);

                AcceptButton = okBtn; // Enter key triggers OK button.
            }
        }

        private string GetDayWithSuffix(int day)
        {
            if (day >= 11 && day <= 13) return day + "th";
            switch (day % 10)
            {
                case 1: return day + "st";
                case 2: return day + "nd";
                case 3: return day + "rd";
                default: return day + "th";
            }
        }
        public class SummaryEntry
        {
            public DateTime Date { get; set; }
            public string Section { get; set; }
            public string Account { get; set; }
            public decimal Before { get; set; }
            public decimal Amount { get; set; }
            public decimal After { get; set; }
            public string Currency { get; set; }
            public string Action { get; set; } // e.g., "payment made on", "added to"
        }
        private void btn_export_to_excel_Click(object sender, EventArgs e)
        {
            if (summaryLog.Count == 0)
            {
                MessageBox.Show("No records to export.");
                return;
            }

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Summary");

                int row = 1;

                var groupedByDate = summaryLog.GroupBy(x => x.Date.Date).OrderBy(x => x.Key);

                foreach (var dayGroup in groupedByDate)
                {
                    ws.Cell(row, 1).Value =
                      $"On {dayGroup.Key:MMMM} {GetDayWithSuffix(dayGroup.Key.Day)} {dayGroup.Key.Year}";
                    ws.Range(row, 1, row, 7).Merge().Style.Font.Bold = true;
                    ws.Range(row, 1, row, 7).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    row++;

                    var sectionsInDay = dayGroup.GroupBy(x => x.Section);

                    foreach (var sec in sectionsInDay)
                    {

                        // Section header row ("LOAN SECTION:" or "HOME SECTION:")
                        ws.Cell(row, 1).Value = (sec.Key == "Loan") ? "LOAN SECTION:"
                            : (sec.Key == "Home") ? "HOME SECTION:"
                            : $"{sec.Key.ToUpper()} SECTION:";
                        ws.Range(row, 1, row, 7).Merge().Style.Font.Bold = true;
                        ws.Range(row, 1, row, 7).Style.Fill.BackgroundColor =
                                          sec.Key == "Loan" ? XLColor.LightGray :
                                          sec.Key == "Home" ? XLColor.LightSalmon :
                                          XLColor.LightCyan;
                        row++;

                        // Column headers:
                        ws.Cell(row, 1).Value = "Account";
                        ws.Cell(row, 2).Value = "Before";
                        ws.Cell(row, 3).Value = "Amount";
                        ws.Cell(row, 4).Value = "After";
                        ws.Cell(row, 5).Value = "Currency";
                        ws.Cell(row, 6).Value = "Action";

                        ws.Range(row, 1, row, 6).Style.Font.Bold = true;
                        row++;

                        foreach (var entry in sec)
                        {
                            ws.Cell(row, 1).Value = entry.Account;
                            ws.Cell(row, 2).Value = entry.Before;
                            ws.Cell(row, 3).Value = entry.Amount;
                            ws.Cell(row, 4).Value = entry.After;
                            ws.Cell(row, 5).Value = entry.Currency;
                            ws.Cell(row, 6).Value = entry.Action;

                            // Excel color logic
                            XLColor colorXL =
                                ((entry.Account.Contains("Loan") && entry.Action.Contains("payment made on")) ||
                                 (entry.Account == "Savings" && entry.Action.Contains("added to"))) ?
                                XLColor.LightGreen :
                                ((entry.Account.Contains("Loan") && entry.Action.Contains("added to"))) ?
                                XLColor.LightPink :
                                ((entry.Account == "Savings" && entry.Action.Contains("subtracted from"))) ?
                                XLColor.LightGoldenrodYellow :
                                XLColor.White;

                            ws.Row(row).Style.Fill.BackgroundColor = colorXL;

                            row++;
                        }

                        row++; // Space between sections/days
                    }
                }

                for (int c = 1; c <= 6; c++)
                    ws.Column(c).AdjustToContents();

                using (SaveFileDialog sfd = new SaveFileDialog()
                { Filter = "Excel Workbook|*.xlsx", FileName = "summary.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                        workbook.SaveAs(sfd.FileName);
                }

                MessageBox.Show("Exported successfully!");
            }

        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            int monthsPaid = ((DateTime.Today.Year - startDate.Year) * 12) + DateTime.Today.Month - startDate.Month;
            int monthsRemaining = totalMonths - monthsPaid;
            decimal remainingPrincipal = originalLoanAmount;
            decimal interestRateMonthly = 2.65m / 100 / 12;
            decimal totalInterestPaid = 0;
            decimal totalPrincipalPaid = 0;

            // Loop through past months to calculate actual paid interest/principal
            for (int i = 0; i < monthsPaid; i++)
            {
                decimal interest = remainingPrincipal * interestRateMonthly;
                decimal principal = monthlyEmi - interest;
                remainingPrincipal -= principal;
                totalInterestPaid += interest;
                totalPrincipalPaid += principal;
            }
            decimal totalPaid = totalPrincipalPaid + totalInterestPaid;
            decimal futureFixedTotal = monthsRemaining * monthlyEmi;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{"Original Loan Amount",-28}       :€{originalLoanAmount:N2}");
            sb.AppendLine($"{"Loan Amount (current)",-28}       :€{loanamount:N2}");
            sb.AppendLine($"{"Fixed EMI",-28}                  :€{monthlyEmi:N2}");
            sb.AppendLine($"{"Start Date",-28}                  :{startDate:dd MMM yyyy}");
            sb.AppendLine($"{"Months Paid",-28}               :€{monthsPaid}");
            sb.AppendLine($"{"Months Remaining",-28}         :€{monthsRemaining}");
            sb.AppendLine($"{"Total Paid",-28}                  :€{totalPaid:N2}");
            sb.AppendLine($"   {"└ Principal Paid",-25}             :€{totalPrincipalPaid:N2}");
            sb.AppendLine($"   {"└ Interest Paid",-25}              :€{totalInterestPaid:N2}");
            sb.AppendLine($"{"Remaining (2.65%)",-28}           :€{futureFixedTotal:N2}");

            sb.AppendLine();
            sb.AppendLine("--- Variable Rate Projections ---");

            foreach (var rate in variableRates)
            {
                decimal rMonthly = rate / 100m / 12m;
                decimal emi = remainingPrincipal * rMonthly *
                              (decimal)Math.Pow(1 + (double)rMonthly, monthsRemaining) /
                              ((decimal)Math.Pow(1 + (double)rMonthly, monthsRemaining) - 1);
                decimal total = emi * monthsRemaining;

                // Align columns in projections as well:
                sb.AppendLine(string.Format("Rate {0,-5}% → EMI: {1,-10} Total: {2}", rate, $"€{emi:N2}", $"€{total:N2}"));
                // Or using interpolation:
                // sb.AppendLine($"Rate {rate,-5}% → EMI: {"€" + emi.ToString("N2"),-10} Total: {"€" + total.ToString("N2") }");

            }

            LoanDetailsForm detailsForm = new LoanDetailsForm(sb.ToString());
            detailsForm.ShowDialog();
        }


    }
}
