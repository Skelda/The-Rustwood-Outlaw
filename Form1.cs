using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace The_Rustwood_Outlaw
{
    public partial class Board : Form
    {

        Timer gameTimer = new Timer();
        
        HashSet<Keys> pressedKeys = new HashSet<Keys>();

        public List<Barricade> barricades = new List<Barricade>();
        public List<Entity> entities = new List<Entity>();
        public Entity player;


        public int mapPixelSize;
        public int offsetX;
        public int offsetY;

        public Board()
        {
            InitializeComponent();

            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;

            int mapPixelSize = GameSettings.CellSize * GameSettings.MapSize;

            offsetX = (ClientSize.Width - mapPixelSize) / 2;
            offsetY = (ClientSize.Height - mapPixelSize) / 2;

            gameTimer.Interval = 1000 / GameSettings.RefreshRate;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();



            LoadMap(this);
        }

        private void GameLoop(object sender, EventArgs e)
        {
            float deltaTime = 1.0f / GameSettings.RefreshRate;

            foreach (var entity in entities.ToList())
            { 
                entity.Update(deltaTime);
                if (entity.IsDestroyed) entity.Destroy();
            }
            pictureBox1.SendToBack();

        }


        public void LoadMap(Board board)
        {
            int MapSize = GameSettings.MapSize;
            string[] lines = Properties.Resources.levels.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);


            // Find the level by name
            int startIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Name:"))
                {
                    string map_name = lines[i].Split(':')[1]; // get the level name
                    startIndex = i + 1; // The map starts after this line
                    break;
                }
            }

            if (startIndex == -1)
            {
                MessageBox.Show("Level not found!");
                return;
            }

            int cellSize = GameSettings.CellSize;

            // Center the map within the form

            for (int y = 0; y < MapSize; y++)
            {
                string line = lines[startIndex + y];
                for (int x = 0; x < Math.Min(line.Length, MapSize); x++)
                {
                    char tile = line[x];
                    Point position = new Point(offsetX + x * cellSize, offsetY + y * cellSize);
                    Size size = new Size(cellSize, cellSize);

                    if (tile == 'x')
                    {
                        var barricade = new Barricade(board, new Point(x, y));
                        barricades.Add(barricade);
                    }
                    else if (tile == 'p')
                    {
                        PictureBox sprite = new PictureBox
                        {
                            Size = size,
                            Location = position,
                            BackColor = Color.Transparent
                        };
                        sprite.Image = new Bitmap(Properties.Resources.front_player_1, GameSettings.SpriteSize);
                        board.Controls.Add(sprite);

                        player = new Player(GameSettings.PlayerSpeed, GameSettings.PlayerHealth,
                                                   GameSettings.PlayerDamage, sprite, position, pressedKeys, this, barricades, entities);
                        entities.Add(player);
                    }

                    else if (tile == 'e')
                    {
                        PictureBox sprite = new PictureBox
                        {
                            Size = size,
                            Location = position,
                            BackColor = Color.Transparent
                        };
                        sprite.Image = new Bitmap(Properties.Resources.red_slime_1, GameSettings.SpriteSize);
                        board.Controls.Add(sprite);

                        Enemy enemy = new Enemy(GameSettings.EnemySpeed, GameSettings.EnemyHealth,
                                                   GameSettings.EnemyDamage, sprite, position, this, barricades, entities);
                        entities.Add(enemy);
                    }

                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
        }
    }
}
