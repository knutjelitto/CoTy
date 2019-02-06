#if false
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CoEd
{
    public class ScrollText : IScrollInfo
    {
        private bool canVerticallyScroll;
        private bool canHorizontallyScroll;
        private double extentWidth;
        private double extentHeight;
        private double viewportWidth;
        private double viewportHeight;
        private double horizontalOffset;
        private double verticalOffset;
        private ScrollViewer scrollOwner;

        public ScrollText()
        {
            this.extentWidth = 500;
            this.extentHeight = 500;
            this.viewportWidth = 50;
            this.viewportHeight = 50;
        }

        public void LineUp()
        {
            throw new NotImplementedException();
        }

        public void LineDown()
        {
            throw new NotImplementedException();
        }

        public void LineLeft()
        {
            throw new NotImplementedException();
        }

        public void LineRight()
        {
            throw new NotImplementedException();
        }

        public void PageUp()
        {
            throw new NotImplementedException();
        }

        public void PageDown()
        {
            throw new NotImplementedException();
        }

        public void PageLeft()
        {
            throw new NotImplementedException();
        }

        public void PageRight()
        {
            throw new NotImplementedException();
        }

        public void MouseWheelUp()
        {
            throw new NotImplementedException();
        }

        public void MouseWheelDown()
        {
            throw new NotImplementedException();
        }

        public void MouseWheelLeft()
        {
            throw new NotImplementedException();
        }

        public void MouseWheelRight()
        {
            throw new NotImplementedException();
        }

        public void SetHorizontalOffset(double offset)
        {
            throw new NotImplementedException();
        }

        public void SetVerticalOffset(double offset)
        {
            throw new NotImplementedException();
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            throw new NotImplementedException();
        }

        public bool CanVerticallyScroll
        {
            get => this.canVerticallyScroll;
            set => this.canVerticallyScroll = value;
        }

        public bool CanHorizontallyScroll
        {
            get => this.canHorizontallyScroll;
            set => this.canHorizontallyScroll = value;
        }

        public double ExtentWidth
        {
            get => this.extentWidth;
            set => this.extentWidth = value;
        }

        public double ExtentHeight
        {
            get => this.extentHeight;
            set => this.extentHeight = value;
        }

        public double ViewportWidth
        {
            get => this.viewportWidth;
            set => this.viewportWidth = value;
        }

        public double ViewportHeight
        {
            get => this.viewportHeight;
            set => this.viewportHeight = value;
        }

        public double HorizontalOffset
        {
            get => this.horizontalOffset;
            set => this.horizontalOffset = value;
        }

        public double VerticalOffset
        {
            get => this.verticalOffset;
            set => this.verticalOffset = value;
        }

        public ScrollViewer ScrollOwner
        {
            get => this.scrollOwner;
            set => this.scrollOwner = value;
        }
    }
}
#endif