using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Rustwood_Outlaw
{
    public class Barricade
    {
        public PictureBox sprite;
        public Point position;
        public Point gridposition;
        public Size size;
        Board board;

        public Barricade(Board board, Point gridposition)
        {
            this.gridposition = gridposition;
            this.board = board;
            this.position = new Point(board.offsetX + gridposition.X * GameSettings.CellSize,
                                      board.offsetY + gridposition.Y * GameSettings.CellSize);
            this.size = new Size(GameSettings.CellSize, GameSettings.CellSize);

            sprite = new PictureBox();
            sprite.BackColor = Color.SaddleBrown; // placeholder color
            sprite.Size = size;
            sprite.Location = position;
            sprite.Image = new Bitmap(Properties.Resources.barricade, GameSettings.SpriteSize);

            board.Controls.Add(sprite);
        }

        public Rectangle Bounds => new Rectangle(position, size);
    }
}
