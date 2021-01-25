using System.Drawing;
using System.Windows.Forms;

namespace JumpAndRun
{
    class Block : Panel
    {
        public TypeOfBlock Type;

        public Block(TypeOfBlock type)
        {
            this.Type = type;
            this.Width = 0;
            this.Height = 0;

            if (type == TypeOfBlock.Player)
            {
                this.BackColor = Color.Blue;
            }
            if (type == TypeOfBlock.Enemy)
            {
                this.BackColor = Color.Red;
            }
            if (type == TypeOfBlock.Ground)
            {
                this.BackColor = Color.Black;
            }
            if (type == TypeOfBlock.World)
            {
                this.BackColor = Color.Green;
            }
            if (type == TypeOfBlock.Collectable)
            {
                this.BackColor = Color.CornflowerBlue;
            }
            if (type == TypeOfBlock.Portal)
            {
                this.BackColor = Color.DeepPink;
            }
        }

        public enum TypeOfBlock
        {
            World,
            Ground,
            Player,
            Enemy,
            Portal,
            Collectable,
            Free
        }

    }
}
