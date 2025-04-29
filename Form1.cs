using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Rustwood_Outlaw
{
    public partial class Form1 : Form
    {
        Timer gameTimer = new Timer();
        Stopwatch frameTimer = new Stopwatch();
        double currentFPS = 0;
        int frameCount = 0;

        List<double> frameTimes = new List<double>(); // For 1% low tracking

        public Form1()
        {
            InitializeComponent();

            gameTimer.Interval = 1000 / GameSettings.RefreshRate;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
            frameTimer.Start();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            double deltaTime = frameTimer.Elapsed.TotalSeconds;
            frameTimer.Restart();

            if (deltaTime > 0)
            {
                double fps = 1.0 / deltaTime;
                currentFPS += fps;
                frameTimes.Add(deltaTime);
                frameCount++;
            }

            if (frameCount >= 10)
            {
                double averageFPS = currentFPS / 10;
                double onePercentLow = CalculateOnePercentLow(frameTimes);

                this.LFrameRate.Text = $"Avg: {averageFPS:F1} FPS\n1% Low: {onePercentLow:F1} FPS";

                currentFPS = 0;
                frameCount = 0;
            }

            this.Refresh();
        }

        private double CalculateOnePercentLow(List<double> times)
        {
            if (times.Count < 100)
                return 0; // Not enough data for 1%

            var sorted = times.OrderByDescending(t => t).ToList();
            int count = Math.Max(1, (int)(times.Count * 0.01));
            double avgSlowest = sorted.Take(count).Average();

            while (times.Count > 500)
                times.RemoveAt(0);

            return 1.0 / avgSlowest;
        }
    }

}
