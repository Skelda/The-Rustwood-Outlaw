using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using The_Rustwood_Outlaw.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static The_Rustwood_Outlaw.Entity;

namespace The_Rustwood_Outlaw
{
    public class Entity
    {
        public int speed; // pixels per second
        public int health;
        public int damage;
        public PictureBox sprite;
        public Point position;
        public Board board;
        public List<Barricade> obstacles;
        public List<Entity> entities;
        public bool IsDestroyed = false;

        public Entity(int speed, int health, int damage, PictureBox sprite, Point position, Board board)
        {
            this.speed = speed;
            this.health = health;
            this.damage = damage;
            this.sprite = sprite;
            this.position = position;
            this.board = board;
            this.obstacles = board.barricades;
            this.entities = board.entities;

        }

        public virtual void Update(float deltaTime) 
        {
            if (this.health <= 0) 
            { 
                IsDestroyed = true;
                return;
            }
            Move(deltaTime);
            Draw();
        }

        protected virtual bool CollidesAt(int x, int y)
        {
            Point testPos = new Point(x, y);
            Rectangle testRect = new Rectangle(testPos, sprite.Size);
            bool collision = obstacles.Any(b => b.Bounds.IntersectsWith(testRect)) ||
                                     entities.Any(e => (!object.ReferenceEquals(e, this) &&
                                                         e.Bounds.IntersectsWith(testRect)));
            return collision;
        }

        protected Point GetMaxPosition(int dx, int dy)
        {
            Point newPos = position;

            // Pohyb po ose X
            if (dx != 0)
            {
                int stepX = Math.Sign(dx);
                for (int i = 1; i <= Math.Abs(dx); i++)
                {
                    if (CollidesAt(newPos.X + stepX, newPos.Y))
                        break;
                    newPos.X += stepX;
                }
            }

            // Pohyb po ose Y
            if (dy != 0)
            {
                int stepY = Math.Sign(dy);
                for (int i = 1; i <= Math.Abs(dy); i++)
                {
                    if (CollidesAt(newPos.X, newPos.Y+ stepY))
                        break;
                    newPos.Y += stepY;
                }
            }

            return newPos;
        }

        public virtual void Draw()
        {
            if (!board.Controls.Contains(sprite))
            {
                board.Controls.Add(sprite);
                sprite.BringToFront();
            }
            sprite.Location = position;
            sprite.Visible = true;
        }



        public virtual void Move(float deltaTime)
        {
            
        }


        public Rectangle Bounds => new Rectangle(position, sprite.Size);

        public virtual void Destroy()
        {
            if (board.Controls.Contains(sprite))
                board.Controls.Remove(sprite);
            sprite.Dispose();
            entities.Remove(this);
        }

    }

    public class Player : Entity
    {
        private Bitmap[] framesUp;
        private Bitmap[] framesDown;
        private int currentFrame = 0;
        private float animationTimer = 0f;
        private float animationSpeed = 0.12f;
        private bool facingDown = true;


        private HashSet<Keys> pressedKeys;
        private float shootCooldown = 0f;
        public float shootDelay = GameSettings.PlayerShootingSpeed;

        public float speedBoostTimer = 0;
        public float fireRateTimer = 0;
        public float damageRateTimer = 0;
        public float multishotTimer = 0;

        public bool multishot = false;

        public Player(int speed, int health, int damage, PictureBox sprite, Point position, HashSet<Keys> keys, Board board, Bitmap[] framesUp, Bitmap[] framesDown)
        : base(speed, health, damage, sprite, position, board)
        {
            this.framesUp = framesUp;
            this.framesDown = framesDown;
            this.pressedKeys = keys;
        }

        public override void Update(float deltaTime)
        {
            if (health <= 0)
            {
                board.YouLost();
            }

            Move(deltaTime);
            Shoot(deltaTime);
            Draw();
            Animate(deltaTime);
            UpdateItems(deltaTime);
        }

        protected override bool CollidesAt(int x, int y)
        {
            Point testPos = new Point(x, y);
            Rectangle testRect = new Rectangle(testPos, sprite.Size);
            bool collision = obstacles.Any(b => b.Bounds.IntersectsWith(testRect)) ||
                             entities.Any(e => (!object.ReferenceEquals(e, this) &&
                                                         e.Bounds.IntersectsWith(testRect))) ||
                             board.spawnAreas.Any(a => a.Bounds.IntersectsWith(testRect));
            return collision;
        }

