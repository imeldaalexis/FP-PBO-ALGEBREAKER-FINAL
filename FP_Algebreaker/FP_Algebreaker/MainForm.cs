namespace FP_Algebreaker
{
    public class MainForm : Form
    {
        private Button _startButton;
        private Button _exitButton;
        private GameForm _gameForm;

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
                Size = new Size(100, 40),
                Text = "Start",
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(300, 250)
            };
            _startButton.Click += StartButton_Click;
            this.Controls.Add(_startButton);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (_gameForm == null || _gameForm.IsDisposed)
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
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {

        }
    }
}
