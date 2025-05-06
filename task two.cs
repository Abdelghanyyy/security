using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace SmartCVValidator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private TextBox txtName, txtEmail, txtPhone, txtPassword, txtAddress, txtPostalCode, txtCVInput;
        private Button btnValidate, btnParseCV, btnSave;
        private Label lblResult;
        private RichTextBox rtbOutput;

        private void InitializeComponent()
        {
            this.Size = new Size(800, 600);
            this.Text = "Smart CV & Form Validator";

            Label lblName = new Label { Text = "Full Name:", Location = new Point(20, 20), Size = new Size(100, 20) };
            txtName = new TextBox { Location = new Point(120, 20), Size = new Size(200, 20) };

            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, 50), Size = new Size(100, 20) };
            txtEmail = new TextBox { Location = new Point(120, 50), Size = new Size(200, 20) };

            Label lblPhone = new Label { Text = "Phone:", Location = new Point(20, 80), Size = new Size(100, 20) };
            txtPhone = new TextBox { Location = new Point(120, 80), Size = new Size(200, 20) };

            Label lblPassword = new Label { Text = "Password:", Location = new Point(20, 110), Size = new Size(100, 20) };
            txtPassword = new TextBox { Location = new Point(120, 110), Size = new Size(200, 20), PasswordChar = '*' };

            Label lblAddress = new Label { Text = "Address:", Location = new Point(20, 140), Size = new Size(100, 20) };
            txtAddress = new TextBox { Location = new Point(120, 140), Size = new Size(200, 20) };

            Label lblPostalCode = new Label { Text = "Postal Code:", Location = new Point(20, 170), Size = new Size(100, 20) };
            txtPostalCode = new TextBox { Location = new Point(120, 170), Size = new Size(200, 20) };

            btnValidate = new Button { Text = "Validate Form", Location = new Point(120, 200), Size = new Size(100, 30) };
            btnValidate.Click += BtnValidate_Click;

            Label lblCV = new Label { Text = "Paste CV Text:", Location = new Point(20, 240), Size = new Size(100, 20) };
            txtCVInput = new TextBox { Location = new Point(120, 240), Size = new Size(600, 100), Multiline = true, ScrollBars = ScrollBars.Vertical };

            btnParseCV = new Button { Text = "Parse CV", Location = new Point(120, 350), Size = new Size(100, 30) };
            btnParseCV.Click += BtnParseCV_Click;

            btnSave = new Button { Text = "Save to File", Location = new Point(230, 350), Size = new Size(100, 30) };
            btnSave.Click += BtnSave_Click;

            lblResult = new Label { Text = "Results:", Location = new Point(20, 390), Size = new Size(100, 20) };
            rtbOutput = new RichTextBox { Location = new Point(120, 390), Size = new Size(600, 150), ReadOnly = true };

            this.Controls.AddRange(new Control[] {
                lblName, txtName, lblEmail, txtEmail, lblPhone, txtPhone, lblPassword, txtPassword,
                lblAddress, txtAddress, lblPostalCode, txtPostalCode, btnValidate,
                lblCV, txtCVInput, btnParseCV, btnSave, lblResult, rtbOutput
            });
        }

        private void BtnValidate_Click(object sender, EventArgs e)
        {
            rtbOutput.Clear();
            bool isValid = true;
            ResetFieldColors();

            if (!Regex.IsMatch(txtName.Text, @"^[\p{L}\s]{2,100}$"))
            {
                txtName.BackColor = Color.LightPink;
                rtbOutput.AppendText("Invalid Name. Use only letters and spaces (2-100 characters).\n");
                isValid = false;
            }

            if (!Regex.IsMatch(txtEmail.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                txtEmail.BackColor = Color.LightPink;
                rtbOutput.AppendText("Invalid Email format.\n");
                isValid = false;
            }

            if (!Regex.IsMatch(txtPhone.Text, @"^\+?\d{10,15}$|^(\d{3}-){2}\d{4}$"))
            {
                txtPhone.BackColor = Color.LightPink;
                rtbOutput.AppendText("Invalid Phone number. Use +1234567890 or 123-456-7890 format.\n");
                isValid = false;
            }

            if (!Regex.IsMatch(txtPassword.Text, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%?&])[A-Za-z\d@$!%?&]{8,}$"))
            {
                txtPassword.BackColor = Color.LightPink;
                rtbOutput.AppendText("Invalid Password. Must be 8+ chars with uppercase, lowercase, number, and special character.\n");
                isValid = false;
            }

            if (!Regex.IsMatch(txtAddress.Text, @"^[\p{L}\d\s,.-]{5,200}$"))
            {
                txtAddress.BackColor = Color.LightPink;
                rtbOutput.AppendText("Invalid Address. Use letters, numbers, spaces, and basic punctuation (5-200 chars).\n");
                isValid = false;
            }

            if (!Regex.IsMatch(txtPostalCode.Text, @"^\d{5}(-\d{4})?$"))
            {
                txtPostalCode.BackColor = Color.LightPink;
                rtbOutput.AppendText("Invalid Postal Code. Use 12345 or 12345-6789 format.\n");
                isValid = false;
            }

            if (isValid)
            {
                rtbOutput.AppendText("All fields are valid!\n");
                rtbOutput.AppendText($"Name: {txtName.Text}\nEmail: {txtEmail.Text}\nPhone: {txtPhone.Text}\n" +
                                     $"Address: {txtAddress.Text}\nPostal Code: {txtPostalCode.Text}");
            }
        }

        private void BtnParseCV_Click(object sender, EventArgs e)
        {
            rtbOutput.Clear();
            string cvText = txtCVInput.Text;

            string namePattern = @"^[A-Za-z\s]+|^[\p{L}\s]+";
            Match nameMatch = Regex.Match(cvText, namePattern, RegexOptions.Multiline);
            string name = nameMatch.Success ? nameMatch.Value.Trim() : "Not found";

            string emailPattern = @"[\w-\.]+@([\w-]+\.)+[\w-]{2,4}";
            Match emailMatch = Regex.Match(cvText, emailPattern);
            string email = emailMatch.Success ? emailMatch.Value : "Not found";

            string phonePattern = @"\+?\d{10,15}|(\d{3}-){2}\d{4}";
            Match phoneMatch = Regex.Match(cvText, phonePattern);
            string phone = phoneMatch.Success ? phoneMatch.Value : "Not found";

            string skillsPattern = @"\b(C#|Java|SQL|Python|JavaScript|HTML|CSS|React|Angular|Node\.js)\b";
            MatchCollection skillsMatches = Regex.Matches(cvText, skillsPattern, RegexOptions.IgnoreCase);
            string skills = skillsMatches.Count > 0 ? string.Join(", ", skillsMatches.Cast<Match>().Select(m => m.Value)) : "Not found";

            string experiencePattern = @"(\d+)\s*(years?|yrs?)\s*(of)?\s*experience";
            Match experienceMatch = Regex.Match(cvText, experiencePattern, RegexOptions.IgnoreCase);
            string experience = experienceMatch.Success ? experienceMatch.Value : "Not found";

            rtbOutput.AppendText("Parsed CV Results:\n");
            rtbOutput.AppendText($"Full Name: {name}\n");
            rtbOutput.AppendText($"Email: {email}\n");
            rtbOutput.AppendText($"Phone: {phone}\n");
            rtbOutput.AppendText($"Skills: {skills}\n");
            rtbOutput.AppendText($"Experience: {experience}\n");
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv";
                sfd.DefaultExt = "txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string content = rtbOutput.Text;
                    if (sfd.FilterIndex == 2)
                    {
                        content = "Field,Value\n" + content.Replace("\n", "\nField,");
                    }
                    File.WriteAllText(sfd.FileName, content);
                    MessageBox.Show("Data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ResetFieldColors()
        {
            txtName.BackColor = SystemColors.Window;
            txtEmail.BackColor = SystemColors.Window;
            txtPhone.BackColor = SystemColors.Window;
            txtPassword.BackColor = SystemColors.Window;
            txtAddress.BackColor = SystemColors.Window;
            txtPostalCode.BackColor = SystemColors.Window;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
