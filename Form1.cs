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
    public partial class Form1 : Form
    {

        Timer gameTimer = new Timer();
        
        HashSet<Keys> pressedKeys = new HashSet<Keys>();

        List<Barricade> barricades = new List<Barricade>();
        List<Entity> entities = new List<Entity>();


        public Form1()
        {
            InitializeComponent();

            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;

            gameTimer.Interval = 1000 / GameSettings.RefreshRate;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            LoadMap(this);
        }

        private void GameLoop(object sender, EventArgs e)
        {
            float deltaTime = 1.0f / GameSettings.RefreshRate;

            foreach (var entity in entities)
                entity.Update(deltaTime, barricades);
        }


        public void LoadMap(Form form)
        {
            int MapSize = GameSettings.MapSize;
            string[] lines = File.ReadAllLines("levels.txt");

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

            // Calculate total map pixel size
            int mapPixelSize = cellSize * MapSize;

            // Center the map within the form
            int offsetX = (form.ClientSize.Width - mapPixelSize) / 2;
            int offsetY = (form.ClientSize.Height - mapPixelSize) / 2;

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
                        var barricade = new Barricade(form, position, size);
                        barricades.Add(barricade);
                    }
                    else if (tile == 'p')
                    {
                        PictureBox sprite = new PictureBox
                        {
                            BackColor = Color.Blue, // You can set an image here too
                            Size = size,
                            Location = position
                        };
                        form.Controls.Add(sprite);

                        Entity player = new Player(GameSettings.PlayerSpeed, GameSettings.PlayerHealth,
                                                   GameSettings.PlayerDamage, sprite, position, pressedKeys);
                        entities.Add(player);
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
