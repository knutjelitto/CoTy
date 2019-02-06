using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEd
{
    public class Line
    {
        public Line()
        {
        }

        public string Text { get; set; }

        public List<Segment> Segments { get; private set; }
    }
}
