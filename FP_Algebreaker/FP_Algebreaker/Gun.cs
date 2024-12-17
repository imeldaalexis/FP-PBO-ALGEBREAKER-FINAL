using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace FP_Algebreaker
{
    public class Gun
    {
        //private Image _bulletImage = Image.FromFile(@"Assets\GunImage.png"); (Masih dalam WIP Aset Gunnya)
        private PictureBox _gunPicBox;
        private Bullet _bullet;

        public Gun(Point PlayerPosition)
        {
            _gunPicBox = new PictureBox
            {
                Size = new Size(32, 32),
                BackColor = Color.Red,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = PlayerPosition
            };
        }

        // method ketika dipress key Enter
        public void ShootManual(Form gameform, Point playerPosition, int currentPlayerRow, List<Alien> aliens, Player player)
        {
            //move bullet to current position of 
            _gunPicBox.Location = playerPosition;
            //_gunPicBox.BackColor = Color.Transparent;
            if (Bullet._currentAmmo > 0)
            {
                _bullet = new Bullet(_gunPicBox.Location, currentPlayerRow, aliens);
                gameform.Controls.Add(_bullet.GetPictureBox());
                Bullet._currentAmmo -= 1;
                Debug.WriteLine($"Bullet fired! Current Ammo: {Bullet._currentAmmo}");
                player.UpdateAmmoLabel();

                SoundPlayer shootSound = new SoundPlayer(@"Sound\PlayerShot.wav");  // Pastikan path sound file benar
                shootSound.Play();
            }
            else
            {
                Debug.WriteLine($"Out of Ammo!");
            }
        }


        public PictureBox GetPictureBox() => _gunPicBox;

    }
}
