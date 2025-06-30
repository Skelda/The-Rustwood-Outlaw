using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace The_Rustwood_Outlaw
{
    public class Barricade
    {
        protected PictureBox sprite;
        protected Point position;
        protected Point gridposition;
        protected Size size;
        protected Board board;

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

        public virtual void Destroy()
        {
            if (board.Controls.Contains(sprite))
                board.Controls.Remove(sprite);
            sprite.Dispose();
            board.barricades.Remove(this);
        }

        public Rectangle Bounds => new Rectangle(position, size);
    }

    public class SpawnArea : Barricade
    {
        private float spawnRate;
        private float timeSinceLastSpawn;
        private static Random random = new Random();

        public SpawnArea(Board board, Point gridposition, float spawnRate) : base(board, gridposition)
        {
            this.spawnRate = spawnRate;
            this.sprite.Image = new Bitmap(Properties.Resources.spawn_area, GameSettings.SpriteSize);
        }

        public override void Destroy()
        {
            if (board.Controls.Contains(sprite))
                board.Controls.Remove(sprite);
            sprite.Dispose();
            board.spawnAreas.Remove(this);
        }

        public void TryToSpawnEnemy(float deltatime)
        {
            timeSinceLastSpawn += deltatime*1000;
            if (timeSinceLastSpawn > spawnRate)
            {
                timeSinceLastSpawn = 0f;
                bool succesfulSpawn = (1 == random.Next(1, (int)(1/GameSettings.enemySpawnChance+1)));
                if (succesfulSpawn)
                {
                    // Náhodně vyber barvu slima
                    bool green = random.Next(2) == 0;

                    Bitmap[] framesRight;
                    Bitmap[] framesLeft;
                    if (green)
                    {
                        framesRight = new Bitmap[]
                        {
                        new Bitmap(Properties.Resources.green_slime_1, GameSettings.SpriteSize),
                        new Bitmap(Properties.Resources.green_slime_2, GameSettings.SpriteSize),
                        new Bitmap(Properties.Resources.green_slime_3, GameSettings.SpriteSize)};
                        framesLeft = new Bitmap[]
                        {
                        new Bitmap(Properties.Resources.green_slime_4, GameSettings.SpriteSize),
                        new Bitmap(Properties.Resources.green_slime_5, GameSettings.SpriteSize),
                        new Bitmap(Properties.Resources.green_slime_6, GameSettings.SpriteSize)};
                    }
                    else
                    {
                        framesRight = new Bitmap[]
                        {
                        new Bitmap(Properties.Resources.red_slime_1, GameSettings.SpriteSize),
                        new Bitmap(Properties.Resources.red_slime_2, GameSettings.SpriteSize),
                        new Bitmap(Properties.Resources.red_slime_3, GameSettings.SpriteSize)};
                        framesLeft = new Bitmap[]
                        {
                        new Bitmap(Properties.Resources.red_slime_4, GameSettings.SpriteSize),
                        new Bitmap(Properties.Resources.red_slime_5, GameSettings.SpriteSize),
                        new Bitmap(Properties.Resources.red_slime_6, GameSettings.SpriteSize)};
                    }

                    PictureBox sprite = new PictureBox
                    {
                        Size = size,
                        Location = position,
                        BackColor = Color.Transparent,
                        Image = framesRight[0]
                    };
                    board.Controls.Add(sprite);

                    var slime = new Enemy(
                        GameSettings.EnemySpeed,
                        GameSettings.EnemyHealth,
                        GameSettings.EnemyDamage,
                        sprite,
                        position,
                        board,
                        framesLeft, 
                        framesRight
                    );
                    board.entities.Add(slime);
                }
            }
        }
    }
}
