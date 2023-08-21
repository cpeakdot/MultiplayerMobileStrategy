namespace MMS.AI
{
    public class PathNode
    {
        private Grid grid;
        private int x;
        private int z;

        public int gCost;
        public int hCost;
        public int fCost;

        public bool walkable = true;

        public PathNode cameFromNode;

        public int GetX => x;
        public int GetZ => z;
        
        public PathNode(Grid grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        public override string ToString()
        {
            return $"{x} , {z}";
        }
    }
}

