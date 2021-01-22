using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JumpAndRun
{
    class Block : Panel
    {
        private TypeOfBlock Type;
        private Color Color;

        public Block(TypeOfBlock type)
        {
            this.Type = type;
            this.Width = 20;
            this.Height = 20;

            if (type == TypeOfBlock.Player)
            {
                this.BackColor = Color.Red;
                this.Location = new Point(20, 340);
            } else
            {
                this.BackColor = Color.Black;
            }
        }

        public enum TypeOfBlock
        {
            World,
            Player,
            Enemy,
            Portal,
            Collectable
        }
    }
    
}
