using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace CaptchaGenerator
{
    public partial class Form1 : Form
    {
        private string textCaptchaAnswer;
        private string mathCaptchaAnswer;
        private string imageCaptchaAnswer;
        private bool recaptchaChecked;

        public Form1()
        {
            InitializeComponent();
        }

        private ComboBox cbCaptchaType;
        private Label lblInstruction, lblResult;
        private TextBox txtUserInput;
        private PictureBox pbImageCaptcha;
        private Button btnSubmit, btnRefresh;
        private CheckBox chkRecaptcha;
        private Panel panelCaptcha;

        private void InitializeComponent()
        {
            this.Size = new Size(500, 400);
            this.Text = "CAPTCHA Generator";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Label lblType = new Label { Text = "Select CAPTCHA Type:", Location = new Point(20, 20), Size = new Size(120, 20) };
            cbCaptchaType = new ComboBox { Location = new Point(140, 20), Size = new Size(200, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            cbCaptchaType.Items.AddRange(new string[] { "Text-Based", "Image-Based", "Math-Based", "reCAPTCHA Checkbox" });
            cbCaptchaType.SelectedIndex = 0;
            cbCaptchaType.SelectedIndexChanged += CbCaptchaType_SelectedIndexChanged;

            lblInstruction = new Label { Location = new Point(20, 50), Size = new Size(440, 40), Text = "Enter the text shown in the CAPTCHA." };
            panelCaptcha = new Panel { Location = new Point(20, 100), Size = new Size(440, 100), BorderStyle = BorderStyle.FixedSingle };

            txtUserInput = new TextBox { Location = new Point(20, 210), Size = new Size(200, 20), Visible = true };
            pbImageCaptcha = new PictureBox { Location = new Point(10, 10), Size = new Size(420, 80), Visible = false, SizeMode = PictureBoxSizeMode.StretchImage };
            chkRecaptcha = new CheckBox { Text = "I'm not a robot", Location = new Point(20, 40), Size = new Size(200, 20), Visible = false };
            chkRecaptcha.CheckedChanged += ChkRecaptcha_CheckedChanged;

            btnSubmit = new Button { Text = "Submit", Location = new Point(20, 240), Size = new Size(100, 30) };
            btnSubmit.Click += BtnSubmit_Click;

            btnRefresh = new Button { Text = "Refresh", Location = new Point(130, 240), Size = new Size(100, 30) };
            btnRefresh.Click += BtnRefresh_Click;

            lblResult = new Label { Location = new Point(20, 280), Size = new Size(440, 60), ForeColor = Color.Black };

            panelCaptcha.Controls.AddRange(new Control[] { txtUserInput, pbImageCaptcha, chkRecaptcha });
            this.Controls.AddRange(new Control[] { lblType, cbCaptchaType, lblInstruction, panelCaptcha, btnSubmit, btnRefresh, lblResult });

            GenerateCaptcha();
        }

        private void CbCaptchaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUserInput.Visible = cbCaptchaType.SelectedIndex != 3;
            pbImageCaptcha.Visible = cbCaptchaType.SelectedIndex == 1;
            chkRecaptcha.Visible = cbCaptchaType.SelectedIndex == 3;
            txtUserInput.Text = "";
            chkRecaptcha.Checked = false;
            lblResult.Text = "";
            GenerateCaptcha();
        }

        private void GenerateCaptcha()
        {
            switch (cbCaptchaType.SelectedIndex)
            {
                case 0:
                    GenerateTextCaptcha();
                    lblInstruction.Text = "Enter the text shown in the CAPTCHA.";
                    break;
                case 1:
                    GenerateImageCaptcha();
                    lblInstruction.Text = "Enter the text shown in the image.";
                    break;
                case 2:
                    GenerateMathCaptcha();
                    lblInstruction.Text = "Solve the math problem and enter the answer.";
                    break;
                case 3:
                    GenerateRecaptcha();
                    lblInstruction.Text = "Check the box to verify you are not a robot.";
                    break;
            }
        }

        private void GenerateTextCaptcha()
        {
            Random rand = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder captcha = new StringBuilder();
            for (int i = 0; i < 6; i++)
                captcha.Append(chars[rand.Next(chars.Length)]);
            textCaptchaAnswer = captcha.ToString();

            panelCaptcha.Controls.RemoveByKey("lblTextCaptcha");
            Label lblTextCaptcha = new Label
            {
                Name = "lblTextCaptcha",
                Text = textCaptchaAnswer,
                Location = new Point(10, 40),
                Size = new Size(420, 20),
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelCaptcha.Controls.Add(lblTextCaptcha);
        }

        private void GenerateImageCaptcha()
        {
            Random rand = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder captcha = new StringBuilder();
            for (int i = 0; i < 6; i++)
                captcha.Append(chars[rand.Next(chars.Length)]);
            imageCaptchaAnswer = captcha.ToString();

            using (Bitmap bmp = new Bitmap(420, 80))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.DrawString(imageCaptchaAnswer, new Font("Arial", 20, FontStyle.Bold), Brushes.Black, new PointF(150, 30));
                for (int i = 0; i < 100; i++)
                    bmp.SetPixel(rand.Next(bmp.Width), rand.Next(bmp.Height), Color.Gray);
                pbImageCaptcha.Image = (Bitmap)bmp.Clone();
            }
        }

        private void GenerateMathCaptcha()
        {
            Random rand = new Random();
            int num1 = rand.Next(1, 20);
            int num2 = rand.Next(1, 20);
            char[] operators = { '+', '-', '*' };
            char op = operators[rand.Next(operators.Length)];

            switch (op)
            {
                case '+': mathCaptchaAnswer = (num1 + num2).ToString(); break;
                case '-': mathCaptchaAnswer = (num1 - num2).ToString(); break;
                case '*': mathCaptchaAnswer = (num1 * num2).ToString(); break;
            }

            panelCaptcha.Controls.RemoveByKey("lblMathCaptcha");
            Label lblMathCaptcha = new Label
            {
                Name = "lblMathCaptcha",
                Text = $"{num1} {op} {num2} = ?",
                Location = new Point(10, 40),
                Size = new Size(420, 20),
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelCaptcha.Controls.Add(lblMathCaptcha);
        }

        private void GenerateRecaptcha()
        {
            recaptchaChecked = false;
            chkRecaptcha.Checked = false;
        }

        private void ChkRecaptcha_CheckedChanged(object sender, EventArgs e)
        {
            recaptchaChecked = chkRecaptcha.Checked;
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            lblResult.ForeColor = Color.Black;
            switch (cbCaptchaType.SelectedIndex)
            {
                case 0:
                    lblResult.Text = txtUserInput.Text.Equals(textCaptchaAnswer, StringComparison.OrdinalIgnoreCase) ? "Success! CAPTCHA verified." : "Error: Incorrect CAPTCHA text.";
                    break;
                case 1:
                    lblResult.Text = txtUserInput.Text.Equals(imageCaptchaAnswer, StringComparison.OrdinalIgnoreCase) ? "Success! CAPTCHA verified." : "Error: Incorrect CAPTCHA text.";
                    break;
                case 2:
                    lblResult.Text = txtUserInput.Text == mathCaptchaAnswer ? "Success! Correct answer." : "Error: Incorrect answer.";
                    break;
                case 3:
                    lblResult.Text = recaptchaChecked ? "Success! Verified as human." : "Error: Please check the box.";
                    break;
            }
            lblResult.ForeColor = lblResult.Text.StartsWith("Success") ? Color.Green : Color.Red;
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtUserInput.Text = "";
            chkRecaptcha.Checked = false;
            lblResult.Text = "";
            GenerateCaptcha();
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
