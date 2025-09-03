using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public Entity(int speed, int health, int damage, Point position, Board board)
        {
            this.speed = speed;
            this.health = health;
            this.damage = damage;
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
            Rectangle testRect = new Rectangle(testPos, sprite.Size); // If any two rectangles interact at (x,y) there is a collision
            bool collision = obstacles.Any(b => b.Bounds.IntersectsWith(testRect)) ||
                                     entities.Any(e => (!object.ReferenceEquals(e, this) &&
                                                         e.Bounds.IntersectsWith(testRect)));
            return collision;
        }

        protected Point GetMaxPosition(int dx, int dy)
        {
            Point newPos = position;

            if (dx != 0)
            {
                int stepX = Math.Sign(dx);
                for (int i = 1; i <= Math.Abs(dx); i++)
                {
                    if (CollidesAt(newPos.X + stepX, newPos.Y)) // Gradually tries to move in the right x direction
                        break;
                    newPos.X += stepX;
                }
            }

            if (dy != 0)
            {
                int stepY = Math.Sign(dy);
                for (int i = 1; i <= Math.Abs(dy); i++)
                {
                    if (CollidesAt(newPos.X, newPos.Y + stepY))
                        break;
                    newPos.Y += stepY;
                }
            }

            return newPos;
        }

        protected virtual void Draw()
        {
            if (!board.Controls.Contains(sprite))
            {
                board.Controls.Add(sprite);
                sprite.BringToFront();
            }
            sprite.Location = position;
            sprite.Visible = true;
        }



        protected virtual void Move(float deltaTime)
        {

        }


        public Rectangle Bounds => new Rectangle(position, sprite.Size); // Call Bounds to get the current hitbox of the entity

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
        private float animationSpeed = GameSettings.animationSpeed;
        private bool facingDown = true;


        private HashSet<Keys> pressedKeys;
        private float shootCooldown = 0f;
        public float shootDelay = GameSettings.PlayerShootingSpeed;

        public float speedBoostTimer = 0;
        public float fireRateTimer = 0;
        public float damageRateTimer = 0;
        public float multishotTimer = 0;

        public bool multishot = false;

        public Player(int speed, int health, int damage, Point position, HashSet<Keys> keys, Board board)
        : base(speed, health, damage, position, board)
        {
            this.pressedKeys = keys;

            sprite = new PictureBox
            {
                Size = new Size(GameSettings.CellSize, GameSettings.CellSize),
                Location = position,
                BackColor = Color.Transparent
            };
            sprite.Image = new Bitmap(Properties.Resources.front_player_1, GameSettings.SpriteSize);
            board.Controls.Add(sprite);

            framesUp = new Bitmap[]
            {
                                    new Bitmap(Properties.Resources.back_player_1, GameSettings.SpriteSize),
                                    new Bitmap(Properties.Resources.back_player_2, GameSettings.SpriteSize)
            };

            framesDown = new Bitmap[]
            {
                                    new Bitmap(Properties.Resources.front_player_1, GameSettings.SpriteSize),
                                    new Bitmap(Properties.Resources.front_player_2, GameSettings.SpriteSize)
            };
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
            bool collision = base.CollidesAt(x, y) ||
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

        protected override void Move(float deltaTime)
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
                        entities.Add(new Bullet(GameSettings.BulletSpeed, GameSettings.BulletHealth, this.damage, position, board, ddx, ddy));
                    }
                }
            }

            else
            {
                entities.Add(new Bullet(GameSettings.BulletSpeed, GameSettings.BulletHealth, this.damage, position, board, dx, dy));
            }

            shootCooldown = shootDelay;
        }

    }

    public enum EnemyType
    {
        GreenSlime,
        RedSlime
    }

    public static class EnemySpriteCache
    {
        public static Dictionary<EnemyType, Bitmap[]> LeftSprites { get; private set; }
        public static Dictionary<EnemyType, Bitmap[]> RightSprites { get; private set; }

        public static void Initialize()
        {
            LeftSprites = new Dictionary<EnemyType, Bitmap[]>
            {
                [EnemyType.GreenSlime] = new Bitmap[]
                {
                new Bitmap(Properties.Resources.green_slime_4, GameSettings.SpriteSize),
                new Bitmap(Properties.Resources.green_slime_5, GameSettings.SpriteSize),
                new Bitmap(Properties.Resources.green_slime_6, GameSettings.SpriteSize)
                },
                [EnemyType.RedSlime] = new Bitmap[]
                {
                new Bitmap(Properties.Resources.red_slime_4, GameSettings.SpriteSize),
                new Bitmap(Properties.Resources.red_slime_5, GameSettings.SpriteSize),
                new Bitmap(Properties.Resources.red_slime_6, GameSettings.SpriteSize)
                }
            };

            RightSprites = new Dictionary<EnemyType, Bitmap[]>
            {
                [EnemyType.GreenSlime] = new Bitmap[]
                {
                new Bitmap(Properties.Resources.green_slime_1, GameSettings.SpriteSize),
                new Bitmap(Properties.Resources.green_slime_2, GameSettings.SpriteSize),
                new Bitmap(Properties.Resources.green_slime_3, GameSettings.SpriteSize)
                },
                [EnemyType.RedSlime] = new Bitmap[]
                {
                new Bitmap(Properties.Resources.red_slime_1, GameSettings.SpriteSize),
                new Bitmap(Properties.Resources.red_slime_2, GameSettings.SpriteSize),
                new Bitmap(Properties.Resources.red_slime_3, GameSettings.SpriteSize)
                }
            };
        }

        public static Bitmap[] GetSpritesLeft(EnemyType type) => LeftSprites[type];
        public static Bitmap[] GetSpritesRight(EnemyType type) => RightSprites[type];
    }

    public class Enemy : Entity
    {
        private Bitmap[] framesLeft;
        private Bitmap[] framesRight;
        private int currentFrame = 0;
        private float animationTimer = 0f;
        private float animationSpeed = GameSettings.animationSpeed;
        private bool facingRight = true;


        private static Dictionary<(Point, Point), List<Point>> pathCache = new Dictionary<(Point, Point), List<Point>>();
        private Point lastStart = Point.Empty;
        private Point lastGoal = Point.Empty;
        private List<Point> lastPath = null;
        private int lastPathIndex = 0;


        private List<PictureBox> pathBoxes = new List<PictureBox>();

        public Enemy(int speed, int health, int damage, Point position, Board board, EnemyType type=EnemyType.GreenSlime)
        : base(speed, health, damage, position, board)
        {
            framesLeft = EnemySpriteCache.GetSpritesLeft(type);
            framesRight = EnemySpriteCache.GetSpritesRight(type);

            sprite = new PictureBox
            {
                Size = new Size(GameSettings.CellSize, GameSettings.CellSize),
                Location = position,
                BackColor = Color.Transparent,
                Image = framesRight[0]
            };
            board.Controls.Add(sprite);
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
                // If last path exists, last start and goal and this start and goal match and it is not the last step of the path:
            {
                lastPathIndex++; // Next step of the path
                Point next = lastPath[lastPathIndex];
                return (Math.Sign(next.X - gridStart.X), Math.Sign(next.Y - gridStart.Y)); // Return the next step in the path
            }

            var cacheKey = (gridStart, gridGoal);
            if (pathCache.TryGetValue(cacheKey, out var cachedPath) && cachedPath.Count > 1)
                // If this start and this goal are in pathCahce get it
            {
                lastPath = cachedPath; 
                lastStart = gridStart;
                lastGoal = gridGoal;
                lastPathIndex = 1;
                Point next = lastPath[lastPathIndex];
                return (Math.Sign(next.X - gridStart.X), Math.Sign(next.Y - gridStart.Y)); // Return the first step of the cached path
            }

            var prevFromStart = new Dictionary<Point, Point>(); // For BFS to reconstruct the path
            var prevFromGoal = new Dictionary<Point, Point>();
            var queueStart = new Queue<Point>(); // Queue for BFS
            var queueGoal = new Queue<Point>();
            var visitedStart = new HashSet<Point>();
            var visitedGoal = new HashSet<Point>();

            queueStart.Enqueue(gridStart);
            queueGoal.Enqueue(gridGoal);
            visitedStart.Add(gridStart);
            visitedGoal.Add(gridGoal);

            Point? meetPoint = null;


            while (queueStart.Count > 0 && queueGoal.Count > 0) // BFS where the "wave" expands from both the start and the goal
            {
                if (Expand(queueStart, visitedStart, visitedGoal, prevFromStart, prevFromGoal, out meetPoint)) break; // If the meetpoint is found, break out from the loop
                if (Expand(queueGoal, visitedGoal, visitedStart, prevFromGoal, prevFromStart, out meetPoint)) break;
            }

            if (meetPoint != null)
            {
                var path = new List<Point>();
                Point p = meetPoint.Value;
                while (p != gridStart) // Reconstruct the path from meetpoint to the start
                {
                    path.Add(p);
                    p = prevFromStart[p];
                }
                path.Add(gridStart);
                path.Reverse();

                if (prevFromGoal.ContainsKey(meetPoint.Value)) // If meetpoint has been found by the BFS from goal
                {
                    p = prevFromGoal[meetPoint.Value];
                    while (p != gridGoal) // Reconstruct the path from meetpoint to the goal
                    {
                        path.Add(p);
                        p = prevFromGoal[p];
                    }
                    path.Add(gridGoal);
                }
                else // Else the path has length 0 and the only step is the gridGoal
                {
                    path.Add(gridGoal);
                }

                pathCache[cacheKey] = path; // Cache the path (also could be cached globally in the board and reset at each level)
                lastPath = path;
                lastStart = gridStart;
                lastGoal = gridGoal;
                lastPathIndex = 1;
                if (path.Count > 1)
                {
                    Point next = path[1];
                    return (Math.Sign(next.X - gridStart.X), Math.Sign(next.Y - gridStart.Y)); // Return the next step in the path
                }
            }


            lastPath = null; // If no path has been found return (0.0) for no next step
            return (0, 0);
        }

        private bool Expand(Queue<Point> queue, HashSet<Point> visitedThis, HashSet<Point> visitedOther, 
            Dictionary<Point, Point> prevThis,Dictionary<Point, Point> prevOther,out Point? meetPoint)
            // Clasic BFS logic without the loop, but in a function so it can be called twice per iteration
        {

            meetPoint = null;
            if (queue.Count == 0) return false;
            Point current = queue.Dequeue();

            int[] dxx = { 0, 0, -1, 1, -1, -1, 1, 1 }; // Is used instead of two loops
            int[] dyy = { -1, 1, 0, 0, -1, 1, -1, 1 }; // Is used instead of two loops

            for (int dir = 0; dir < 8; dir++) // Check all neighbours
            {
                int nx = current.X + dxx[dir];
                int ny = current.Y + dyy[dir];
                Point next = new Point(nx, ny);
                if (CollidesAt(nx, ny)) continue;
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


        protected override void Move(float deltaTime)
        {
            var (dx, dy) = PathFinding(board.player.position); // Find the next step

            if (dx < 0) facingRight = false; // Useed for animating the enemy
            else if (dx > 0) facingRight = true;

            float moveAmount = speed * deltaTime;

            bool isDiagonal = dx != 0 && dy != 0;
            float factor = isDiagonal ? GameSettings.diagonalMove : 1.0f;
            dx *= (int)(moveAmount * factor); // Convert direction into number of pixels
            dy *= (int)(moveAmount * factor);

            Point testPos = new Point(position.X+dx, position.Y+dy);
            Rectangle testRect = new Rectangle(testPos, sprite.Size);

            if (testRect.IntersectsWith(board.player.Bounds)) // Check if enemy can hit the player
            {
                board.player.health--;
                IsDestroyed = true;
                return;
            }

            Point newPos = GetMaxPosition(dx, dy);
            position = newPos;
        }

        private Point RoundPointToGrid(Point coords)
            // Converts the position in pixels into a position on a board
        {
            Point new_coords = new Point();
            new_coords.X = (coords.X - board.offsetX) / GameSettings.CellSize;
            new_coords.Y = (coords.Y - board.offsetY) / GameSettings.CellSize;
            return new_coords;
        }

        private void TryDropItem()
        {
            Random rnd = new Random();
            int chance = rnd.Next(0, 100);
            if (chance < GameSettings.itemDropChance)
            {
                Array values = Enum.GetValues(typeof(ItemType));
                ItemType randomType = (ItemType)values.GetValue(rnd.Next(values.Length));
                board.items.Add(new Item(randomType, position, board));  
            }
        }
    }


    class Bullet : Entity
    {
        float fx, fy; // Current position of the bullet
        float fdx, fdy; // Vector of the velocity

        public Bullet(int speed, int health, int damage, Point position, Board board, int dx, int dy)
            : base(speed, health, damage, position, board)
        {
            // Vypočítat střed hráče
            int centerX = position.X + GameSettings.CellSize / 2;
            int centerY = position.Y + GameSettings.CellSize / 2;

            // Vypočítat offset (10 pixelů ve směru střelby)
            int offsetX = 0, offsetY = 0;
            if (dx != 0 || dy != 0)
            {
                // Normalizace směru
                double len = Math.Sqrt(dx * dx + dy * dy);
                offsetX = (int)Math.Round(dx / len * (GameSettings.BulletOffset + GameSettings.CellSize / 2));
                offsetY = (int)Math.Round(dy / len * (GameSettings.BulletOffset + GameSettings.CellSize / 2));
            }

            // Nová pozice střely
            position = new Point(centerX + offsetX - GameSettings.BulletSize.Width / 2,
                                 centerY + offsetY - GameSettings.BulletSize.Height / 2);

            RotateFlipType rt = RotateFlipType.RotateNoneFlipNone;
            Size bulletSize = GameSettings.BulletSize;

            // Flip the sprite if need
            if (dy == -1) rt = RotateFlipType.RotateNoneFlipNone;
            else if (dy == 1) rt = RotateFlipType.Rotate180FlipNone;
            if (dx == -1) { rt = RotateFlipType.Rotate270FlipNone; bulletSize = new Size(bulletSize.Height, bulletSize.Width); }
            else if (dx == 1) { rt = RotateFlipType.Rotate90FlipNone; bulletSize = new Size(bulletSize.Height, bulletSize.Width); }

            this.sprite = new PictureBox
            {
                Size = bulletSize,
                Location = position,
                BackColor = Color.Transparent
            };

            Bitmap bulletImage = new Bitmap(Resources.bullet);
            bulletImage.RotateFlip(rt);
            this.sprite.Image = new Bitmap(bulletImage, bulletSize);

            board.Controls.Add(this.sprite);

            fx = position.X;
            fy = position.Y;
            float lenVec = (float)Math.Sqrt(dx * dx + dy * dy);
            if (lenVec == 0) { fdx = 0; fdy = 0; }
            else { fdx = dx / lenVec; fdy = dy / lenVec; }
        }

        protected override void Move(float deltaTime)
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

