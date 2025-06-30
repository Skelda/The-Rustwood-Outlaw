using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Rustwood_Outlaw
{
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        Impossible
    }
    public static class GameSettings
    {
        public static readonly Dictionary<Difficulty, float> DifficultySpawnChances = new Dictionary<Difficulty, float>
        {
            { Difficulty.Easy, 1f / 6f },
            { Difficulty.Normal, 1f / 4f },
            { Difficulty.Hard, 1f / 2f },
            { Difficulty.Impossible, 1f }
        };
        public static int RefreshRate = 60;
        public static float diagonalMove = 0.707107f;
        public static int MapSize = 21;
        public static int CellSize = 32;
        public static Size SpriteSize = new Size(CellSize , CellSize);
        public static Size BulletSize = new Size(CellSize / 4, CellSize / 2);
        public static int PlayerSpeed = 500;
        public static int PlayerHealth = 3;
        public static int PlayerDamage = 1;
        public static float PlayerShootingSpeed = 0.10f;
        public static int EnemySpeed = 200;
        public static int EnemyDamage = 1;
        public static int EnemyHealth = 2;
        public static int BulletSpeed = 750;
        public static int BulletHealth = 1;
        public static int itemDropChance = 10;
        public static Difficulty difficulty = Difficulty.Normal;
        public static float enemySpawnChance = DifficultySpawnChances[difficulty];
        public static float speedBoostTime = 5f;
        public static float fireRateTime = 5f;
        public static float damageRateTime = 5f;
        public static float multishotTime = 5f;
        public static float itemOnGroundTime = 5f;
    }
}
