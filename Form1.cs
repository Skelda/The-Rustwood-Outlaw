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
    public partial class Form1: Form
    {
        Timer gameTimer = new Timer();
        Stopwatch frameTimer = new Stopwatch();
        double currentFPS = 0;
        int frameCount = 0;
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
                currentFPS += 1.0 / deltaTime;
                frameCount++;
            }

            if (frameCount >= 10)
            {
                this.LFrameRate.Text = $"{currentFPS/10}";
                currentFPS = 0;
                frameCount = 0;
            }

            

            this.Refresh();
        }
    }
}
