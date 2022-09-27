using QuadTree;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace UI
{
    public class MyCanvas : Canvas
    {
        Domain domain = null;

        System.Drawing.Color circleColor = System.Drawing.Color.FromArgb(255, 42, 193, 160);

        SolidColorBrush circleBrush = new SolidColorBrush(Color.FromArgb(255, 42, 193, 160));
        SolidColorBrush selectedCircleBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 100));
        SolidColorBrush quadNodeBrush = new SolidColorBrush(Color.FromArgb(255, 132, 155, 153));
        SolidColorBrush selectedQuadNodeBrush = new SolidColorBrush(Color.FromArgb(255, 237, 85, 235));
        Pen quadNodePen = null;
        Pen selectedQuadNodePen = null;

        Circle? selected = null;
        (QuadNode node, Circle circle)? found = null;

        public MyCanvas()
        {
            Background = new SolidColorBrush(Color.FromArgb(255, 34, 51, 51));

            quadNodePen = new Pen(quadNodeBrush, 2);
            selectedQuadNodePen = new Pen(selectedQuadNodeBrush, 2);
        }

        public void SetDomain(Domain domain) => this.domain = domain;

        public void TryAddRandomCircle()
        {
            var circle = domain.TryAddCircle(circleColor);
            InvalidateVisual();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var mousePos = e.GetPosition(this);
            //Circle? circle = domain.Find((int)mousePos.X, (int)mousePos.Y);

            //if (circle != selected)
            //{
            //    selected = circle;
            //    InvalidateVisual();
            //}

            (QuadNode, Circle)? newFound = domain.FindInQuadTree((int)mousePos.X, (int)mousePos.Y);
            if (newFound != found)
            {
                newFound = domain.FindInQuadTree((int)mousePos.X, (int)mousePos.Y);
                found = newFound;
                InvalidateVisual();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.RightButton == MouseButtonState.Pressed)
            {
                var mousePos = e.GetPosition(this);
                domain.TryAddCircle((int)mousePos.X, (int)mousePos.Y, circleColor);
                InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (domain == null)
                return;

            foreach (var circle in domain.Circles)
                dc.DrawEllipse(circleBrush, null, new System.Windows.Point(circle.X, circle.Y), circle.Radius, circle.Radius);

            DrawQuadTree(dc, domain.QuadTree.Root);

            if (selected != null)
                dc.DrawEllipse(selectedCircleBrush, null, new System.Windows.Point(selected.X, selected.Y), selected.Radius, selected.Radius);

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

        private void DrawQuadTree(DrawingContext dc, QuadNode node)
        {
            dc.DrawRectangle(null, quadNodePen, new System.Windows.Rect(node.Area.X, node.Area.Y, node.Area.Width, node.Area.Height));

            foreach (var child in node.Children)
                DrawQuadTree(dc, child);
        }
    }
}
