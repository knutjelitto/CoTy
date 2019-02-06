using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace CoEd
{
    public class Caret : ReactiveObject
    {
        private double height;
        private double width;
        private int row;
        private int col;

        public Caret()
        {
            Height = 22;
            Width = 2.5;
            Row = 0;
            Col = 0;
        }

        public double Height
        {
            get => this.height;
            set => this.RaiseAndSetIfChanged(ref this.height, value);
        }

        public double Width
        {
            get => this.width;
            set => this.RaiseAndSetIfChanged(ref this.width, value);
        }

        public int Row
        {
            get => this.row;
            set => this.RaiseAndSetIfChanged(ref this.row, value);
        }

        public int Col
        {
            get => this.col;
            set => this.RaiseAndSetIfChanged(ref this.col, value);
        }
    }
}
