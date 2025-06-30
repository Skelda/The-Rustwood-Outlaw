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

        
        HashSet<Keys> pressedKeys = new HashSet<Keys>();

        private static List<Level> levels = new List<Level>();

        Timer gameTimer = new Timer();
        public List<Barricade> barricades = new List<Barricade>();
        public List<SpawnArea> spawnAreas = new List<SpawnArea>();
        public List<Entity> entities = new List<Entity>();
        public List<Item> items = new List<Item>();
        public int level;
        public int score;
        public Player player;

        private int lastDrawnHearts = -1;


        private int totalTime; 
        private int timeLeft;

        private bool paused = false;

        public int mapPixelSize;
        public int offsetX;
        public int offsetY;

        public Board()
        {
            InitializeComponent();
            this.difficulty.DataSource = Enum.GetValues(typeof(Difficulty));
            this.difficulty.SelectedIndex = (int)GameSettings.difficulty;
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

            gameTimer.Tick += levelTimer;

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
                if (entity.IsDestroyed) 
                { 
                    entity.Destroy();
                    continue;
                }
                entity.Update(deltaTime);
                
            }

            foreach (var spawnArea in spawnAreas.ToList())
            {
                spawnArea.TryToSpawnEnemy(deltaTime);
            }

            foreach (var item in items.ToList())
            {
                item.Update(deltaTime);
            }

            DrawHearts();
            lScore.Text = $"Score: {score}";
            Background.SendToBack();

        }

        private void levelTimer(object sender, EventArgs e)
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
                this.lLevel.Text = $"Level: {level}";
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
                multiplier = levelIndex - levels.Count + 1;
                levelIndex = levels.Count-1;
            }
            Level level = levels[levelIndex];

            board.totalTime = level.Time * multiplier;
            board.timeLeft = level.Time * multiplier;

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

                                Bitmap[] framesUp = new Bitmap[]
                                {
                                    new Bitmap(Properties.Resources.back_player_1, GameSettings.SpriteSize),
                                    new Bitmap(Properties.Resources.back_player_2, GameSettings.SpriteSize)
                                };

                                Bitmap[] framesDown = new Bitmap[]
                                {
                                    new Bitmap(Properties.Resources.front_player_1, GameSettings.SpriteSize),
                                    new Bitmap(Properties.Resources.front_player_2, GameSettings.SpriteSize)
                                };

                                player = new Player(GameSettings.PlayerSpeed, GameSettings.PlayerHealth,
                                                           GameSettings.PlayerDamage, sprite, position, pressedKeys, this, framesUp, framesDown);
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



                                Bitmap[] framesRight;
                                Bitmap[] framesLeft;
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

                                sprite.Image = framesRight[0];
                                board.Controls.Add(sprite);

                                Enemy enemy = new Enemy(GameSettings.EnemySpeed, GameSettings.EnemyHealth,
                                                           GameSettings.EnemyDamage, sprite, position, this, framesLeft, framesRight);
                                entities.Add(enemy);
                                break;
                            }
                        case 's':
                            {
                                SpawnArea area = new SpawnArea(board, new Point(x, y), level.SpawnRate / multiplier);
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
                    Pause.BringToFront();
                    Pause.Visible = true;
                    gameTimer.Stop();
                }
                else 
                {
                    bContinue_Click(); 
                    paused = !paused;

                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
        }

        private void bStartGame_Click(object sender, EventArgs e)
        {
            paused = false;
            Startgame();
            MainMenu.Visible = false;
            pYouLost.Visible = false;
        }

        private void bContinue_Click(object sender=null, EventArgs e=null)
        {
            switch (difficulty.SelectedIndex)
            {
                case 0:
                    {
                        GameSettings.enemySpawnChance = 1 / 6f;
                        break;
                    }
                case 1:
                    {
                        GameSettings.enemySpawnChance = 1 / 4f;
                        break;
                    }
                case 2:
                    {
                        GameSettings.enemySpawnChance = 1 / 2f;
                        break;
                    }
                case 3:
                    {
                        GameSettings.enemySpawnChance = 1f;
                        break;
                    }
            }
            paused = !paused;
            Pause.Visible = false;
            gameTimer.Start();
        }

        public void DrawHearts()
        {
            if (player == null) return;
            if (player.health == lastDrawnHearts) return;

            panelHearts.Controls.Clear();
            int heartWidth = 16;
            int spacing = 1;
            int totalWidth = player.health * heartWidth + Math.Max(0, player.health - 1) * spacing;
            panelHearts.Width = totalWidth;

            for (int i = 0; i < player.health; i++)
            {
                PictureBox heart = new PictureBox
                {
                    Image = Properties.Resources.heart,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Size = new Size(heartWidth, heartWidth),
                    Location = new Point(i * (heartWidth + spacing), 0),
                    BackColor = Color.Transparent
                };
                panelHearts.Controls.Add(heart);
            }
            lastDrawnHearts = player.health;
        }

        public void YouLost()
        {
            gameTimer.Stop();
            pYouLost.Visible = true;
            lLostScreenLevel.Text = $"Level: {level}";
            lLostScreenScore.Text = $"Score: {score}";
            gameTimer = new Timer();
            foreach (var bar in barricades.ToList()) bar.Destroy();
            foreach (var ent in entities.ToList()) ent.IsDestroyed = true;
            foreach (var spa in spawnAreas.ToList()) spa.Destroy();
            foreach (var item in items.ToList()) item.Destroy();

            level = 0;
            score = 0;
            lastDrawnHearts = -1;
    }
    }
}
