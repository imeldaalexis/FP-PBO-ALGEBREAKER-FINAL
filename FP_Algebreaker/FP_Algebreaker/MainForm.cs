namespace FP_Algebreaker
{
    public class MainForm : Form
    {
        private Button _startButton;
        private Button _exitButton;
        private GameForm _gameForm;
        private HighScoreForm _highScoreForm;
        private Button _highScoreButton;
        private int _topButtonStartPosition = 190;
        private int _xOfAllButton = 300;

        public MainForm()
        {
            InitializeForm();
            InitializeControl();
        }

        private void InitializeForm()
        {
            this.Text = "Main Menu";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeControl()
        {
            _startButton = new Button
            {
                Size = new Size(150, 40),
                Text = "Start",
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(_xOfAllButton, _topButtonStartPosition)
            };
            _startButton.Click += StartButton_Click;
            this.Controls.Add(_startButton);

            _highScoreButton = new Button
            {
                Size = new Size(150, 40),
                Text = "High Score",
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(_xOfAllButton, _topButtonStartPosition + 50)
            };
            _highScoreButton.Click += HighScoreButton_Click;
            this.Controls.Add(_highScoreButton);

            _exitButton = new Button
            {
                Size = new Size(150, 40),
                Text = "Exit",
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(_xOfAllButton, _topButtonStartPosition + 100)
            };
            _exitButton.Click += ExitButton_Click;
            this.Controls.Add(_exitButton);


        }

        private void StartButton_Click(object sender, EventArgs e)
        {
                _gameForm = new GameForm();
                _gameForm.FormClosed += (s, args) =>
                {
                    this.Show();
                    _gameForm.Dispose();
                    _gameForm = null; // Release reference
                };
                _gameForm.Show();
                this.Hide();
        }

        private void HighScoreButton_Click (object sender, EventArgs e)
        {
                _highScoreForm = new HighScoreForm();
                _highScoreForm.ShowDialog();
        }


        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
