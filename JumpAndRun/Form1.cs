using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace JumpAndRun
{
    public partial class Form1 : Form
    {
        //Worlddetails
        int groundZero;
        Point spawnPoint;
        List<Block> groundBlocks = new List<Block>();
        List<Block> worldBlocks = new List<Block>();
        List<Block> enemyBlocks = new List<Block>();
        List<Block> collectableBlocks = new List<Block>();
        List<Block> portalBlocks = new List<Block>();
        Block targetBlock = new Block(Block.TypeOfBlock.World);
        int blockSize;


        //Player- and Movementdetails
        Block player;
        bool gameOver;
        bool canDrop = true;
        bool canJump = true;
        bool isPressed = false;
        int score;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);
            CreateWorld(this);
            player = new Block(Block.TypeOfBlock.Player)
            {
                Location = spawnPoint,
                Width = this.blockSize,
                Height = this.blockSize
            };
            this.Controls.Add(player);
            gameOver = false;
            score = 0;
        }

        private Tuple<int, int> GetCoordsFromString(string coords)
        {
            int xCoord = Convert.ToInt32(coords.Split('/')[0]);
            int yCoord = Convert.ToInt32(coords.Split('/')[1]);
            return Tuple.Create(xCoord, this.groundZero - yCoord);
        }


        public void CreateWorld(Form1 formCtx)
        {
            var xml = XDocument.Load(@"map.xml");
            var root = from c in xml.Root.Descendants("GroundZero")
                       select c.Value;
            foreach (var item in root)
            {
                this.groundZero = Convert.ToInt32(item);
            }

            root = from c in xml.Root.Descendants("BlockSize")
                   select c.Value;
            foreach (var item in root)
            {
                this.blockSize = Convert.ToInt32(item);
            }

            //Creating SpawnPoint
            var query = from c in xml.Root.Descendants("SpawnPoint")
                        select $"{c.Element("X").Value}/{c.Element("Y").Value}";
            foreach (var coords in query)
            {
                int xCoord = GetCoordsFromString(coords).Item1;
                int yCoord = GetCoordsFromString(coords).Item2;
                spawnPoint = new Point(xCoord, yCoord);
            }

            //Creating GroundBlocks
            for (int i = 0; i < 5000; i += this.blockSize)
            {
                Block newBlock = new Block(Block.TypeOfBlock.Ground)
                {
                    Location = new Point(i, this.groundZero),
                    Width = this.blockSize,
                    Height = this.blockSize
                };
                groundBlocks.Add(newBlock);
                formCtx.Controls.Add(newBlock);
            }
            query = from c in xml.Root.Descendants("GroundBlocks")
                    select $"{c.Element("X").Value}";

            foreach (string coords in query)
            {
                int xCoord = Convert.ToInt32(coords);
                List<Block> blockToRemove = new List<Block>();
                foreach (var block in groundBlocks)
                {
                    if (block.Location.Equals(new Point(xCoord, this.groundZero)))
                    {
                        blockToRemove.Add(block);
                        formCtx.Controls.Remove(block);
                        block.Invalidate();
                    }
                }
                foreach (var block in blockToRemove)
                {
                    groundBlocks.Remove(block);
                }
            }

            //Creating WorldBlocks
            query = from c in xml.Root.Descendants("WorldBlocks")
                    select $"{c.Element("X").Value}/{c.Element("Y").Value}";

            foreach (string coords in query)
            {
                int xCoord = GetCoordsFromString(coords).Item1;
                int yCoord = GetCoordsFromString(coords).Item2;
                Block newBlock = new Block(Block.TypeOfBlock.World)
                {
                    Location = new Point(xCoord, yCoord),
                    Width = this.blockSize,
                    Height = this.blockSize
                };
                worldBlocks.Add(newBlock);
                formCtx.Controls.Add(newBlock);
            }

            //Creating CollectableBlocks
            query = from c in xml.Root.Descendants("CollectableBlocks")
                    select $"{c.Element("X").Value}/{c.Element("Y").Value}";

            foreach (string coords in query)
            {
                int xCoord = GetCoordsFromString(coords).Item1;
                int yCoord = GetCoordsFromString(coords).Item2;
                Block newBlock = new Block(Block.TypeOfBlock.Collectable)
                {
                    Location = new Point(xCoord, yCoord),
                    Width = this.blockSize,
                    Height = this.blockSize
                };
                collectableBlocks.Add(newBlock);
                formCtx.Controls.Add(newBlock);
            }

            //Creating EnemyBlocks
            query = from c in xml.Root.Descendants("EnemyBlocks")
                    select $"{c.Element("X").Value}/{c.Element("Y").Value}";

            foreach (string coords in query)
            {
                int xCoord = GetCoordsFromString(coords).Item1;
                int yCoord = GetCoordsFromString(coords).Item2;
                Block newBlock = new Block(Block.TypeOfBlock.Enemy)
                {
                    Location = new Point(xCoord, yCoord),
                    Width = this.blockSize,
                    Height = this.blockSize
                };
                enemyBlocks.Add(newBlock);
                formCtx.Controls.Add(newBlock);
            }

            //Creating PortalBlocks
            query = from c in xml.Root.Descendants("PortalBlocks")
                    select $"{c.Element("X").Value}/{c.Element("Y").Value}";

            foreach (string coords in query)
            {
                int xCoord = GetCoordsFromString(coords).Item1;
                int yCoord = GetCoordsFromString(coords).Item2;
                Block newBlock = new Block(Block.TypeOfBlock.Portal)
                {
                    Location = new Point(xCoord, yCoord),
                    Width = this.blockSize,
                    Height = this.blockSize
                };
                portalBlocks.Add(newBlock);
                formCtx.Controls.Add(newBlock);
            }

        }

        private Block GetBlockAtCoords(Point targetLocation)
        {
            for (int i = 0; i <= enemyBlocks.Count - 1; i++)
            {
                if (enemyBlocks[i].Location.Equals(targetLocation))
                {
                    return enemyBlocks[i];
                }
            }

            for (int i = 0; i <= worldBlocks.Count - 1; i++)
            {
                if (worldBlocks[i].Location.Equals(targetLocation))
                {
                    return worldBlocks[i];
                }
            }

            for (int i = 0; i <= groundBlocks.Count - 1; i++)
            {
                if (groundBlocks[i].Location.Equals(targetLocation))
                {
                    return groundBlocks[i];
                }
            }

            for (int i = 0; i <= collectableBlocks.Count - 1; i++)
            {
                if (collectableBlocks[i].Location.Equals(targetLocation))
                {
                    return collectableBlocks[i];
                }
            }

            for (int i = 0; i <= portalBlocks.Count - 1; i++)
            {
                if (portalBlocks[i].Location.Equals(targetLocation))
                {
                    return portalBlocks[i];
                }
            }
            return new Block(Block.TypeOfBlock.Free)
            {
                Width = this.blockSize,
                Height = this.blockSize
            };
        }

        private void GravityTimer_Tick(object sender, EventArgs e)
        {
            if (gameOver) return;
            canDrop = true;
            Point newLoc = new Point(player.Location.X, player.Location.Y + this.blockSize);
            targetBlock = GetBlockAtCoords(newLoc);
            if (targetBlock.Type == Block.TypeOfBlock.World || targetBlock.Type == Block.TypeOfBlock.Ground)
            {
                canDrop = false;
            }

            if (canDrop)
            {
                player.Location = newLoc;
            }

            if (targetBlock.Type == Block.TypeOfBlock.Collectable)
            {
                score += 1;
                label2.Text = score.ToString();
                collectableBlocks.Remove(targetBlock);
                targetBlock.Invalidate();
                this.Controls.Remove(targetBlock);
            }

            if (targetBlock.Type == Block.TypeOfBlock.Enemy)
            {
                AddScore(10);
                player.Location = newLoc;
                enemyBlocks.Remove(targetBlock);
                targetBlock.Invalidate();
                this.Controls.Remove(targetBlock);
            }

            if (targetBlock.Type == Block.TypeOfBlock.Portal)
            {
                GameOver("win");
            }
            if (player.Location.Y > this.groundZero)
            {
                GameOver("lose");
            }
        }

        private void MovementTimer_Tick(object sender, EventArgs e)
        {
            if (gameOver) return;
        }

        private void WorldTimer_Tick(object sender, EventArgs e)
        {
            if (gameOver) return;
        }

        private void GameOver(string winOrLose)
        {
            MovementTimer.Stop();
            gameOver = true;
            DialogResult result = DialogResult.None;
            if (winOrLose == "win")
            {
                result = MessageBox.Show("HURRA DU HAST GEWONNEN. Ok für noch eine Runde", "Gewonnen", MessageBoxButtons.OKCancel);
            }
            if (winOrLose == "lose")
            {
                result = MessageBox.Show("Du hast das Spiel verloren. Ok für Neustart", "Verloren", MessageBoxButtons.OKCancel);
            }

            if (result == DialogResult.OK)
            {
                Application.Restart();
                Environment.Exit(0);
            }
            else
            {
                Close();
            }
        }

        private void MoveWorld(int dirOffset)
        {
            foreach (var block in groundBlocks)
            {
                block.Location = new Point(block.Location.X - dirOffset, block.Location.Y);
            }
            foreach (var block in enemyBlocks)
            {
                block.Location = new Point(block.Location.X - dirOffset, block.Location.Y);
            }
            foreach (var block in worldBlocks)
            {
                block.Location = new Point(block.Location.X - dirOffset, block.Location.Y);
            }
            foreach (var block in collectableBlocks)
            {
                block.Location = new Point(block.Location.X - dirOffset, block.Location.Y);
            }
            foreach (var block in portalBlocks)
            {
                block.Location = new Point(block.Location.X - dirOffset, block.Location.Y);
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (gameOver || (e.KeyChar != 'd' && e.KeyChar != 'a')) return;
            int dirOffset = 20;

            if (e.KeyChar == 'a')
            {
                dirOffset *= -1;
            }
            Point newLoc = new Point(player.Location.X + dirOffset, player.Location.Y);
            targetBlock = GetBlockAtCoords(newLoc);
            if (targetBlock.Type == Block.TypeOfBlock.World) return;
            MoveWorld(dirOffset);

            if (targetBlock.Type == Block.TypeOfBlock.Enemy)
            {
                GameOver("lose");
            }

            if (targetBlock.Type == Block.TypeOfBlock.Portal)
            {
                GameOver("win");
            }

            if (targetBlock.Type == Block.TypeOfBlock.Collectable)
            {
                AddScore(1);
                player.Location = newLoc;
                collectableBlocks.Remove(targetBlock);
                targetBlock.Invalidate();
                this.Controls.Remove(targetBlock);
            }

        }

        private void AddScore(int points)
        {
            score += points;
            label2.Text = score.ToString();
        }

        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (gameOver || e.KeyCode != Keys.W && e.KeyCode != Keys.E && e.KeyCode != Keys.Q) return;
            int dirOffset = 20;
            Point newLoc = Point.Empty;
            if (e.KeyCode == Keys.W && canJump && !isPressed)
            {
                dirOffset = 0;
                newLoc = new Point(player.Location.X, player.Location.Y - this.blockSize);
                targetBlock = GetBlockAtCoords(newLoc);
                if (targetBlock.Type != Block.TypeOfBlock.World || targetBlock.Type != Block.TypeOfBlock.Ground)
                {
                    player.Location = newLoc;
                }
                isPressed = true;
            }
            if (e.KeyCode == Keys.E && canJump && !isPressed)
            {
                newLoc = new Point(player.Location.X + this.blockSize, player.Location.Y - this.blockSize);
                targetBlock = GetBlockAtCoords(newLoc);
                if (targetBlock.Type == Block.TypeOfBlock.World)
                    return;

                player.Location = new Point(newLoc.X - this.blockSize, newLoc.Y);
                isPressed = true;
                MoveWorld(dirOffset);
            }
            if (e.KeyCode == Keys.Q && canJump && !isPressed)
            {
                dirOffset *= -1;
                newLoc = new Point(player.Location.X - this.blockSize, player.Location.Y - this.blockSize);
                targetBlock = GetBlockAtCoords(newLoc);
                if (targetBlock.Type == Block.TypeOfBlock.World)
                    return;

                player.Location = new Point(newLoc.X + this.blockSize, newLoc.Y);
                isPressed = true;
                MoveWorld(dirOffset);
            }
        }

        private void Form1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.E || e.KeyCode == Keys.Q)
            {
                isPressed = false;
            }
        }
    }
}
