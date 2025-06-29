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
        public List<SpawnArea> spawnAreas = new List<SpawnArea>();
        public List<Entity> entities = new List<Entity>();
        private static List<Level> levels = new List<Level>();
        public Entity player;

        private int totalTime; 
        private int timeLeft;

        private bool paused = false;

        public int mapPixelSize;
        public int offsetX;
        public int offsetY;
        public int level;

        public Board()
        {
            InitializeComponent();
            MainMenu.Visible = true;
        }


        private void Startgame()
        {
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            int mapPixelSize = GameSettings.CellSize * GameSettings.MapSize;

            offsetX = (ClientSize.Width - mapPixelSize) / 2;
            offsetY = (ClientSize.Height - mapPixelSize) / 2;

            gameTimer.Interval = 1000 / GameSettings.RefreshRate;
            gameTimer.Tick += GameLoop;

            gameTimer.Tick += Timer_Tick;

            // Start countdown
            timeLeft = totalTime;
            gameTimer.Start();

            LoadLevels();
            level = 0;

            LoadMap(this, level);
        }

        private void GameLoop(object sender, EventArgs e)
        {
            float deltaTime = 1.0f / GameSettings.RefreshRate;

            foreach (var entity in entities.ToList())
            {
                entity.Update(deltaTime);
                if (entity.IsDestroyed) entity.Destroy();
            }
            Background.SendToBack();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft -= 1000 / GameSettings.RefreshRate;
            if (timeLeft >= 0)
            {
                progressBar1.Value = timeLeft;
            }
            else
            {
                level++;
                foreach (var entity in this.entities.ToList()) entity.Destroy();
                foreach (var barricade in this.barricades.ToList()) barricade.Destroy();
                foreach (var spawnarea in this.spawnAreas.ToList()) spawnarea.Destroy();
                LoadMap(this, level);
            }
        }


        public static void LoadLevels()
        {
            var lines = Properties.Resources.levels.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            int i = 0;
            while (i < lines.Length)
            {
                if (lines[i].StartsWith("Name:"))
                {
                    Level level = new Level();
                    // Parse the header
                    var header = lines[i].Split(' ');
                    foreach (var part in header)
                    {
                        if (part.StartsWith("Name:")) level.Name = part.Substring(5);
                        else if (part.StartsWith("Time:")) level.Time = int.Parse(part.Substring(5));
                        else if (part.StartsWith("SpawnRate:")) level.SpawnRate = int.Parse(part.Substring(10));
                        else if (part.StartsWith("Boss:")) level.BossCount = int.Parse(part.Substring(5));
                    }
                    i++;
                    // Read the map
                    level.MapLines = new List<string>();
                    for (int j = 0; j < 21 && i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]); j++, i++)
                    {
                        level.MapLines.Add(lines[i]);
                    }
                    // Skip empty lines
                    while (i < lines.Length && string.IsNullOrWhiteSpace(lines[i])) i++;
                    levels.Add(level);
                }
                else
                {
                    i++;
                }
            }
        }


        public void LoadMap(Board board, int levelIndex)
        {
            int MapSize = GameSettings.MapSize;
            int multiplier = 1;
            if (levelIndex >= levels.Count)
            {
                multiplier = levels.Count - 1 - levelIndex;
                levelIndex = levels.Count-1;
            }
            Level level = levels[levelIndex];

            board.totalTime = level.Time;
            board.timeLeft = level.Time;

            progressBar1.Maximum = totalTime;
            progressBar1.Value = totalTime;

            int cellSize = GameSettings.CellSize;

            for (int y = 0; y < MapSize; y++)
            {
                string line = level.MapLines[y];
                for (int x = 0; x < Math.Min(line.Length, MapSize); x++)
                {
                    char tile = line[x];
                    Point position = new Point(offsetX + x * cellSize, offsetY + y * cellSize);
                    Size size = new Size(cellSize, cellSize);

                    switch (tile)
                    {
                        case 'x':
                            {
                                var barricade = new Barricade(board, new Point(x, y));
                                barricades.Add(barricade);
                                break;
                            }
                        case 'p':
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
                                                           GameSettings.PlayerDamage, sprite, position, pressedKeys, this);
                                entities.Add(player);
                                break;
                            }
                        case 'e':
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
                                                           GameSettings.EnemyDamage, sprite, position, this);
                                entities.Add(enemy);
                                break;
                            }
                        case 's':
                            {
                                PictureBox sprite = new PictureBox
                                {
                                    Size = size,
                                    Location = position,
                                    BackColor = Color.Transparent
                                };
                                sprite.Image = new Bitmap(Properties.Resources.spawn_area, GameSettings.SpriteSize);
                                board.Controls.Add(sprite);

                                SpawnArea area = new SpawnArea(board, new Point(x, y), level.SpawnRate);
                                spawnAreas.Add(area);
                                break;
                            }
                    }
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
            if (e.KeyCode == Keys.P) 
            {
                paused = !paused;
                if (paused)
                {
                    PauseText.Visible = true;
                    gameTimer.Stop();
                }
                else 
                { 
                    PauseText.Visible = false;
                    gameTimer.Start();
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Startgame();
            MainMenu.Visible = false;
        }
    }
}
