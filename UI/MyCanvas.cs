using QuadTree;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace UI
{
    public class MyCanvas : Canvas
    {
        Domain? domain = null;

        // Circle colors
        SolidColorBrush circleBrush = new SolidColorBrush(Color.FromArgb(255, 42, 193, 160));
        SolidColorBrush selectedCircleBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 100));
        Pen circlePen = new Pen(new SolidColorBrush(Color.FromArgb(255, 255, 193, 160)), 2);
        // QuadNode colors
        SolidColorBrush quadNodeBrush = new SolidColorBrush(Color.FromArgb(255, 132, 155, 153));
        SolidColorBrush selectedQuadNodeBrush = new SolidColorBrush(Color.FromArgb(255, 237, 85, 235));
        Pen quadNodePen = new Pen(new SolidColorBrush(Color.FromArgb(255, 132, 155, 153)), 2);
        Pen selectedQuadNodePen = new Pen(new SolidColorBrush(Color.FromArgb(255, 237, 85, 235)), 2);

        (QuadNode node, Circle circle)? found = null;

        public MyCanvas()
        {
            Background = new SolidColorBrush(Color.FromArgb(255, 34, 51, 51));

            quadNodePen = new Pen(quadNodeBrush, 2);
            selectedQuadNodePen = new Pen(selectedQuadNodeBrush, 2);
        }

        public void SetDomain(Domain domain) => this.domain = domain;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (domain != null)
            {
                var mousePos = e.GetPosition(this);
                (QuadNode, Circle)? newFound = domain.FindInQuadTree((int)mousePos.X, (int)mousePos.Y);
                if (newFound != found)
                {
                    newFound = domain.FindInQuadTree((int)mousePos.X, (int)mousePos.Y);
                    found = newFound;
                    InvalidateVisual();
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.RightButton == MouseButtonState.Pressed)
            {
                var mousePos = e.GetPosition(this);
                domain?.AddCircle((int)mousePos.X, (int)mousePos.Y);
                InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (domain == null)
                return;

            DrawCircles(dc);
            DrawQuadTree(dc, domain.QuadTree.Root);
            DrawFoundCircleAndNode(dc);
        }

        private void DrawCircles(DrawingContext dc)
        {
            if (domain != null)
                foreach (var circle in domain.Circles)
                    dc.DrawEllipse(circleBrush, circlePen, new System.Windows.Point(circle.X, circle.Y), circle.Radius, circle.Radius);
        }

        private void DrawQuadTree(DrawingContext dc, QuadNode node)
        {
            dc.DrawRectangle(null, quadNodePen, new System.Windows.Rect(node.Area.X, node.Area.Y, node.Area.Width, node.Area.Height));
            foreach (var child in node.Children)
                DrawQuadTree(dc, child);
        }

        private void DrawFoundCircleAndNode(DrawingContext dc)
        {
            if (found != null)
            {
                dc.DrawEllipse(selectedCircleBrush, null, new System.Windows.Point(
                    found.Value.circle.X,
                    found.Value.circle.Y),
                    found.Value.circle.Radius,
                    found.Value.circle.Radius);

                dc.DrawRectangle(null, selectedQuadNodePen, new System.Windows.Rect(
                    found.Value.node.Area.X,
                    found.Value.node.Area.Y,
                    found.Value.node.Area.Width,
                    found.Value.node.Area.Height));
            }
        }
    }
}
