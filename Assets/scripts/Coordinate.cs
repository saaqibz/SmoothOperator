using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts
{
    public class Coordinate
    {
        static Random rand = new Random(); // because creating multiple Randoms in quick succession gives them the seed value. We tend to make two of these at once.
        public int x;
        public int y;
        public Coordinate()
        {
            x = rand.Next(0,4);
            y = rand.Next(0,4);
        }

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(Object obj)
        {
            Coordinate coordObj = obj as Coordinate;
            if (coordObj == null)
            {
                return false;
            }
            return (x == coordObj.x && y == coordObj.y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public String CoordString()
        {
            return ("[" + x + ", " + y + "]");
        }
    }
}
