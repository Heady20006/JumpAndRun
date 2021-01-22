using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JumpAndRun
{
    public partial class Form1 : Form
    {

        List<Block> groundBlocks = new List<Block>();
        Block player;
        bool canDrop = true;

        public Form1()
        {
            InitializeComponent();
            player = new Block(Block.TypeOfBlock.Player);
            player.Location = new Point(0, 350);
            this.Controls.Add(player);
            CreateWorld();
        }

        private void CreateWorld()
        {
            for (int i = 0; i < this.ClientRectangle.Width; i += 20)
            {
                Block gBlock = new Block(Block.TypeOfBlock.World)
                {
                    Location = new Point(i, this.ClientRectangle.Height - 40),
                };
                groundBlocks.Add(gBlock);
                this.Controls.Add(gBlock);
            };
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            canDrop = true;
            Point newLoc = new Point(player.Location.X, player.Location.Y + 20);
            for (int i = -1; i < groundBlocks.Count - 1; i++)
            {
                if (player.Location.Equals(groundBlocks[i + 1].Location))
                {
                    canDrop = false;
                }
            }

            if (canDrop)
            {
                player.Location = newLoc;
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'd')
            {
                player.Location = new Point(player.Location.X + 20, this.player.Location.Y);
            }
            if (e.KeyChar == 'a')
            {
                player.Location = new Point(player.Location.X - 20, this.player.Location.Y);
            }
            if (e.KeyChar == 'w')
            {
                player.Location = new Point(player.Location.X, this.player.Location.Y - 20);
            }
        }
    }
}
