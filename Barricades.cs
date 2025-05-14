using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Rustwood_Outlaw
{
    class Barricade
    {
        public PictureBox sprite;
        public Point position;
        public Size size;

        public Barricade(Form form, Point position, Size size)
        {
            this.position = position;
            this.size = size;

            sprite = new PictureBox();
            sprite.BackColor = Color.SaddleBrown; // placeholder color
            sprite.Size = size;
            sprite.Location = position;

            form.Controls.Add(sprite);
        }

        public Rectangle Bounds => new Rectangle(position, size);
    }
}
