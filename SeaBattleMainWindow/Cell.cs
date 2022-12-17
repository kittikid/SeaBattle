using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleMainWindow
{
    public class Cell
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public Cell(int Row, int Col)
        {
            this.Row = Row;
            this.Col = Col;
        }
    }
}
