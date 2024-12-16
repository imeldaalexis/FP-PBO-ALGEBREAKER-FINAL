using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace FP_Algebreaker
{
    //Kelas untuk bullet yang dipakai player
    public class Bullet
    {
        private const int _bulletSpeed = 10;
        private int _currentRowBullet;
        private Image _bulletImage = Image.FromFile(@"Assets\bullet.png");
        private PictureBox _bulletPicBox;
        private Timer _bulletTimer;
        public static int _maxAmmo = 10; //Dipakai untuk menentukan maxAmmo player
        public static int _currentAmmo = _maxAmmo; //Dibuat static, karena variabel ini bukan untuk single bullet, melainkan seluruh objek bullet yang pernah tercipta
        private List<Alien> _aliens = new List<Alien>();

        //Konstruktor
        public Bullet(Point gunPosition, int currentRow)
        {
            _bulletPicBox = new PictureBox
            {
                Size = new Size(32, 32),
                Location = gunPosition,
                Image = _bulletImage,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };

            _bulletTimer = new Timer
            {
                Interval = 50
            };
            _bulletTimer.Tick += OnBulletMove;
            _bulletTimer.Start();

            _currentRowBullet = currentRow;

            Debug.WriteLine($"Current Ammo: {_currentAmmo}");
        }

        //Event handler method
        private void OnBulletMove(object sender, EventArgs e)
        {
            CekArahBulletDanTembak(_currentRowBullet);

            foreach (var alien in _aliens)  // Pastikan 'aliens' adalah daftar alien yang aktif di dalam game
            {
                CheckCollisionWithAlien(alien);  // Cek tabrakan dengan setiap alien
            }
        }

        public void CekArahBulletDanTembak(int currentPlayerRow)
        {
            switch (currentPlayerRow)
            {
                //Yang ada dalam case nya adalah animasi bullet bergerak

                //Kalau current row player terakhir
                //0 -> Menghadap ke atas
                case 0:
                    _bulletPicBox.Top += _bulletSpeed;
                    break;
                //1 -> Menghadap ke Bawah
                case 1:
                    _bulletPicBox.Top -= _bulletSpeed;
                    break;
                //2 -> Menghadap ke kiri
                case 2:
                    _bulletPicBox.Left -= _bulletSpeed;
                    break;
                //3 -> Menghadap ke kanan
                case 3:
                    _bulletPicBox.Left += _bulletSpeed;
                    break;
            }
        }

        public void CheckCollisionWithAlien(Alien alien)
        {
            if (_bulletPicBox.Bounds.IntersectsWith(alien.AlienPictureBox.Bounds))
            {
                alien.TakeDamage(10);  // Berikan damage ke alien
                alien.UpdateHealthBar();
                Debug.WriteLine("Alien terkena tembakan! Health Alien: " + alien.CurrentHealth);
                _bulletPicBox.Hide();  // Sembunyikan peluru setelah mengenai alien
                _bulletTimer.Stop();

                // Hapus bullet dari form setelah tabrakan
                if (this.GetPictureBox().Parent != null)
                {
                    this.GetPictureBox().Parent.Controls.Remove(this.GetPictureBox());
                }
            }
        }

        public PictureBox GetPictureBox() => _bulletPicBox;
    }
}
