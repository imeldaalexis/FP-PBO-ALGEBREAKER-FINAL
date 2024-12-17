using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Algebreaker
{
    public class Player : IHealth
    {
        private const int PlayerWidth = 24; // const meaning that you have to set up value for this attribute IMMEDIATELY
        private const int PlayerHeight = 32;
        private const int _totalFrame = 8;
        private Gun _playerGun;
        private PictureBox _playerPictureBox;
        private Image _spriteSheet = Image.FromFile(@"Assets\rpg_sprite_walk.png");
        private int _currentFrame;
        private int _currentRow;
        private bool _isMoving;
        private MathForm _mathForm;
        private ProgressBar _healthBar; // ProgressBar untuk menampilkan health
        public ProgressBar HealthBar => _healthBar;
        private Label _ammoLabel;

        // Properti Health
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }

        public Player(Point startPosition)
        {

            _currentFrame = 0;
            _currentRow = 0;

            _playerPictureBox = new PictureBox
            {
                Size = new Size(48, 64),
                Location = startPosition,
                BackColor = Color.Transparent,
                Image = _spriteSheet,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            _playerGun = new Gun(startPosition);

            // Inisialisasi health
            MaxHealth = 1000; // Bisa disesuaikan
            CurrentHealth = MaxHealth;

            _healthBar = new ProgressBar
            {
                Maximum = MaxHealth,
                Value = CurrentHealth,
                Size = new Size(100, 10), // Lebar dan tinggi bar
                BackColor = Color.Gray,
                ForeColor = Color.Green
            };

            _ammoLabel = new Label
            {
                Text = $"Ammo: {Bullet._currentAmmo}/{Bullet._maxAmmo}",
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Arial", 8, FontStyle.Bold)
            };

            UpdateAmmoLabelPosition();

            UpdateSprite();
        }

        public List<PictureBox> GetPictureBoxes()
        {
            return new List<PictureBox> { _playerPictureBox, _playerGun.GetPictureBox() };
        }

        public PictureBox GetGunPictureBox() => _playerGun.GetPictureBox();

        public ProgressBar GetHealthBar() => _healthBar;

        // Implementasi Interface IHealth
        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
                Die();  // Call the Die method if health reaches 0
            }
            Debug.WriteLine($"Player took {damage} damage. Current Health: {CurrentHealth}");
            UpdateHealthBar();
        }

        private void Die()
        {
            // Handle death logic, like hiding the player or alien
            Debug.WriteLine("Character has died");
            _playerPictureBox.Hide();
            HealthBar.Hide();
        }

        public void Heal(int amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
            Debug.WriteLine($"Player healed by {amount}. Current Health: {CurrentHealth}");
            UpdateHealthBar();
        }

        public bool IsAlive()
        {
            return CurrentHealth > 0;
        }

        public void Walk(Keys key, Size boundary, Form form, List<Alien> aliens)
        {
            int speed = 10;
            _isMoving = true; // this will be used in Animate(), to signal that yes, I am moving, please animate me

            switch (key)
            {
                case Keys.Down:
                    _currentRow = 0;
                    if (_playerPictureBox.Bottom < boundary.Height)
                    {
                        _playerPictureBox.Top += speed;
                    }
                    break;
                case Keys.Up:
                    _currentRow = 1;
                    if (_playerPictureBox.Top > 0)
                    {
                        _playerPictureBox.Top -= speed;
                    }
                    break;
                case Keys.Left:
                    _currentRow = 2;
                    if (_playerPictureBox.Left > 0)
                    {
                        _playerPictureBox.Left -= speed;
                    }
                    break;
                case Keys.Right:
                    _currentRow = 3;
                    if (_playerPictureBox.Right < boundary.Width)
                    {
                        _playerPictureBox.Left += speed;
                    }
                    break;
                case Keys.Enter:
                    _playerGun.ShootManual(form, _playerPictureBox.Location, _currentRow, aliens, this);  // Pass form to add control for bullets
                    break;
                case Keys.R:
                default:
                    _isMoving = false;
                    break;

            }
            UpdateHealthBarPosition();
            UpdateAmmoLabelPosition();
            UpdateSprite();
        }

        public void StopWalking()
        {
            _isMoving = false;
            _currentFrame = 0; // so it can be the standing emoji state again
            UpdateSprite();
        }

        public void Animate()
        {
            if (_isMoving)
            {
                //when you move, the index is gonna change, it will keep going left
                _currentFrame = (_currentFrame + 1) % _totalFrame; // dimodul so it can go back to index 0 like a loop 
                UpdateSprite();
            }
        }

        public void UpdateSprite()
        {
            int frameWidth = PlayerWidth;
            int frameHeight = PlayerHeight;

            //Protocol: take the starting x axis, starting y axis and then drag  the frame width and height to make the rectangle 
            Rectangle srcRect = new Rectangle(_currentFrame * PlayerWidth, _currentRow * PlayerHeight, frameWidth, frameHeight);

            //Make it into object of map
            Bitmap currentFrameImage = new Bitmap(frameWidth, frameHeight);

            //Slicer
            using (Graphics g = Graphics.FromImage(currentFrameImage))
            {
                g.DrawImage(_spriteSheet, new Rectangle(0, 0, frameWidth, frameHeight), srcRect, GraphicsUnit.Pixel);
            }

            //Update the Image inside picture Box
            _playerPictureBox.Image = currentFrameImage;
        }

        public void UpdateHealthBar()
        {
            _healthBar.Value = Math.Max(0, Math.Min(CurrentHealth, MaxHealth));
            Debug.WriteLine($"Updated health bar: {CurrentHealth}/{MaxHealth}");
        }

        public void UpdateHealthBarPosition()
        {
            // Posisi label di atas player
            _healthBar.Size = new Size(80, 12);
            _healthBar.ForeColor = Color.LimeGreen;
            _healthBar.Location = new Point(
                _playerPictureBox.Left + (_playerPictureBox.Width / 2) - (_healthBar.Width / 2),
                _playerPictureBox.Top - 20
            );
        }

        // Method untuk memperbarui posisi label di bawah pemain
        public void UpdateAmmoLabelPosition()
        {
            _ammoLabel.Location = new Point(
                _playerPictureBox.Left + (_playerPictureBox.Width / 2) - (_ammoLabel.Width / 2),
                _playerPictureBox.Bottom + 5
            );
        }

        // Method untuk memperbarui teks label ammo
        public void UpdateAmmoLabel()
        {
            _ammoLabel.Text = $"Ammo: {Bullet._currentAmmo}/{Bullet._maxAmmo}";
            UpdateAmmoLabelPosition();
        }

        // Method untuk mendapatkan label ammo
        public Label GetAmmoLabel() => _ammoLabel;

    }
}
