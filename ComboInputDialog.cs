using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbationDaysApp
{
    public class ComboInputDialog: Form
    {
        public string SelectedItem { get; private set; }
        private ComboBox combo;
        private Button okButton;

        public ComboInputDialog(string[] options, string title)
        {
            this.Text = title;
            combo = new ComboBox() { DataSource = options, Dock = DockStyle.Top };
            okButton = new Button() { Text = "OK", Dock = DockStyle.Bottom };
            okButton.Click += (s, e) => { SelectedItem = combo.SelectedItem.ToString(); this.DialogResult = DialogResult.OK; Close(); };
            Controls.Add(combo);
            Controls.Add(okButton);
            StartPosition = FormStartPosition.CenterParent;
            Width = 250;
            Height = 100;
        }
    }
}
