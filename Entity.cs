using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using The_Rustwood_Outlaw.Properties;
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

        public Entity(int speed, int health, int damage, PictureBox sprite, Point position, Board board, List<Barricade> obstacles, List<Entity> entities)
        {
            this.speed = speed;
            this.health = health;
            this.damage = damage;
            this.sprite = sprite;
            this.position = position;
            this.board = board;
            this.obstacles = obstacles;
            this.entities = entities;

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

        protected Point GetMaxPosition(int dx, int dy)
        {
            Point newPos = position;

            // Pohyb po ose X
            if (dx != 0)
            {
                int stepX = Math.Sign(dx);
                for (int i = 1; i <= Math.Abs(dx); i++)
                {
                    Point testPos = new Point(newPos.X + stepX, newPos.Y);
                    Rectangle testRect = new Rectangle(testPos, sprite.Size);
                    bool collision = obstacles.Any(b => b.Bounds.IntersectsWith(testRect)) ||
                                     entities.Any(e => (!object.ReferenceEquals(e, this) &&
                                                         e.Bounds.IntersectsWith(testRect)));
;
                    if (collision)
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
                    Point testPos = new Point(newPos.X, newPos.Y + stepY);
                    Rectangle testRect = new Rectangle(testPos, sprite.Size);
                    bool collision = obstacles.Any(b => b.Bounds.IntersectsWith(testRect)) ||
                                     entities.Any(e => (!object.ReferenceEquals(e, this) &&
                                                         e.Bounds.IntersectsWith(testRect)));
                    if (collision)
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

    class Player : Entity
    {
        private HashSet<Keys> pressedKeys;
        private float shootCooldown = 0f;
        private float shootDelay = GameSettings.PlayerShootingSpeed;

        public Player(int speed, int health, int damage, PictureBox sprite, Point position, HashSet<Keys> keys, Board board, List<Barricade> obstacles, List<Entity> entities)
            : base(speed, health, damage, sprite, position, board, obstacles, entities)
        {
            pressedKeys = keys;
        }

        public override void Update(float deltaTime)
        {
            Move(deltaTime);
            Shoot(deltaTime);
            Draw();
        }
        public override void Move(float deltaTime) 
        {
            bool up = pressedKeys.Contains(Keys.W);
            bool down = pressedKeys.Contains(Keys.S);
            bool left = pressedKeys.Contains(Keys.A);
            bool right = pressedKeys.Contains(Keys.D);

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

        // Fix for CS1503: Argument 1: Nejde převést z void na string.
        // Fix for CS1503: Argument 2: Nejde převést z System.Drawing.Size na bool.

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
            RotateFlipType rt = RotateFlipType.RotateNoneFlipNone;
            Size bulletSize = GameSettings.BulletSize;

            // Určete směr a velikost střely
            if (up) { dy = -1; rt = RotateFlipType.RotateNoneFlipNone;}
            else if (down) { dy = 1; rt = RotateFlipType.Rotate180FlipNone;}
            if (left) { dx = -1; rt = RotateFlipType.Rotate270FlipNone; bulletSize = new Size(GameSettings.BulletSize.Height, GameSettings.BulletSize.Width); }
            else if (right) { dx = 1; rt = RotateFlipType.Rotate90FlipNone; bulletSize = new Size(GameSettings.BulletSize.Height, GameSettings.BulletSize.Width); }
            if (dx == 0 && dy == 0) return;

            Point pos = new Point(position.X + 20 * dx, position.Y + 20 * dy);
            PictureBox sprite = new PictureBox
            {
                Size = bulletSize,
                Location = pos,
                BackColor = Color.Transparent
            };

            // Nejprve změňte velikost, pak rotujte
            Bitmap bulletImage = new Bitmap(Resources.bullet);
            bulletImage.RotateFlip(rt);
            sprite.Image = new Bitmap(bulletImage, bulletSize);

            board.Controls.Add(sprite);
            entities.Add(new Bullet(GameSettings.BulletSpeed, GameSettings.BulletHealth, this.damage, sprite, pos, board, obstacles, entities, dx, dy));

            shootCooldown = shootDelay;
        }

    }


    class Enemy : Entity
    {
        private List<PictureBox> pathBoxes = new List<PictureBox>();

        public Enemy(int speed, int health, int damage, PictureBox sprite, Point position, Board board, List<Barricade> obstacles, List<Entity> entities)
            : base(speed, health, damage, sprite, position, board, obstacles, entities)
        {
        }

        private (int, int) PathFinding(Point goal)
        {
            Point gridStart = RoundPointToGrid(position);
            Point gridGoal = RoundPointToGrid(goal);

            if (gridGoal == gridStart) return (0 , 0);

            int Boardsize = GameSettings.MapSize;

            Point[] howToGetTo = new Point[Boardsize * Boardsize];
            Queue<(Point, int)> fronta = new Queue<(Point, int)>();
            fronta.Enqueue((gridStart, 0));
            howToGetTo[gridStart.Y + Boardsize * (gridStart.X)] = gridStart;
            while (fronta.Count != 0)
            {
                (Point, int) pos = fronta.Dequeue();
                Point grid_position = pos.Item1;
                int depth = pos.Item2;
                if (grid_position == gridGoal)
                {
                    Point nextStep = new Point();
                    foreach (var pb in pathBoxes)
                    {
                        if (board.Controls.Contains(pb))
                            board.Controls.Remove(pb);
                        pb.Dispose();
                    }
                    pathBoxes.Clear();
                    while (howToGetTo[grid_position.Y  + Boardsize * (grid_position.X)] != gridStart)
                    {
                        grid_position = howToGetTo[grid_position.Y + Boardsize * (grid_position.X)];


                        if (GameSettings.drawPathfinding)
                        {
                            PictureBox ss = new PictureBox
                            {
                                BackColor = Color.Yellow,
                                Size = sprite.Size,
                                Location = grid_position
                            };

                            if (!board.Controls.Contains(ss))
                            {
                                board.Controls.Add(ss);
                                ss.BringToFront();
                            }
                            ss.Location = GridToCoords(grid_position);
                            ss.Visible = true;
                            pathBoxes.Add(ss);
                        }
                        
                    }
                    int dx = 0, dy = 0;
                    Point normalPos = GridToCoords(grid_position);
                    if (normalPos.X < this.position.X) dx = -1;
                    else if (normalPos.X > this.position.X) dx = 1;
                    if (normalPos.Y < this.position.Y) dy = -1;
                    else if (normalPos.Y > this.position.Y) dy = 1;

                    return (dx, dy);
                }
                ;
                int[] dxx = { 0, 0, -1, 1, -1, -1, 1, 1 };
                int[] dyy = { -1, 1, 0, 0, -1, 1, -1, 1 };

                for (int dir = 0; dir < 8; dir++)
                {
                    int i = dxx[dir];
                    int j = dyy[dir];
                    if ((i != 0) || (j != 0))
                    {
                        Point move = new Point(grid_position.X + i, grid_position.Y + j);
                        Rectangle testRect = new Rectangle(GridToCoords(move), sprite.Size);
                        bool collision = obstacles.Any(b => b.Bounds.IntersectsWith(testRect));

                        if (Math.Abs(i) == 1 && Math.Abs(j) == 1)
                        {
                            Point moveX = new Point(grid_position.X + i, grid_position.Y);
                            Point moveY = new Point(grid_position.X, grid_position.Y + j);
                            Rectangle rectX = new Rectangle(GridToCoords(moveX), sprite.Size);
                            Rectangle rectY = new Rectangle(GridToCoords(moveY), sprite.Size);
                            bool collisionX = obstacles.Any(b => b.Bounds.IntersectsWith(rectX));
                            bool collisionY = obstacles.Any(b => b.Bounds.IntersectsWith(rectY));
                            if (collisionX || collisionY)
                                collision = true;
                        }

                        if (!collision && (howToGetTo[move.Y + Boardsize * (move.X)] == new Point()))
                        {
                            howToGetTo[move.Y + Boardsize * (move.X)] = grid_position;
                            fronta.Enqueue((move, depth + 1));
                        }
                    
                    }
                }
            }
            return (0, 0);
        }


        public override void Move(float deltaTime)
        {
            var (dx, dy) = PathFinding(board.player.position);

            float moveAmount = speed * deltaTime;
            float diagonalFactor = GameSettings.diagonalMove;

            bool isDiagonal = dx != 0 && dy != 0;
            float factor = isDiagonal ? diagonalFactor : 1.0f;
            dx *= (int)(moveAmount * factor);
            dy *= (int)(moveAmount * factor);

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
    }


    class Bullet : Entity
    {
        float fx, fy;
        float fdx, fdy;

        public Bullet(int speed, int health, int damage, PictureBox sprite, Point position, Board board, List<Barricade> obstacles, List<Entity> entities, int dx, int dy)
            : base(speed, health, damage, sprite, position, board, obstacles, entities)
        {
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

            if (obstacles.Any(b => b.Bounds.IntersectsWith(bulletRect)))
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