        private void UpdateItems(float deltatime)
        {
            speedBoostTimer = Math.Max(speedBoostTimer - deltatime, 0);
            fireRateTimer = Math.Max(fireRateTimer - deltatime, 0);
            damageRateTimer = Math.Max(damageRateTimer - deltatime, 0);
            multishotTimer = Math.Max(multishotTimer - deltatime, 0);
            if (speedBoostTimer == 0) speed = GameSettings.PlayerSpeed;
            if (fireRateTimer == 0) shootDelay = GameSettings.PlayerShootingSpeed;
            if (damageRateTimer == 0) damage = GameSettings.PlayerDamage;
            if (multishotTimer == 0) multishot = false;
        }

        private void Animate(float deltaTime)
        {
            animationTimer += deltaTime;
            if (animationTimer >= animationSpeed)
            {
                animationTimer = 0f;
                currentFrame = (currentFrame + 1) % framesUp.Length;
                sprite.Image = facingDown ? framesDown[currentFrame] : framesUp[currentFrame];
            }
        }

        public override void Move(float deltaTime) 
        {
            bool up = pressedKeys.Contains(Keys.W);
            bool down = pressedKeys.Contains(Keys.S);
            bool left = pressedKeys.Contains(Keys.A);
            bool right = pressedKeys.Contains(Keys.D);

            facingDown = !up;

            float moveAmount = speed * deltaTime;
            float diagonalFactor = GameSettings.diagonalMove;

            int dx = 0, dy = 0;

            if (up) dy = -1;
            else if (down) dy = 1;
            if (left) dx = -1;
            else if (right) dx = 1;

            bool isDiagonal = dx != 0 && dy != 0;
            float factor = isDiagonal ? diagonalFactor : 1.0f;
            dx *= (int)(moveAmount * factor);
            dy *= (int)(moveAmount * factor);

            Point newPos = GetMaxPosition(dx, dy);
            position = newPos;
        }


        private void Shoot(float deltaTime)
        {
            if (shootCooldown > 0f)
            {
                shootCooldown -= deltaTime;
                return;
            }

            bool up = pressedKeys.Contains(Keys.Up);
            bool down = pressedKeys.Contains(Keys.Down);
            bool left = pressedKeys.Contains(Keys.Left);
            bool right = pressedKeys.Contains(Keys.Right);
            int dx = 0, dy = 0;

            if (up) dy = -1;
            else if (down) dy = 1;
            if (left) dx = -1;
            else if (right) dx = 1;

            if (dx == 0 && dy == 0) return; // Nestřílíme vůbec

            if (multishot)
            {
                for (int ddx = -1; ddx < 2; ddx++)
                {
                    for (int ddy = -1; ddy < 2; ddy++)
                    {
                        if (ddx == 0 && ddy == 0) continue;
                        entities.Add(new Bullet(GameSettings.BulletSpeed, GameSettings.BulletHealth, this.damage, sprite, position, board, ddx, ddy));
                    }
                }
            }

            else
            {
                entities.Add(new Bullet(GameSettings.BulletSpeed, GameSettings.BulletHealth, this.damage, sprite, position, board, dx, dy));
            }

            shootCooldown = shootDelay;
        }

    }


    public class Enemy : Entity
    {
        private Bitmap[] framesLeft;
        private Bitmap[] framesRight;
        private int currentFrame = 0;
        private float animationTimer = 0f;
        private float animationSpeed = 0.12f;
        private bool facingRight = true;


        private static Dictionary<(Point, Point), List<Point>> pathCache = new Dictionary<(Point, Point), List<Point>>();
        private Point lastStart = Point.Empty;
        private Point lastGoal = Point.Empty;
        private List<Point> lastPath = null;
        private int lastPathIndex = 0;


        private List<PictureBox> pathBoxes = new List<PictureBox>();

        public Enemy(int speed, int health, int damage, PictureBox sprite, Point position, Board board, Bitmap[] framesLeft, Bitmap[] framesRight)
        : base(speed, health, damage, sprite, position, board)
        {
            this.framesLeft = framesLeft;
            this.framesRight = framesRight;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (this.health == 0){
                board.score++;
                TryDropItem();
            }
            Animate(deltaTime);
        }

