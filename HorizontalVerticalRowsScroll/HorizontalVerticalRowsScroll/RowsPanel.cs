using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace HorizontalVerticalRowsScroll
{
    public class RowsPanel : Panel, IScrollInfo
    {
        public ColumnsCollection Columns { get; set; }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
        {
            if (this.Columns == null)
            {
                EnsureColumns(this);
                double extent = 0;

                foreach (var item in this.Columns)
                {
                    extent += item.Width;
                }
                this.extentWidth = extent;
            }
            foreach (var item in this.Children)
            {
                Row row = item as Row;
                row.Measure(new Size(this.extentWidth, double.PositiveInfinity));
            }
            return base.MeasureOverride(availableSize);
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize)
        {
            ArrangeColumns(finalSize, this.horizontalOffset);
            double yOffset = 0;
            foreach (var item in this.Children)
            {
                Row row = item as Row;
                row.Arrange(new Rect(new Point(0, yOffset), row.DesiredSize));
                yOffset += row.DesiredSize.Height;
            }
            return finalSize;
        }

        private void ArrangeColumns(System.Windows.Size finalSize, double p)
        {
            
        }

        private void EnsureColumns(FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
                throw new InvalidOperationException("RowsPanel must be set as ItemsPanel to the DataGrid object.");

            FrameworkElement templatedParent = frameworkElement.TemplatedParent as FrameworkElement;
            if (templatedParent == null)
                throw new InvalidOperationException("RowsPanel must be set as ItemsPanel to the DataGrid object.");

            if (templatedParent is DataGrid)
            {
                this.Columns = ((DataGrid)templatedParent).Columns;
                return;
            }

            this.EnsureColumns(templatedParent);

        }

        #region IScrollInfo Members

        bool canHorizontallyScroll;
        public bool CanHorizontallyScroll
        {
            get
            {
                return this.canHorizontallyScroll;
            }
            set
            {
                this.canHorizontallyScroll = value;
            }
        }

        bool canVerticallyScroll;
        public bool CanVerticallyScroll
        {
            get
            {
                return this.canVerticallyScroll;
            }
            set
            {
                this.canVerticallyScroll = value;
            }
        }

        public double ExtentHeight
        {
            get { throw new NotImplementedException(); }
        }

        double extentWidth;
        public double ExtentWidth
        {
            get { throw new NotImplementedException(); }
        }
        double horizontalOffset;
        public double HorizontalOffset
        {
            get { throw new NotImplementedException(); }
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

        public void LineUp()
        {
            throw new NotImplementedException();
        }

        public Rect MakeVisible(System.Windows.Media.Visual visual, Rect rectangle)
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

        public void MouseWheelUp()
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

        public void PageUp()
        {
            throw new NotImplementedException();
        }

        ScrollViewer scrollOwner;
        public ScrollViewer ScrollOwner
        {
            get
            {
                return this.scrollOwner;
            }
            set
            {
                this.scrollOwner = value;
                double d =this.scrollOwner.ScrollableHeight;
            }
        }

        public void SetHorizontalOffset(double offset)
        {
            throw new NotImplementedException();
        }

        public void SetVerticalOffset(double offset)
        {
            throw new NotImplementedException();
        }

        public double VerticalOffset
        {
            get { throw new NotImplementedException(); }
        }

        public double ViewportHeight
        {
            get { throw new NotImplementedException(); }
        }

        public double ViewportWidth
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
