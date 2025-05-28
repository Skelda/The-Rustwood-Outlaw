using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Rustwood_Outlaw
{
    public static class GameSettings
    {
        public static int RefreshRate = 60;
        public static float diagonalMove = 0.707107f;
        public static int MapSize = 21;
        public static int CellSize = 32;
        public static Size SpriteSize = new Size(CellSize , CellSize);
        public static Size BulletSize = new Size(CellSize / 4, CellSize / 2);
        public static int PlayerSpeed = 750;
        public static int PlayerHealth = 5;
        public static int PlayerDamage = 1;
        public static float PlayerShootingSpeed = 0.10f;
        public static int EnemySpeed = 200;
        public static int EnemyDamage = 1;
        public static int EnemyHealth = 4;
        public static int BulletSpeed = 500;
        public static int BulletHealth = 1;
        public static bool drawPathfinding = false;
    }
}
