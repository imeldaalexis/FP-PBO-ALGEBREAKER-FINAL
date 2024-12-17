using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Algebreaker
{
    public class HighScoreForm : Form
    {
        private const int FormHeight = 604;
        private const int FormWidth = 992;
        private Label _highScoreTimeLabel = new Label();
        private Label _highScoreKillLabel = new Label();

        private int _currentTimeHighScore = 0;
        private const string HighScoreFilePath = @"Assets\HighScore.txt";
        private int _gameFormTimeScore;
        private int _gameFormKillScore;
        private int _labelXPoint = 300;
        private int _labelYPointTop = 200;

        public HighScoreForm()
        {
            InitializeHighScores();
            InitializeForm();
        }

        private void InitializeHighScores()
        {
            if (File.Exists(HighScoreFilePath))
            {
                try
                {
                    string[] scores = File.ReadAllLines(HighScoreFilePath);
                    if (scores.Length >= 2)
                    {
                        _gameFormTimeScore = int.Parse(scores[0]);
                        _gameFormKillScore = int.Parse(scores[1]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading high scores: " + ex.Message);
                }
            }
            else
            {
                Debug.WriteLine("File doesn't exist");
                // If the file doesn't exist, initialize scores to 0 and create the file
                _gameFormTimeScore = 0;
                _gameFormKillScore = 0;
                File.WriteAllLines(HighScoreFilePath, new string[] { "0", "0" });
            }
        }

        private void InitializeForm()
        {
            this.Text = "High Score";
            this.Size = new Size(FormWidth, FormHeight);
            this.BackgroundImage = Image.FromFile(@"Assets\HighScoreBackground.png");
            this.StartPosition = FormStartPosition.CenterScreen;

            // Label: Time Score
            _highScoreTimeLabel = new Label
            {
                Text = $"⏰ Time Score: {_gameFormTimeScore}",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Location = new Point(_labelXPoint - 20, _labelYPointTop),
                AutoSize = true,
                BackColor = Color.White
            };

            // Label: Kill Score
            _highScoreKillLabel = new Label
            {
                Text = $"🔫 Kill Score: {_gameFormKillScore}",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Location = new Point(_labelXPoint, _labelYPointTop + 100),
                AutoSize = true,
                BackColor = Color.White
            };

            // Add labels to the form
            this.Controls.Add(_highScoreTimeLabel);
            this.Controls.Add(_highScoreKillLabel);
        }
    }
}
