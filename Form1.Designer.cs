namespace ProbationDaysApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBoxDays = new RichTextBox();
            label1 = new Label();
            label2 = new Label();
            richTextBoxPersonnelLoan = new RichTextBox();
            richTextBoxCreditLoan = new RichTextBox();
            label3 = new Label();
            label4 = new Label();
            richTextBoxSavings = new RichTextBox();
            label5 = new Label();
            btnSubtract = new Button();
            btnAdd = new Button();
            btnReset = new Button();
            richTextBoxSummary = new RichTextBox();
            btnSummary = new Button();
            btn_export_to_excel = new Button();
            groupBox1 = new GroupBox();
            groupBox4 = new GroupBox();
            label9 = new Label();
            richTextTotalBalance = new RichTextBox();
            label8 = new Label();
            label7 = new Label();
            richTextRevolut = new RichTextBox();
            richTextAib = new RichTextBox();
            groupBox3 = new GroupBox();
            label6 = new Label();
            btnLoandetails = new Button();
            richTexthomeLoan = new RichTextBox();
            groupBox2 = new GroupBox();
            groupBox1.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // textBoxDays
            // 
            textBoxDays.BackColor = SystemColors.Menu;
            textBoxDays.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxDays.Location = new Point(169, 22);
            textBoxDays.Name = "textBoxDays";
            textBoxDays.ReadOnly = true;
            textBoxDays.Size = new Size(321, 46);
            textBoxDays.TabIndex = 0;
            textBoxDays.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(234, 0);
            label1.Name = "label1";
            label1.Size = new Size(256, 20);
            label1.TabIndex = 2;
            label1.Text = "No of Days Remaining in Probation";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(269, 19);
            label2.Name = "label2";
            label2.Size = new Size(98, 20);
            label2.TabIndex = 3;
            label2.Text = "Loan Section";
            // 
            // richTextBoxPersonnelLoan
            // 
            richTextBoxPersonnelLoan.BackColor = SystemColors.Menu;
            richTextBoxPersonnelLoan.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBoxPersonnelLoan.Location = new Point(14, 69);
            richTextBoxPersonnelLoan.Name = "richTextBoxPersonnelLoan";
            richTextBoxPersonnelLoan.ReadOnly = true;
            richTextBoxPersonnelLoan.Size = new Size(192, 47);
            richTextBoxPersonnelLoan.TabIndex = 4;
            richTextBoxPersonnelLoan.Text = "";
            // 
            // richTextBoxCreditLoan
            // 
            richTextBoxCreditLoan.BackColor = SystemColors.Menu;
            richTextBoxCreditLoan.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBoxCreditLoan.Location = new Point(224, 69);
            richTextBoxCreditLoan.Name = "richTextBoxCreditLoan";
            richTextBoxCreditLoan.ReadOnly = true;
            richTextBoxCreditLoan.Size = new Size(198, 47);
            richTextBoxCreditLoan.TabIndex = 5;
            richTextBoxCreditLoan.Text = "";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(14, 51);
            label3.Name = "label3";
            label3.Size = new Size(106, 15);
            label3.TabIndex = 6;
            label3.Text = "Personnel Loan(€)";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(283, 51);
            label4.Name = "label4";
            label4.Size = new Size(84, 15);
            label4.TabIndex = 7;
            label4.Text = "Credit Card(€)";
            // 
            // richTextBoxSavings
            // 
            richTextBoxSavings.BackColor = SystemColors.Menu;
            richTextBoxSavings.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBoxSavings.Location = new Point(446, 69);
            richTextBoxSavings.Name = "richTextBoxSavings";
            richTextBoxSavings.ReadOnly = true;
            richTextBoxSavings.Size = new Size(207, 47);
            richTextBoxSavings.TabIndex = 8;
            richTextBoxSavings.Text = "";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(501, 51);
            label5.Name = "label5";
            label5.Size = new Size(64, 15);
            label5.TabIndex = 9;
            label5.Text = "Savings(₹)";
            // 
            // btnSubtract
            // 
            btnSubtract.BackColor = SystemColors.Info;
            btnSubtract.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSubtract.Location = new Point(281, 297);
            btnSubtract.Name = "btnSubtract";
            btnSubtract.Size = new Size(126, 40);
            btnSubtract.TabIndex = 10;
            btnSubtract.Text = "Subtract";
            btnSubtract.UseVisualStyleBackColor = false;
            btnSubtract.Click += btnSubtract_Click;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = SystemColors.Info;
            btnAdd.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdd.Location = new Point(419, 297);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(126, 40);
            btnAdd.TabIndex = 11;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnReset
            // 
            btnReset.BackColor = SystemColors.Info;
            btnReset.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReset.Location = new Point(705, 297);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(126, 40);
            btnReset.TabIndex = 12;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btnReset_Click;
            // 
            // richTextBoxSummary
            // 
            richTextBoxSummary.BackColor = SystemColors.Menu;
            richTextBoxSummary.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBoxSummary.Location = new Point(6, 351);
            richTextBoxSummary.Name = "richTextBoxSummary";
            richTextBoxSummary.ReadOnly = true;
            richTextBoxSummary.Size = new Size(1086, 260);
            richTextBoxSummary.TabIndex = 13;
            richTextBoxSummary.Text = "";
            // 
            // btnSummary
            // 
            btnSummary.BackColor = SystemColors.Info;
            btnSummary.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSummary.Location = new Point(560, 297);
            btnSummary.Name = "btnSummary";
            btnSummary.Size = new Size(126, 40);
            btnSummary.TabIndex = 15;
            btnSummary.Text = "Summary";
            btnSummary.UseVisualStyleBackColor = false;
            btnSummary.Click += btnSummary_Click;
            // 
            // btn_export_to_excel
            // 
            btn_export_to_excel.BackColor = SystemColors.Info;
            btn_export_to_excel.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_export_to_excel.Location = new Point(1016, 311);
            btn_export_to_excel.Name = "btn_export_to_excel";
            btn_export_to_excel.Size = new Size(78, 26);
            btn_export_to_excel.TabIndex = 16;
            btn_export_to_excel.Text = "Export";
            btn_export_to_excel.UseVisualStyleBackColor = false;
            btn_export_to_excel.Click += btn_export_to_excel_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox4);
            groupBox1.Controls.Add(richTextBoxSummary);
            groupBox1.Controls.Add(btn_export_to_excel);
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(richTextBoxCreditLoan);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(btnSummary);
            groupBox1.Controls.Add(richTextBoxPersonnelLoan);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(btnReset);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(btnAdd);
            groupBox1.Controls.Add(richTextBoxSavings);
            groupBox1.Controls.Add(btnSubtract);
            groupBox1.Controls.Add(label5);
            groupBox1.Location = new Point(12, 132);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1100, 617);
            groupBox1.TabIndex = 17;
            groupBox1.TabStop = false;
            groupBox1.Text = "Loan Section";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(label9);
            groupBox4.Controls.Add(richTextTotalBalance);
            groupBox4.Controls.Add(label8);
            groupBox4.Controls.Add(label7);
            groupBox4.Controls.Add(richTextRevolut);
            groupBox4.Controls.Add(richTextAib);
            groupBox4.Location = new Point(659, 22);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(435, 249);
            groupBox4.TabIndex = 20;
            groupBox4.TabStop = false;
            groupBox4.Text = "Balance ";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(192, 141);
            label9.Name = "label9";
            label9.Size = new Size(58, 15);
            label9.TabIndex = 10;
            label9.Text = "Salary (€)";
            // 
            // richTextTotalBalance
            // 
            richTextTotalBalance.BackColor = SystemColors.Menu;
            richTextTotalBalance.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextTotalBalance.Location = new Point(57, 178);
            richTextTotalBalance.Name = "richTextTotalBalance";
            richTextTotalBalance.ReadOnly = true;
            richTextTotalBalance.Size = new Size(321, 46);
            richTextTotalBalance.TabIndex = 9;
            richTextTotalBalance.Text = "";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(264, 29);
            label8.Name = "label8";
            label8.Size = new Size(66, 15);
            label8.TabIndex = 8;
            label8.Text = "Revolut(€)";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(75, 29);
            label7.Name = "label7";
            label7.Size = new Size(45, 15);
            label7.TabIndex = 7;
            label7.Text = "AIB (€)";
            // 
            // richTextRevolut
            // 
            richTextRevolut.BackColor = SystemColors.Menu;
            richTextRevolut.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextRevolut.Location = new Point(225, 47);
            richTextRevolut.Name = "richTextRevolut";
            richTextRevolut.ReadOnly = true;
            richTextRevolut.Size = new Size(192, 47);
            richTextRevolut.TabIndex = 6;
            richTextRevolut.Text = "";
            // 
            // richTextAib
            // 
            richTextAib.BackColor = SystemColors.Menu;
            richTextAib.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextAib.Location = new Point(0, 47);
            richTextAib.Name = "richTextAib";
            richTextAib.ReadOnly = true;
            richTextAib.Size = new Size(192, 47);
            richTextAib.TabIndex = 5;
            richTextAib.Text = "";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(btnLoandetails);
            groupBox3.Controls.Add(richTexthomeLoan);
            groupBox3.Location = new Point(14, 142);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(639, 129);
            groupBox3.TabIndex = 19;
            groupBox3.TabStop = false;
            groupBox3.Text = "Home Loan";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(269, 40);
            label6.Name = "label6";
            label6.Size = new Size(85, 15);
            label6.TabIndex = 7;
            label6.Text = "Home Loan(€)";
            // 
            // btnLoandetails
            // 
            btnLoandetails.BackColor = SystemColors.Info;
            btnLoandetails.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLoandetails.Location = new Point(561, 10);
            btnLoandetails.Name = "btnLoandetails";
            btnLoandetails.Size = new Size(78, 26);
            btnLoandetails.TabIndex = 6;
            btnLoandetails.Text = "Loan Details";
            btnLoandetails.UseVisualStyleBackColor = false;
            btnLoandetails.Click += btnDetails_Click;
            // 
            // richTexthomeLoan
            // 
            richTexthomeLoan.BackColor = SystemColors.Menu;
            richTexthomeLoan.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTexthomeLoan.Location = new Point(175, 58);
            richTexthomeLoan.Name = "richTexthomeLoan";
            richTexthomeLoan.ReadOnly = true;
            richTexthomeLoan.Size = new Size(321, 46);
            richTexthomeLoan.TabIndex = 3;
            richTexthomeLoan.Text = "";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(textBoxDays);
            groupBox2.Controls.Add(label1);
            groupBox2.Location = new Point(12, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(701, 114);
            groupBox2.TabIndex = 18;
            groupBox2.TabStop = false;
            groupBox2.Text = "Probation";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1124, 755);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Loan App";
            Load += Form1_Load_1;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox textBoxDays;
        private Label label1;
        private Label label2;
        private RichTextBox richTextBoxPersonnelLoan;
        private RichTextBox richTextBoxCreditLoan;
        private Label label3;
        private Label label4;
        private RichTextBox richTextBoxSavings;
        private Label label5;
        private Button btnSubtract;
        private Button btnAdd;
        private Button btnReset;
        private RichTextBox richTextBoxSummary;
        private Button btnSummary;
        private Button btn_export_to_excel;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private RichTextBox richTexthomeLoan;
        private Button btnLoandetails;
        private Label label6;
        private GroupBox groupBox4;
        private RichTextBox richTextRevolut;
        private RichTextBox richTextAib;
        private Label label8;
        private Label label7;
        private Label label9;
        private RichTextBox richTextTotalBalance;
    }
}
