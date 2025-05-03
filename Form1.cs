using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RegexValidatorApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            patternComboBox.Items.Add("Email");
            patternComboBox.Items.Add("Phone");
            patternComboBox.Items.Add("Postal Code");
            patternComboBox.SelectedIndex = 0;
        }

        private void validateButton_Click(object sender, EventArgs e)
        {
            string input = inputTextBox.Text.Trim();
            string pattern = "";
            string selectedType = patternComboBox.SelectedItem.ToString();

            switch (selectedType)
            {
                case "Email":
                    pattern = @"^[\w\.-]+@[\w\.-]+\.\w{2,}$";
                    break;
                case "Phone":
                    pattern = @"^\+?[0-9\s\-]{7,15}$";
                    break;
                case "Postal Code":
                    pattern = @"^\d{5}(-\d{4})?$";
                    break;
            }

            if (Regex.IsMatch(input, pattern))
            {
                resultLabel.Text = $"✅ {selectedType} is valid.";
                resultLabel.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                resultLabel.Text = $"❌ Invalid {selectedType}. Please check your input.";
                resultLabel.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
