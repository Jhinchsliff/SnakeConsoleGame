using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeClasses
{
    public class Vector
    {       
        //properties
        public int X { get; set; }
        public int Y { get; set; }
        //constructors
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }//FQCTOR

        //Methods
        public Vector() : base() { }
    }
}
