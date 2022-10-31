using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleMainWindow
{
    public class Ship
    {
        public int x, y, lengthShip, count;
        public bool orientation;
        public int X { get; set; }
        public int Y { get; set; }
        public int LengthShip { get; set; }
        public bool Orientation { get; set; }
        public Ship(int x, int y, int lengthShip, bool orientation)
        {
            this.X = x;
            this.Y = y;
            this.LengthShip = lengthShip;
            this.Orientation = orientation;
        }
        public void countShip()
        {
            if (lengthShip == 4)
                count = 1;
            countShips = new List<int>() { 1, 2, 3, 4 };
        }
        public List<int> countShips = new List<int>() { 1, 2, 3, 4 };
        public List<int> lifeShips = new List<int>() { 4, 6, 6, 4 };
    } 
}
