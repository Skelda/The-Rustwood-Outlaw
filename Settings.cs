using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Rustwood_Outlaw
{
    public static class GameSettings
    {
        public static int RefreshRate = 60;
        public static float diagonalMove = 0.707107f;
        public static int MapSize = 20;
        public static int CellSize = 20;
        public static int PlayerSpeed = 500;
        public static int PlayerHealth = 5;
        public static int PlayerDamage = 1;
        public static float PlayerShootingSpeed = 0.10f;
        public static int EnemySpeed = 150;
        public static int EnemyDamage = 1;
        public static int EnemyHealth = 10;
        public static int BulletSpeed = 200;
        public static int BulletHealth = 1;
        public static bool drawPathfinding = false;
    }
}
