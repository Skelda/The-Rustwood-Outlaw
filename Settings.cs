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
        public const int RefreshRate = 120;
        public const float diagonalMove = 0.707107f;
        public const int MapSize = 21;
        public const int CellSize = 32;
        public static Size SpriteSize = new Size(CellSize , CellSize);
        public static Size BulletSize = new Size(CellSize / 4, CellSize / 2);
        public const int PlayerSpeed = 250;
        public const int PlayerHealth = 3;
        public const int PlayerDamage = 1;
        public const float PlayerShootingSpeed = 0.25f;
        public const int EnemySpeed = 100;
        public const int EnemyDamage = 1;
        public const int EnemyHealth = 2;
        public const int BulletSpeed = 350;
        public const int BulletHealth = 1;
        public const int itemDropChance = 10;
        public static Difficulty difficulty = Difficulty.Normal;
        public static float enemySpawnChance = DifficultySpawnChances[difficulty];
        public const float speedBoostTime = 5f;
        public const float fireRateTime = 5f;
        public const float damageRateTime = 5f;
        public const float multishotTime = 5f;
        public const float itemOnGroundTime = 5f;
        public const float animationSpeed = 0.12f;
    }
}
