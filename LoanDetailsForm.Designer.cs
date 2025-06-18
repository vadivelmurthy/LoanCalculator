namespace ProbationDaysApp
{
    partial class LoanDetailsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            rtbSummary = new RichTextBox();
            txtExtraPayment = new RichTextBox();
            rtbImpact = new RichTextBox();
            btnSeeImpact = new Button();
            txtExtraPaymentPostFixed = new RichTextBox();
            btnSeeImpactPostFixed = new Button();
            SuspendLayout();
            // 
            // rtbSummary
            // 
            rtbSummary.Location = new Point(14, 35);
            rtbSummary.Margin = new Padding(3, 4, 3, 4);
            rtbSummary.Name = "rtbSummary";
            rtbSummary.Size = new Size(674, 613);
            rtbSummary.TabIndex = 0;
            rtbSummary.Text = "";
            // 
            // txtExtraPayment
            // 
            txtExtraPayment.BackColor = SystemColors.Menu;
            txtExtraPayment.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtExtraPayment.Location = new Point(695, 35);
            txtExtraPayment.Margin = new Padding(3, 4, 3, 4);
            txtExtraPayment.Name = "txtExtraPayment";
            txtExtraPayment.Size = new Size(219, 61);
            txtExtraPayment.TabIndex = 5;
            txtExtraPayment.Text = "";
            // 
            // rtbImpact
            // 
            rtbImpact.Location = new Point(920, 35);
            rtbImpact.Name = "rtbImpact";
            rtbImpact.Size = new Size(675, 613);
            rtbImpact.TabIndex = 11;
            rtbImpact.Text = "";
            // 
            // btnSeeImpact
            // 
            btnSeeImpact.BackColor = SystemColors.Info;
            btnSeeImpact.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSeeImpact.Location = new Point(728, 124);
            btnSeeImpact.Margin = new Padding(3, 4, 3, 4);
            btnSeeImpact.Name = "btnSeeImpact";
            btnSeeImpact.Size = new Size(144, 53);
            btnSeeImpact.TabIndex = 8;
            btnSeeImpact.Text = "Impact";
            btnSeeImpact.UseVisualStyleBackColor = false;
            // 
            // txtExtraPaymentPostFixed
            // 
            txtExtraPaymentPostFixed.BackColor = SystemColors.Menu;
            txtExtraPaymentPostFixed.Enabled = false;
            txtExtraPaymentPostFixed.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtExtraPaymentPostFixed.Location = new Point(695, 225);
            txtExtraPaymentPostFixed.Margin = new Padding(3, 4, 3, 4);
            txtExtraPaymentPostFixed.Name = "txtExtraPaymentPostFixed";
            txtExtraPaymentPostFixed.Size = new Size(219, 61);
            txtExtraPaymentPostFixed.TabIndex = 9;
            txtExtraPaymentPostFixed.Text = "";
            // 
            // btnSeeImpactPostFixed
            // 
            btnSeeImpactPostFixed.BackColor = SystemColors.Info;
            btnSeeImpactPostFixed.Enabled = false;
            btnSeeImpactPostFixed.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSeeImpactPostFixed.Location = new Point(728, 333);
            btnSeeImpactPostFixed.Margin = new Padding(3, 4, 3, 4);
            btnSeeImpactPostFixed.Name = "btnSeeImpactPostFixed";
            btnSeeImpactPostFixed.Size = new Size(144, 53);
            btnSeeImpactPostFixed.TabIndex = 10;
            btnSeeImpactPostFixed.Text = "Impact";
            btnSeeImpactPostFixed.UseVisualStyleBackColor = false;
            btnSeeImpactPostFixed.Click += btnSeeImpactPostFixed_Click_1;
            // 
            // LoanDetailsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1607, 863);
            Controls.Add(btnSeeImpactPostFixed);
            Controls.Add(txtExtraPaymentPostFixed);
            Controls.Add(btnSeeImpact);
            Controls.Add(rtbImpact);
            Controls.Add(txtExtraPayment);
            Controls.Add(rtbSummary);
            Margin = new Padding(3, 4, 3, 4);
            Name = "LoanDetailsForm";
            Text = "Loan Details";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox rtbSummary;
        private RichTextBox txtExtraPayment;
        private RichTextBox rtbImpact;
        private Button btnSeeImpact;
        private RichTextBox txtExtraPaymentPostFixed;
        private Button btnSeeImpactPostFixed;
    }
}