        private void Animate(float deltaTime)
        {
            animationTimer += deltaTime;
            if (animationTimer >= animationSpeed)
            {
                animationTimer = 0f;
                currentFrame = (currentFrame + 1) % framesLeft.Length;
                sprite.Image = facingRight ? framesRight[currentFrame] : framesLeft[currentFrame];
            }
        }

        private (int, int) PathFinding(Point goal)
        {
            Point gridStart = RoundPointToGrid(position);
            Point gridGoal = RoundPointToGrid(goal);

            if (lastPath != null && lastStart == gridStart && lastGoal == gridGoal && lastPathIndex < lastPath.Count - 1)
            {
                lastPathIndex++;
                Point next = lastPath[lastPathIndex];
                return (Math.Sign(next.X - gridStart.X), Math.Sign(next.Y - gridStart.Y));
            }

            var cacheKey = (gridStart, gridGoal);
            if (pathCache.TryGetValue(cacheKey, out var cachedPath) && cachedPath.Count > 1)
            {
                lastPath = cachedPath;
                lastStart = gridStart;
                lastGoal = gridGoal;
                lastPathIndex = 1;
                Point next = lastPath[lastPathIndex];
                return (Math.Sign(next.X - gridStart.X), Math.Sign(next.Y - gridStart.Y));
            }

            var prevFromStart = new Dictionary<Point, Point>();
            var prevFromGoal = new Dictionary<Point, Point>();
            var queueStart = new Queue<Point>();
            var queueGoal = new Queue<Point>();
            var visitedStart = new HashSet<Point>();
            var visitedGoal = new HashSet<Point>();

            queueStart.Enqueue(gridStart);
            queueGoal.Enqueue(gridGoal);
            visitedStart.Add(gridStart);
            visitedGoal.Add(gridGoal);

            Point? meetPoint = null;

            int[] dxx = { 0, 0, -1, 1, -1, -1, 1, 1 };
            int[] dyy = { -1, 1, 0, 0, -1, 1, -1, 1 };

            while (queueStart.Count > 0 && queueGoal.Count > 0)
            {
                if (Expand(queueStart, visitedStart, visitedGoal, prevFromStart, prevFromGoal, out meetPoint, dxx, dyy)) break;
                if (Expand(queueGoal, visitedGoal, visitedStart, prevFromGoal, prevFromStart, out meetPoint, dxx, dyy)) break;
            }

            if (meetPoint != null)
            {
                var path = new List<Point>();
                Point p = meetPoint.Value;
                while (p != gridStart)
                {
                    path.Add(p);
                    p = prevFromStart[p];
                }
                path.Add(gridStart);
                path.Reverse();

                if (prevFromGoal.ContainsKey(meetPoint.Value))
                {
                    p = prevFromGoal[meetPoint.Value];
                    while (p != gridGoal)
                    {
                        path.Add(p);
                        p = prevFromGoal[p];
                    }
                    path.Add(gridGoal);
                }
                else
                {
                    path.Add(gridGoal);
                }

                pathCache[cacheKey] = path;
                lastPath = path;
                lastStart = gridStart;
                lastGoal = gridGoal;
                lastPathIndex = 1;
                if (path.Count > 1)
                {
                    Point next = path[1];
                    return (Math.Sign(next.X - gridStart.X), Math.Sign(next.Y - gridStart.Y));
                }
            }


            lastPath = null;
            return (0, 0);
        }

        private bool Expand(
            Queue<Point> queue,
            HashSet<Point> visitedThis,
            HashSet<Point> visitedOther,
            Dictionary<Point, Point> prevThis,
            Dictionary<Point, Point> prevOther,
            out Point? meetPoint,
            int[] dxx, int[] dyy)
        {
            meetPoint = null;
            if (queue.Count == 0) return false;
            Point current = queue.Dequeue();
            for (int dir = 0; dir < 8; dir++)
            {
                int nx = current.X + dxx[dir];
                int ny = current.Y + dyy[dir];
                Point next = new Point(nx, ny);
                Rectangle testRect = new Rectangle(GridToCoords(next), sprite.Size);
                bool collision = obstacles.Any(b => b.Bounds.IntersectsWith(testRect)) || board.spawnAreas.Any(b => b.Bounds.IntersectsWith(testRect));
                if (collision) continue;
                if (!visitedThis.Contains(next))
                {
                    visitedThis.Add(next);
                    prevThis[next] = current;
                    queue.Enqueue(next);
                    if (visitedOther.Contains(next))
                    {
                        meetPoint = next;
                        return true;
                    }
                }
            }
            return false;
        }


