using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace The_Rustwood_Outlaw
{
    class Entity
    {
        public enum directions { STAY, UP, DOWN, LEFT, RIGHT, UPLEFT, UPRIGHT, DOWNLEFT, DOWNRIGHT };
        public int speed; // pixels per second
        public int health;
        public int damage;
        public PictureBox sprite;
        public Point position;

        public Entity(int speed, int health, int damage, PictureBox sprite, Point position)
        {
            this.speed = speed;
            this.health = health;
            this.damage = damage;
            this.sprite = sprite;
            this.position = position;
        }

        public virtual void Update(float deltaTime, List<Barricade> obstacles) { }

        public virtual void Draw(Graphics g) { }


        public virtual void Move(float deltaTime, directions direction, List<Barricade> obstacles)
        {
            float moveAmount = speed * deltaTime;
            float diagonalFactor = (float)GameSettings.diagonalMove;

            float dx = 0, dy = 0;

            switch (direction)
            {
                case directions.UP: dy = -1; break;
                case directions.DOWN: dy = 1; break;
                case directions.LEFT: dx = -1; break;
                case directions.RIGHT: dx = 1; break;
                case directions.UPLEFT: dx = -1; dy = -1; break;
                case directions.UPRIGHT: dx = 1; dy = -1; break;
                case directions.DOWNLEFT: dx = -1; dy = 1; break;
                case directions.DOWNRIGHT: dx = 1; dy = 1; break;
                case directions.STAY: return;
            }

            bool isDiagonal = dx != 0 && dy != 0;
            float factor = isDiagonal ? diagonalFactor : 1.0f;
            dx *= moveAmount * factor;
            dy *= moveAmount * factor;

            // Stepwise check: try X axis first, then Y
            Point nextPosX = new Point(position.X + (int)dx, position.Y);
            Rectangle boundsX = new Rectangle(nextPosX, sprite.Size);

            bool xBlocked = obstacles.Any(b => b.Bounds.IntersectsWith(boundsX));
            if (!xBlocked)
            {
                position.X += (int)dx;
            }
            else
            {
                // Slide up to the edge of the barricade
                position.X = FindMaxXBeforeCollision(position, (int)dx, sprite.Size, obstacles);
            }

            Point nextPosY = new Point(position.X, position.Y + (int)dy);
            Rectangle boundsY = new Rectangle(nextPosY, sprite.Size);

            bool yBlocked = obstacles.Any(b => b.Bounds.IntersectsWith(boundsY));
            if (!yBlocked)
            {
                position.Y += (int)dy;
            }
            else
            {
                position.Y = FindMaxYBeforeCollision(position, (int)dy, sprite.Size, obstacles);
            }

            sprite.Location = position;
        }

        private int FindMaxXBeforeCollision(Point current, int dx, Size size, List<Barricade> obstacles)
        {
            int step = Math.Sign(dx);
            for (int i = 0; i != dx; i += step)
            {
                var testPos = new Point(current.X + i, current.Y);
                var rect = new Rectangle(testPos, size);
                if (obstacles.Any(b => b.Bounds.IntersectsWith(rect)))
                    return current.X + i - step;
            }
            return current.X + dx;
        }

        private int FindMaxYBeforeCollision(Point current, int dy, Size size, List<Barricade> obstacles)
        {
            int step = Math.Sign(dy);
            for (int i = 0; i != dy; i += step)
            {
                var testPos = new Point(current.X, current.Y + i);
                var rect = new Rectangle(testPos, size);
                if (obstacles.Any(b => b.Bounds.IntersectsWith(rect)))
                    return current.Y + i - step;
            }
            return current.Y + dy;
        }


    }

    class Player : Entity
    {
        private HashSet<Keys> pressedKeys;

        public Player(int speed, int health, int damage, PictureBox sprite, Point position, HashSet<Keys> keys)
            : base(speed, health, damage, sprite, position)
        {
            pressedKeys = keys;
        }

        public override void Update(float deltaTime, List<Barricade> obstacles)
        {
            directions dir = directions.STAY;

            bool up = pressedKeys.Contains(Keys.W);
            bool down = pressedKeys.Contains(Keys.S);
            bool left = pressedKeys.Contains(Keys.A);
            bool right = pressedKeys.Contains(Keys.D);

            if (up && left) dir = directions.UPLEFT;
            else if (up && right) dir = directions.UPRIGHT;
            else if (down && left) dir = directions.DOWNLEFT;
            else if (down && right) dir = directions.DOWNRIGHT;
            else if (up) dir = directions.UP;
            else if (down) dir = directions.DOWN;
            else if (left) dir = directions.LEFT;
            else if (right) dir = directions.RIGHT;


            Move(deltaTime, dir, obstacles);
        }
    }

}
