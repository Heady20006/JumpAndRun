using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JumpAndRun.Classes
{
    class Map
    {
        public string BackgroundColor { get; set; }
        public List<Block> GroundBlocks { get; set; }
        public List<Block> WorldBlocks { get; set; }
        public List<Block> EnemyBlocks { get; set; }
        public List<Block> CollectableBlocks { get; set; }
        public List<Block> PortalBlocks { get; set; }

        public List<Block> LoadBlocks()
        {
            List<Block> retVal = new List<Block>();

            return retVal;
        }
    }
}