        public override void Move(float deltaTime)
        {
            var (dx, dy) = PathFinding(board.player.position);

            if (dx < 0) facingRight = false;
            else if (dx > 0) facingRight = true;

            float moveAmount = speed * deltaTime;
            float diagonalFactor = GameSettings.diagonalMove;

            bool isDiagonal = dx != 0 && dy != 0;
            float factor = isDiagonal ? diagonalFactor : 1.0f;
            dx *= (int)(moveAmount * factor);
            dy *= (int)(moveAmount * factor);

            Point testPos = new Point(position.X+dx, position.Y+dy);
            Rectangle testRect = new Rectangle(testPos, sprite.Size);

            if (testRect.IntersectsWith(board.player.Bounds)) 
            {
                board.player.health--;
                IsDestroyed = true;
                return;
            }

            Point newPos = GetMaxPosition(dx, dy);
            position = newPos;
        }

        private Point RoundPointToGrid(Point coords)
        {
            Point new_coords = new Point();
            new_coords.X = (coords.X - board.offsetX) / GameSettings.CellSize;
            new_coords.Y = (coords.Y - board.offsetY) / GameSettings.CellSize;
            return new_coords;
        }

        private Point GridToCoords(Point coords)
        {
            Point new_coords = new Point();
            new_coords.X = coords.X * GameSettings.CellSize + board.offsetX;
            new_coords.Y = coords.Y * GameSettings.CellSize + board.offsetY;
            return new_coords;
        }

        public void TryDropItem()
        {
            Random rnd = new Random();
            int chance = rnd.Next(0, 100);
            if (chance < GameSettings.itemDropChance)
            {
                Array values = Enum.GetValues(typeof(ItemType));
                ItemType randomType = (ItemType)values.GetValue(rnd.Next(values.Length));
                board.items.Add(new Item(randomType, 1, position, board));  
            }
        }
    }


    class Bullet : Entity
    {
        float fx, fy;
        float fdx, fdy;

        public Bullet(int speed, int health, int damage, PictureBox sprite, Point position, Board board, int dx, int dy)
            : base(speed, health, damage, sprite, position, board)
        {
            position = new Point(position.X + 20 * dx, position.Y + 20 * dy);


            RotateFlipType rt = RotateFlipType.RotateNoneFlipNone;
            Size bulletSize = GameSettings.BulletSize;

            if (dy == -1) rt = RotateFlipType.RotateNoneFlipNone;
            else if (dy == 1) rt = RotateFlipType.Rotate180FlipNone; 
            if (dx == -1) {rt = RotateFlipType.Rotate270FlipNone; bulletSize = new Size(bulletSize.Height, bulletSize.Width); }
            else if (dx == 1) {rt = RotateFlipType.Rotate90FlipNone; bulletSize = new Size(bulletSize.Height, bulletSize.Width); }

            this.sprite = new PictureBox
            {
                Size = bulletSize,
                Location = position,
                BackColor = Color.Transparent
            };

            // Nejprve změňte velikost, pak rotujte
            Bitmap bulletImage = new Bitmap(Resources.bullet);
            bulletImage.RotateFlip(rt);
            this.sprite.Image = new Bitmap(bulletImage, bulletSize);

            board.Controls.Add(this.sprite);

            fx = position.X;
            fy = position.Y;
            float len = (float)Math.Sqrt(dx * dx + dy * dy);
            if (len == 0) { fdx = 0; fdy = 0; }
            else { fdx = dx / len; fdy = dy / len; }
        }

        public override void Move(float deltaTime)
        {
            float moveAmount = speed * deltaTime;
            fx += fdx * moveAmount;
            fy += fdy * moveAmount;

            position = new Point((int)Math.Round(fx), (int)Math.Round(fy));

            Rectangle bulletRect = new Rectangle(position, sprite.Size);

            if (obstacles.Any(b => b.Bounds.IntersectsWith(bulletRect)) || board.spawnAreas.Any(b => b.Bounds.IntersectsWith(bulletRect)))
            {
                IsDestroyed = true;
                return;
            }

            foreach (var entity in entities.ToList())
            {
                if (entity == this || entity is Player) continue;
                if (entity.Bounds.IntersectsWith(bulletRect))
                {
                    entity.health -= damage;
                    IsDestroyed = true;
                    return;
                }
            }
        }

    }

}

