using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Algebreaker
{
    public class MathForm : Form
    {
        private const int FormHeight = 604;
        private const int FormWidth = 992;
        private Label _soalMath;
        private SoalAlgebraic _soalAlgebraic;
        private TextBox _answerTextBox;
        private Button _submitButton;


        public MathForm()
        {
            InitializeForm();
            InitializeSoalMat();
        }

        public void InitializeForm()
        {
            this.Text = "Do the Math to Reload!";
            this.Size = new Size(FormWidth, FormHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackgroundImage = Image.FromFile(@"Assets\MathFormBackground.png");
        }

        public void InitializeSoalMat()
        {
            _soalAlgebraic = new SoalAlgebraic();
            _answerTextBox = new TextBox
            {
                Width = 200,
                Location = new Point(FormWidth / 2 - 140, FormHeight / 2 - 50),
            };
            _submitButton = new Button
            {
                Width = 200,
                Height = 50,
                Text = "Submit",
                Location = new Point(FormWidth / 2 - 140, (FormHeight / 2)),

            };

            _submitButton.Click += SubmitButton_Click;

            this.Controls.Add(_answerTextBox);
            this.Controls.Add(_submitButton);

            Label questionLabel = new Label
            {
                BackColor = Color.White,
                Text = _soalAlgebraic.GetExpression(),
                Width = 400,
                Location = new Point((FormWidth / 2 - 120), (FormHeight / 2 - 100))
            };

            this.Controls.Add(questionLabel);
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            int playerAnswer;

            if (int.TryParse(_answerTextBox.Text, out playerAnswer))
            {

                if (playerAnswer == _soalAlgebraic.x)
                {
                    Bullet._currentAmmo = 10;
                    MessageBox.Show("Correct! Returning to the game...");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect. Try again!");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }
    };
}
