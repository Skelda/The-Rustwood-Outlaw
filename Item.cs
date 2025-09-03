using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Rustwood_Outlaw
{
    public enum ItemType
    {
        Health,
        FireRate,
        MultiShot,
        SeedBoost,
        DamageBoost
    }

    public class Item
    {
        public ItemType Type;
        public Point Position;
        public Board Board;
        public PictureBox sprite;
        private float onGroundTimer = GameSettings.itemOnGroundTime;

        public Item(ItemType type, Point position, Board board)
        {
            Type = type;
            Position = position;
            Board = board;

            sprite = new PictureBox
            {
                Size = GameSettings.SpriteSize,
                Location = Position,
                BackColor = Color.Transparent,
                Image = new Bitmap(GetSprite(), GameSettings.SpriteSize)
            };
        }

        public Image GetSprite()
        {
            switch (Type)
            {
                case ItemType.Health:
                    return (Properties.Resources.healthBoost);
                case ItemType.FireRate:
                    return Properties.Resources.fireRateBoost;
                case ItemType.MultiShot:
                    return Properties.Resources.multishot;
                case ItemType.SeedBoost:
                    return Properties.Resources.speedBoost;
                case ItemType.DamageBoost:
                    return Properties.Resources.damageBoost;
                default:
                    return null;
            }
        }

        public void Draw()
        {
            if (!Board.Controls.Contains(sprite))
            {
                Board.Controls.Add(sprite);
                sprite.BringToFront();
            }
            sprite.Location = Position;
            sprite.Visible = true;
        }


        public void Destroy()
        {
            if (Board.Controls.Contains(sprite))
                Board.Controls.Remove(sprite);
            sprite.Dispose();
            Board.items.Remove(this);
        }

        public void Update(float deltaTime)
        {
            onGroundTimer -= deltaTime;
            Draw();
            if (Bounds.IntersectsWith(Board.player.Bounds))
            {
                GetPickedUp();
                Destroy();
            }
            if (onGroundTimer <= 0) Destroy();
        }

        private void GetPickedUp()
        {
            switch (Type)
            {
                case ItemType.Health:
                    if (Board.player.health < GameSettings.PlayerHealth) Board.player.health++;
                    break;
                case ItemType.FireRate:
                    Board.player.shootDelay = GameSettings.PlayerShootingSpeed / 2f;
                    Board.player.fireRateTimer = GameSettings.fireRateTime;
                    break;
                case ItemType.MultiShot:
                    Board.player.multishot = true;
                    Board.player.multishotTimer = GameSettings.multishotTime;
                    break;
                case ItemType.SeedBoost:
                    Board.player.speed = GameSettings.PlayerSpeed * 2;
                    Board.player.speedBoostTimer = GameSettings.speedBoostTime;
                    break;
                case ItemType.DamageBoost:
                    Board.player.damage = GameSettings.PlayerDamage * 2;
                    Board.player.damageRateTimer = GameSettings.damageRateTime;
                    break;
            }
        }

        public Rectangle Bounds => new Rectangle(Position, GameSettings.SpriteSize);
    }
}
