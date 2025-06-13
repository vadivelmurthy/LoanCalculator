using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ProbationDaysApp
{
    public partial class LoanDetailsForm : Form
    {
        public LoanDetailsForm(string details)
        {
            InitializeComponent();
            txtDetails.Text = details;
        }
    }
}
