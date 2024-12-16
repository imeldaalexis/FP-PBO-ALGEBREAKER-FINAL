using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Algebreaker
{
    // Kelas dasar Alien yang mengimplementasikan IHealth
    public abstract class Alien : IHealth
    {
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public PictureBox AlienPictureBox { get; set; }
        public ProgressBar HealthBar { get; private set; }

        // Konstruktor dasar
        public Alien(Point startPosition)
        {
            AlienPictureBox = new PictureBox
            {
                Size = new Size(32, 48),
                Location = startPosition,
                BackColor = Color.Purple // Warna ungu sebagai placeholder
            };

            MaxHealth = 100; // Nilai default
            CurrentHealth = MaxHealth;

            HealthBar = new ProgressBar
            {
                Maximum = MaxHealth,
                Value = CurrentHealth,
                Size = new Size(50, 12), // Ukuran health bar
                BackColor = Color.Gray,
                ForeColor = Color.Green // Warna health bar alien
            };

            UpdateHealthBarPosition();

        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();  // Call the Die method if health reaches 0
            }
            UpdateHealthBar();
        }

        private void Die()
        {
            Debug.WriteLine("Alien has died");
            AlienPictureBox.Hide();
            HealthBar.Hide();
        }

        public void Heal(int amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
            UpdateHealthBar();
        }

        public bool IsAlive()
        {
            return CurrentHealth > 0;
        }

        public void UpdateHealthBar()
        {
            HealthBar.Value = Math.Max(0, Math.Min(CurrentHealth, MaxHealth));
        }

        public void UpdateHealthBarPosition()
        {
            HealthBar.Location = new Point(
                AlienPictureBox.Left + (AlienPictureBox.Width / 2) - (HealthBar.Width / 2),
                AlienPictureBox.Top - 10 // Posisi di atas alien
            );
        }

        // Method umum untuk Alien untuk mengejar Player
        public abstract void ChasePlayer(Point playerPosition);
    }

    // RookieAlien - Mengimplementasikan metode fisik dan dapat menyebabkan damage
    public class RookieAlien : Alien
    {
        public RookieAlien(Point startPosition) : base(startPosition)
        {
            MaxHealth = 50;
            CurrentHealth = MaxHealth;
        }

        // Implementasi dari method ChasePlayer
        public override void ChasePlayer(Point playerPosition)
        {
            // Logika untuk mengejar player
            if (AlienPictureBox.Left < playerPosition.X)
                AlienPictureBox.Left += 2;
            else if (AlienPictureBox.Left > playerPosition.X)
                AlienPictureBox.Left -= 2;

            if (AlienPictureBox.Top < playerPosition.Y)
                AlienPictureBox.Top += 2;
            else if (AlienPictureBox.Top > playerPosition.Y)
                AlienPictureBox.Top -= 2;
        }

        // Method khusus untuk RookieAlien yang menyerang fisik jika bersenggolan
        public void PhysicalAttack(Player player)
        {
            if (AlienPictureBox.Bounds.IntersectsWith(player.GetPictureBoxes()[0].Bounds))
            {
                player.TakeDamage(10); // Memberikan damage
            }
        }
    }

    // KacynzkiAlien - Bisa meledak saat bersenggolan dengan player
    public class KacynzkiAlien : Alien
    {
        public KacynzkiAlien(Point startPosition) : base(startPosition)
        {
            MaxHealth = 70;
            CurrentHealth = MaxHealth;
        }

        public override void ChasePlayer(Point playerPosition)
        {
            // Logika mengejar player
            if (AlienPictureBox.Left < playerPosition.X)
                AlienPictureBox.Left += 3;
            else if (AlienPictureBox.Left > playerPosition.X)
                AlienPictureBox.Left -= 3;

            if (AlienPictureBox.Top < playerPosition.Y)
                AlienPictureBox.Top += 3;
            else if (AlienPictureBox.Top > playerPosition.Y)
                AlienPictureBox.Top -= 3;
        }

        // Method khusus KacynzkiAlien untuk ledakan
        public void BombAttack(Player player)
        {
            if (player.IsAlive() && AlienPictureBox.Bounds.IntersectsWith(player.GetPictureBoxes()[0].Bounds))
            {
                TakeDamage(50); // Mengurangi health alien lebih banyak karena ledakan
                player.TakeDamage(30); // Memberikan damage lebih besar ke player
            }
        }
    }

    // EldritchHorrorAlien - Bisa teleportasi dan menyerang fisik
    public class EldritchHorrorAlien : Alien
    {
        private System.Windows.Forms.Timer _teleportTimer;
        private Player _player;
        public EldritchHorrorAlien(Point startPosition, Player player) : base(startPosition)
        {
            _player = player; // Menyimpan referensi objek player
            MaxHealth = 100;
            CurrentHealth = MaxHealth;

            _teleportTimer = new System.Windows.Forms.Timer
            {
                Interval = 7000 // 7 detik
            };
            _teleportTimer.Tick += (sender, e) => Teleport();
            _teleportTimer.Start();
        }

        // Mengimplementasikan ChasePlayer dengan pengejaran normal
        public override void ChasePlayer(Point playerPosition)
        {
            // Mengejar player seperti alien lain
            if (AlienPictureBox.Left < playerPosition.X)
                AlienPictureBox.Left += 2;
            else if (AlienPictureBox.Left > playerPosition.X)
                AlienPictureBox.Left -= 2;

            if (AlienPictureBox.Top < playerPosition.Y)
                AlienPictureBox.Top += 2;
            else if (AlienPictureBox.Top > playerPosition.Y)
                AlienPictureBox.Top -= 2;
        }

        private void Teleport()
        {
            Random rand = new Random();
            Point playerPosition = _player.GetPictureBoxes()[0].Location;

            int xOffset = rand.Next(100, 200);  // Adjust distance to ensure teleport is far enough from player
            int yOffset = rand.Next(100, 200);

            // Ensure teleport position is not too close to the player
            int newX = playerPosition.X + xOffset * (rand.Next(0, 2) == 0 ? -1 : 1);
            int newY = playerPosition.Y + yOffset * (rand.Next(0, 2) == 0 ? -1 : 1);

            AlienPictureBox.Location = new Point(newX, newY);
        }

        // Method fisik untuk EldritchHorrorAlien
        public void PhysicalAttack(Player player)
        {
            if (AlienPictureBox.Bounds.IntersectsWith(player.GetPictureBoxes()[0].Bounds))
            {
                player.TakeDamage(20); // Memberikan damage
            }
        }
    }
}
