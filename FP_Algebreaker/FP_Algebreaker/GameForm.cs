using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;


namespace FP_Algebreaker
{
    public class GameForm : Form
    {
        private const int FormHeight = 604;
        private const int FormWidth = 992;
        private const int _PlayerStartingPositionX = 32;
        private const int _PlayerStartingPositionY = 32;
        private const int _AnimationInterval = 100;

        private Image _backgroundLevel = Image.FromFile(@"Assets\BackgroundLevel.png");
        private Player _mainCharacter;
        private List<Alien> _aliens = new List<Alien>();
        private Timer _animationTimer;
        private MathForm _mathForm;


        public GameForm()
        {
            InitializeForm();
            InitializeControl();
            this.DoubleBuffered = true;
        }

        private void InitializeForm()
        {
            this.Text = "Algebreaker";
            this.Size = new Size(FormWidth, FormHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            _animationTimer = new Timer
            {
                Interval = _AnimationInterval
            };
            // Everytime it ticks, return render
            _animationTimer.Tick += (sender, e) => Render();
            _animationTimer.Start();

            this.KeyDown += onKeyDown;
            this.KeyUp += onKeyUp;
        }

        private void InitializeControl()
        {
            _mainCharacter = new Player(
                 new Point(_PlayerStartingPositionX, _PlayerStartingPositionY)
             );
            var pictureBoxes = _mainCharacter.GetPictureBoxes();
            var healthBar = _mainCharacter.GetHealthBar(); // Dapatkan ProgressBar
            
            PictureBox playerBox = pictureBoxes[0];
            PictureBox gunBox = pictureBoxes[1];

            this.Controls.Add(playerBox);
            this.Controls.Add(gunBox);

            // Tambahkan health label ke dalam Form
            this.Controls.Add(healthBar);

            // Perbarui posisi awal health label
            _mainCharacter.UpdateHealthBarPosition();

            // Menambahkan beberapa alien ke dalam permainan
            AddAliens();
        }

        private void AddAliens()
        {
            // Menambahkan beberapa jenis alien
            _aliens.Add(new RookieAlien(new Point(200, 200)));
            _aliens.Add(new KacynzkiAlien(new Point(400, 400)));

            // Menambahkan EldritchHorrorAlien dan memberikan parameter Player (_mainCharacter)
            _aliens.Add(new EldritchHorrorAlien(new Point(600, 100), _mainCharacter));

            // Menambahkan PictureBox untuk masing-masing alien
            foreach (var alien in _aliens)
            {
                this.Controls.Add(alien.AlienPictureBox);  // Menambahkan alien ke dalam form
                this.Controls.Add(alien.HealthBar);

                alien.UpdateHealthBarPosition();
            }
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            // Kalau ada pencetan R, load _mathForm
            if (e.KeyCode == Keys.R)
            {
                if (_mathForm == null || _mathForm.IsDisposed)
                {
                    _mainCharacter.StopWalking();
                    MathForm _mathForm = new MathForm();
                    _mathForm.ShowDialog();
                    // Hide the GameForm when MathForm is shown
                }
            }
            _mainCharacter.Walk(e.KeyCode, this.ClientSize, this);
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            _mainCharacter.StopWalking();
        }

        //Untuk render animasi
        private void Render()
        {
            this.Invalidate();
            _mainCharacter.Animate();

            // Memperbarui posisi alien dan mengejar pemain
            foreach (var alien in _aliens)
            {
                alien.ChasePlayer(_mainCharacter.GetPictureBoxes()[0].Location);

                if (alien is RookieAlien rookie)
                {
                    rookie.PhysicalAttack(_mainCharacter); // Memanggil metode PhysicalAttack jika alien ini adalah RookieAlien
                }
                else if (alien is KacynzkiAlien kacynzki)
                {
                    kacynzki.BombAttack(_mainCharacter); // Memanggil metode BombAttack jika alien ini adalah KacynzkiAlien
                }
                else if (alien is EldritchHorrorAlien eldritchHorror)
                {
                    eldritchHorror.PhysicalAttack(_mainCharacter); // Memanggil metode BombAttack jika alien ini adalah KacynzkiAlien
                }

                if (alien.IsAlive())
                {
                    alien.UpdateHealthBarPosition();
                }
            }
        }

        //Ini supaya background tidak digambar ulang per frame render
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);  // Call the base method to make sure form is properly painted

            // Draw the background
            e.Graphics.DrawImage(_backgroundLevel, 0, 0, this.ClientSize.Width, this.ClientSize.Height);

        }

        //Pause Button goes here
        /* the idea is to stop the time ticking, and then make a 
         * rectangle with the size of the client, make the opacity 20% and make a button called
         Continue Button */
    }
}
