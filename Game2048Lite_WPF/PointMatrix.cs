using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2048Lite_WPF
{
    public class PointMatrix
    {
        public int i { get; set; }
        public int j { get; set; }
        public PointMatrix(int i, int j)
        {
            this.i = i;
            this.j = j;
        }
    }
}
