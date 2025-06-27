using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ClosedXML.Excel;
using Newtonsoft.Json;

namespace ProbationDaysApp
{
    public partial class Form1 : Form
    {
        private readonly DateTime probationStart = new DateTime(2025, 4, 28); // 28th April 2025
        private readonly int probationMonths = 6;
        DateTime probationEnd => probationStart.AddMonths(probationMonths);
        private readonly string savePath = "days.txt";
        decimal personnelLoan = 20675m;
        decimal creditLoan = 2015m;
        decimal savings = 450000m;
        decimal loanamount = 180000m;
        decimal originalLoanAmount = 185000m;
        decimal monthlyEmi = 699.14m;
        decimal aibBalance = 300m;
        decimal revolutBalance = 100m;
        DateTime startDate = new DateTime(2023, 8, 12);
        decimal[] variableRates = { 2.75m, 3m, 3.25m, 3.5m, 3.75m, 4m, 4.25m, 4.5m, 4.75m, 5m };
        int totalMonths = 35 * 12;
        private readonly string accountsPath = "accounts.json";
        private readonly string summaryPath = "summary.json";
        List<SummaryEntry> summaryLog = new List<SummaryEntry>();

        public Form1()
        {
            InitializeComponent();
            UpdateTextBox();
            LoadAmounts();      
            UpdateDisplays();   
            LoadSummary();
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {
            int daysLeft = GetDaysLeft();
            textBoxDays.Text = daysLeft.ToString();
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
        private int GetDaysLeft()
        {
            int daysLeft = (probationEnd - DateTime.Today).Days;
            return daysLeft > 0 ? daysLeft : 0;
        }
        private void UpdateDisplays()
        {
            richTextBoxPersonnelLoan.Text = personnelLoan.ToString();
            richTextBoxCreditLoan.Text = creditLoan.ToString();
            richTextBoxSavings.Text = savings.ToString();
            richTexthomeLoan.Text = loanamount.ToString();
            richTextAib.Text = aibBalance.ToString();
            richTextRevolut.Text = revolutBalance.ToString();
            decimal totalSalaryBalance = aibBalance + revolutBalance;
            richTextTotalBalance.Text = totalSalaryBalance.ToString("N2");

        }
        private void UpdateTextBox()
        {
            int daysLeft = GetDaysLeft();
            textBoxDays.Text = daysLeft.ToString();
        }
        private void SaveDayValue()
        {
            int daysLeft = GetDaysLeft();
            File.WriteAllText(savePath, daysLeft.ToString());
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
            loanamount = 180000m;
            revolutBalance = 300m;
            aibBalance = 300m;

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
                Savings = savings,
                AibBalance = aibBalance,
                RevolutBalamce = revolutBalance
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
                        aibBalance= data.AibBalance;
                        revolutBalance = data.RevolutBalamce;
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
            string[] options = { "Personnel Loan", "Credit Loan", "Savings", "Home Loan", "AIB Balance", "Revolut Balance" };

            using (var selectForm = new ComboInputDialog(options, isAdd ?
                   "Select Account to Add" :
                   "Select Account to Subtract"))
            {
                if (selectForm.ShowDialog() == DialogResult.OK)
                {
                    string selectedOption = selectForm.SelectedItem;

                    string promptTitle = $"{(isAdd ? "Add" : "Subtract")} Amount";
                    string promptMessage = $"Enter amount(s) to {(isAdd ? "add to" : "subtract from")} {selectedOption} (separate multiple amounts with commas):";

                    using (var inputForm = new AmountInputDialog(promptTitle, promptMessage))
                    {
                        if (inputForm.ShowDialog() != DialogResult.OK)
                            return;

                        // Parse multiple decimal values from comma-separated input
                        var amounts = inputForm.AmountString
                            .Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => decimal.TryParse(s.Trim(), out var val) ? val : (decimal?)null)
                            .Where(val => val.HasValue && val.Value > 0)
                            .Select(val => val.Value)
                            .ToList();

                        if (amounts.Count == 0)
                        {
                            MessageBox.Show("Please enter one or more valid positive amounts.");
                            return;
                        }

                        foreach (var amt in amounts)
                        {
                            bool success = false;
                            decimal before = 0;
                            decimal after = 0;
                            string currency = "";
                            string actionWord = "";

                            // Section for summary grouping
                            string section =
                                  selectedOption == "Home Loan" ? "Home"
                                  : selectedOption == "Savings" ? ""
                                  : (selectedOption == "AIB Balance" || selectedOption == "Revolut Balance") ? "Salary"
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
                                    MessageBox.Show("You exceeded the available Credit Loan amount.");
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
                                currency = "Rupees"; before = savings; savings += isAdd ? amt : -amt; after = savings; actionWord = (isAdd ? "added to" : "subtracted from"); success = true;
                            }
                            else if (selectedOption == "Home Loan")
                            {
                                if (!isAdd && amt > loanamount)
                                {
                                    MessageBox.Show("You exceeded	the available Home	Loan amount."); return;
                                }
                                currency = "Euro"; before = loanamount; loanamount += isAdd ? amt : -amt; after = loanamount; actionWord = (isAdd ? "added to" : "payment made on"); success = true;
                            }
                            else if (selectedOption == "AIB Balance")
                            { currency = "Euro"; before = aibBalance; aibBalance += isAdd ? amt : -amt; after = aibBalance; actionWord = (isAdd ? "added to" : "payment made on"); success = true; }
                            else if (selectedOption == "Revolut Balance")
                            { currency = "Euro"; before = revolutBalance; revolutBalance += isAdd ? amt : -amt; after = revolutBalance; actionWord = (isAdd ? "added to" : "payment made on"); success = true; }

                            if (success)
                            {
                                AddToSummary(section, selectedOption, before, amt, after, currency, actionWord);
                                UpdateDisplays();
                                SaveAmounts();
                            }
                        }
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
            richTextBoxSummary.Font = new Font("Consolas", 10);
            richTextBoxSummary.Clear();

            var groupedByDate = summaryLog.GroupBy(e => e.Date.Date).OrderBy(g => g.Key);

            foreach (var dayGroup in groupedByDate)
            {
                // === Date Header: Big, bold, blue ===
                string dateStr = $"On {dayGroup.Key:MMMM} {GetDayWithSuffix(dayGroup.Key.Day)} {dayGroup.Key.Year}:";
                int dateHeaderStart = richTextBoxSummary.TextLength;
                richTextBoxSummary.AppendText(dateStr + Environment.NewLine + Environment.NewLine);

                var dateFont = new Font("Consolas", 18, FontStyle.Bold);
                richTextBoxSummary.Select(dateHeaderStart, dateStr.Length);
                richTextBoxSummary.SelectionFont = dateFont;
                richTextBoxSummary.SelectionColor = Color.RoyalBlue;

                // Reset selection/font for next block
                richTextBoxSummary.Select(richTextBoxSummary.TextLength, 0);
                richTextBoxSummary.SelectionFont = new Font("Consolas", 10);
                richTextBoxSummary.SelectionColor = Color.Black;

                var bySection = dayGroup.GroupBy(e => e.Section);

                foreach (var sectionGrp in bySection)
                {
                    // Section header - only name (no code)
                    string secHeader = $"{sectionGrp.Key.ToUpper()} SECTION:";
                    int secHeaderStart = richTextBoxSummary.TextLength;
                    richTextBoxSummary.AppendText(secHeader + Environment.NewLine);

                    var secFont = new Font("Consolas", 14, FontStyle.Bold);
                    richTextBoxSummary.Select(secHeaderStart, secHeader.Length);
                    richTextBoxSummary.SelectionFont = secFont;
                    richTextBoxSummary.SelectionColor = Color.Navy;

                    // Reset for rows:
                    richTextBoxSummary.Select(richTextBoxSummary.TextLength, 0);
                    richTextBoxSummary.SelectionFont = new Font("Consolas", 10);
                    richTextBoxSummary.SelectionColor = Color.Black;

                    // Table header row
                    string tableHeader =
                        String.Format("{0,-18}{1,16}{2,14}{3,14}{4,10}   {5}",
                            "Account", "Before", "Amount", "After", "Currency", "Action");

                    int tableHeaderStartIdx = richTextBoxSummary.TextLength;
                    // Append and format header row bold black:
                    richTextBoxSummary.AppendText(tableHeader + Environment.NewLine);

                    var tableHeadFontBold = new Font("Consolas", 10f, FontStyle.Bold);

                    // Color Table Header:
                    richTextBoxSummary.Select(tableHeaderStartIdx, tableHeader.Length);
                    richTextBoxSummary.SelectionFont = tableHeadFontBold;

                    // Append separator and make it gray:
                    string separatorLine = new string('-', tableHeader.Length) + Environment.NewLine;

                    int sepStartIdx = richTextBoxSummary.TextLength;
                    int sepLen = separatorLine.Length;

                    richTextBoxSummary.AppendText(separatorLine);

                    // Color the dashes gray except the newline at end
                    if (sepLen > 0)
                    {
                        try
                        {
                            int dashLenOnly = separatorLine.TrimEnd('\r', '\n').Length;
                            if (dashLenOnly > 0)
                                richTextBoxSummary.Select(sepStartIdx, dashLenOnly);
                            if (dashLenOnly > 0)
                                richTextBoxSummary.SelectionColor = Color.Gray;
                        }
                        catch { }
                        finally
                        {
                            try { richTextBoxSummary.Select(richTextBoxSummary.TextLength, 0); } catch { }
                        }
                    }

                    foreach (var entry in sectionGrp)
                    {
                        string accLowerNoSpaces = entry.Account.Replace(" ", "").ToLowerInvariant();
                        string accLower = entry.Account.ToLowerInvariant();
                        string actionLowerNoSpace = entry.Action.Replace(" ", "").ToLowerInvariant();

                        Color colorDetailRow = Color.Black;

                        // Loans/Credit accounts: Home Loan / Personal Loan / Credit Card etc.
                        if (
                            accLower.Contains("loan")
                            || accLower.Contains("credit")
                            || accLower.Contains("personal loan")
                           )
                        {
                            if (entry.Action.ToLowerInvariant().Contains("added to"))
                                colorDetailRow = Color.Red;
                            else
                                colorDetailRow = Color.Green;
                        }
                        // Savings/Balances accounts: Savings / AIB Balance / Revolut Balance etc.
                        else if (
                            accLowerNoSpaces == "savings"
                            || accLowerNoSpaces == "aibbalance"
                            || accLowerNoSpaces == "revolutbalance"
                           )
                        {
                            if (NormalizeWhitespace(entry.Action.ToLowerInvariant()).Contains("added to"))
                                colorDetailRow = Color.Green;
                            else if (NormalizeWhitespace(entry.Action.ToLowerInvariant()).Contains("payment made on"))
                                colorDetailRow = Color.Red;
                        }

                        string line =
                            String.Format("{0,-18}{1,16:N2}{2,14:N2}{3,14:N2}{4,10}   {5}",
                                entry.Account,
                                entry.Before,
                                entry.Amount,
                                entry.After,
                                entry.Currency,
                                entry.Action.Replace("\t", " ")
                            );

                        int startIdxDetailRow = richTextBoxSummary.TextLength;
                        int len = line.Length;

                        var normalTableFont = new Font(richTextBoxSummary.Font.FontFamily, 10f);

                        if (colorDetailRow == Color.Red || colorDetailRow == Color.Green)
                            normalTableFont = new Font(richTextBoxSummary.Font.FontFamily, 10f, FontStyle.Bold);

                        try
                        {
                            // Write the line:
                            richTextBoxSummary.AppendText(line + Environment.NewLine);
                            if (len > 0)
                            {
                                try { richTextBoxSummary.Select(startIdxDetailRow, len); } catch { }
                                try { richTextBoxSummary.SelectionFont = normalTableFont; } catch { }
                                try { richTextBoxSummary.SelectionColor = colorDetailRow; } catch { }
                            }
                        }
                        catch { }
                        finally
                        {
                            try
                            {
                                if (len > 0)
                                {
                                    int after = len + startIdxDetailRow + Environment.NewLine.Length;
                                    if (after <= richTextBoxSummary.TextLength)
                                    {
                                        try { richTextBoxSummary.Select(after, 0); } catch { }
                                    }
                                    else
                                    {
                                        try { richTextBoxSummary.Select(richTextBoxSummary.TextLength, 0); } catch { }
                                    }
                                }
                                else
                                {
                                    try { richTextBoxSummary.Select(richTextBoxSummary.TextLength, 0); } catch { }
                                }
                                try
                                {
                                    Color fallbackCol =
                                        !richTextBoxSummary.ForeColor.IsEmpty ?
                                        richTextBoxSummary.ForeColor : Color.Black;
                                    /* fallback foreground */
                                    ricHtexTboXtOSeleCtion(normalTableFont, fallbackCol);
                                }
                                catch { }
                            }
                            catch { }
                        }
                    }

                    // Show total salary balance at end of salary group:
                    if (sectionGrp.Key == "Salary")
                    {
                        decimal totalAibThisDay =
                            sectionGrp.Where(x => x.Account.Equals("AIB Balance", StringComparison.OrdinalIgnoreCase))
                                      .LastOrDefault()?.After ?? 0m;
                        decimal totalRevolutThisDay =
                            sectionGrp.Where(x => x.Account.Equals("Revolut Balance", StringComparison.OrdinalIgnoreCase))
                                      .LastOrDefault()?.After ?? 0m;

                        decimal totalSalaryBalanceThisDay = totalAibThisDay + totalRevolutThisDay;

                        string totalStr =
                    $@"{"",-18}{"",-16}{"",-14}{"",-14}{"",-10}   Total Salary Balance: {totalSalaryBalanceThisDay:N2} Euro{Environment.NewLine}";

                        int tStartSalBalSumSec = richTextBoxSummary.TextLength;

                        var fontToUseForTotalSalaryBoldItalicBlue =
                           new Font(richTextBoxSummary.Font.FontFamily, 11f, FontStyle.Bold | FontStyle.Italic);

                        ricHtexTboXtOAppend(totalStr);

                        ricHtexTboXtOSeleCt(tStartSalBalSumSec, totalStr.Length);

                        ricHtexTboXtOSeleCtion(fontToUseForTotalSalaryBoldItalicBlue, Color.RoyalBlue);

                        ricHtexTboXtOSeleCt(richTextBoxSummary.TextLength, 0);
                    }


                    ricHtexTboXtOAppend(Environment.NewLine); // blank line after each section

                }

                ricHtexTboXtOAppend(Environment.NewLine + Environment.NewLine); // double blank between days

            }

            /*Scroll down after filling*/
            try { ricHtexTboXtOScroll(); } catch { }


            // ----------- Local Helper Functions -------------

            // These are just wrappers for clarity. You can use your own or inline as usual.
            void ricHtexTboXtOAppend(string s) => this.richTextBoxSummary.AppendText(s);
            void ricHtexTboXtOSeleCt(int s, int l) => this.richTextBoxSummary.Select(s, l);
            void ricHtexTboXtOSeleCtion(Font f, Color c) { this.richTextBoxSummary.SelectionFont = f; this.richTextBoxSummary.SelectionColor = c; }
            void ricHtexTboXtOScroll() => this.richTextBoxSummary.ScrollToCaret();

        }
        string NormalizeWhitespace(string input)
        {
            return Regex.Replace(input ?? "", @"\s+", " ").Trim();
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

                        ws.Cell(row, 1).Value =
                            sec.Key == "Loan" ? "LOAN SECTION:" :
                            sec.Key == "Home" ? "HOME SECTION:" :
                            sec.Key == "" ? "SAVINGS SECTION:" :
                            sec.Key == "Salary" ? "SALARY SECTION:" :
                            $"{sec.Key.ToUpper()} SECTION:";
                        ws.Range(row, 1, row, 7).Merge().Style.Font.Bold = true;

                        ws.Range(row, 1,
                                 row,
                                 7).Style.Fill.BackgroundColor =
                                 sec.Key == "Loan" ? XLColor.LightGray :
                                 sec.Key == "Home" ? XLColor.LightSalmon :
                                 sec.Key == "Salary" ? XLColor.CornflowerBlue :
                                 XLColor.LightCyan;

                        row++;

                        ws.Cell(row,
                                1).Value = "Account";
                        ws.Cell(row,
                                2).Value = "Before";
                        ws.Cell(row,
                                3).Value = "Amount";
                        ws.Cell(row,
                                4).Value = "After";
                        ws.Cell(row,
                                5).Value = "Currency";
                        ws.Cell(row,
                               6).Value = "Action";

                        ws.Range(row,
                                 1, row,
                                    6).Style.Font.Bold = true; row++;

                        foreach (var entry in sec)
                        {
                            ws.Cell(
                                row,
                                1).Value = entry.Account;
                            ws.Cell(
                                row,
                                 2).Value = entry.Before;
                            ws.Cell(
                                row,
                                3).Value = entry.Amount;
                            ws.Cell(
                                row,
                                4).Value = entry.After;
                            ws.Cell(
                                row,
                                5).Value = entry.Currency;
                            ws.Cell(
                                row,
                                6).Value = entry.Action;

                            XLColor colorXL =
                                ((entry.Account.Contains("Loan") && entry.Action.Contains("payment made on")) ||
                                 (entry.Account == "Savings" && entry.Action.Contains("added to"))) ?
                                XLColor.LightGreen :
                                ((entry.Section == "Salary")) ?
                                XLColor.CornflowerBlue :
                                ((entry.Account.Contains("Loan") && entry.Action.Contains("added to"))) ?
                                XLColor.LightPink :
                                ((entry.Account == "Savings" && entry.Action.Contains("subtracted from"))) ?
                                XLColor.LightGoldenrodYellow :
                                XLColor.White;

                            ws.Row(row).Style.Fill.BackgroundColor = colorXL;

                            row++;
                        }

                        // For Salary Section add the combined AIB+Revolut after all entries of that section for this day:
                        if (sec.Key == "Salary")
                        {
                            decimal totalAibThisDay =
                                 sec.Where(x => x.Account == "AIB Balance").LastOrDefault()?.After ?? 0m;
                            decimal totalRevolutThisDay =
                                 sec.Where(x => x.Account == "Revolut Balance").LastOrDefault()?.After ?? 0m;
                            decimal totalSalaryBalanceThisDay =
                                 totalAibThisDay + totalRevolutThisDay;

                            // Row for TOTAL SALARY BALANCE
                            ws.Cell(row, 1).Value = "Total Salary Balance (AIB + Revolut)";
                            ws.Range(row, 1, row, 4).Merge().Style.Font.Bold = true;
                            ws.Range(row, 1, row, 4).Style.Fill.BackgroundColor = XLColor.Aqua;

                            ws.Cell(row, 5).Value = totalSalaryBalanceThisDay;// you may want to use .ToString(“N2”) as well.
                            ws.Row(row).Height = 18d;

                            ++row;
                        }

                        ++row;// Space between sections/days
                    }
                }


                for (int c = 1; c <= 6; c++)
                    ws.Column(c).AdjustToContents();

                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", FileName = "summary.xlsx" })
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
            decimal interestRateMonthly = 2.75m / 100 / 12;

            for (int i = 0; i < monthsPaid; i++)
            {
                decimal interest = remainingPrincipal * interestRateMonthly;
                decimal principal = monthlyEmi - interest;
                remainingPrincipal -= principal;
            }
            if (remainingPrincipal < 0m) remainingPrincipal = 0m;

            decimal totalPaid = monthsPaid * monthlyEmi;
            decimal totalPrincipalPaid = originalLoanAmount - loanamount;
            decimal totalInterestPaid = totalPaid - totalPrincipalPaid;

            var loanDetailsModel = new LoanDetailsModel
            {
                OriginalLoanAmount = originalLoanAmount,
                CurrentLoanAmount = loanamount,
                MonthlyEmi = monthlyEmi,
                StartDate = startDate,
                MonthsPaid = monthsPaid,
                MonthsRemaining = monthsRemaining,
                TotalPaid = totalPaid,
                TotalPrincipalPaid = totalPrincipalPaid,
                TotalInterestPaid = totalInterestPaid, 
                FutureFixedTotal = monthsRemaining * monthlyEmi,
                VariableRates = variableRates,
                RemainingPrincipal = remainingPrincipal,
                AnnualInterestRate = 2.75
            };

            var detailsForm = new LoanDetailsForm(loanDetailsModel);
            detailsForm.ShowDialog();
        }

    }
}
