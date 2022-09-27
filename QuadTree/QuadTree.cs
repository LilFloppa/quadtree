using System.Drawing;

namespace QuadTree
{
    public class QuadNode
    {
        public Rectangle Area { get; set; }
        public List<Circle> Circles { get; set; } = new();
        public List<QuadNode> Children { get; set; } = new();
        public bool IsLeaf => Children.Count == 0;

        public QuadNode(Rectangle area) => Area = area;

        public void AddCircle(Circle circle)
        {
            if (Circles.Count < QuadTree.K && IsLeaf)
            {
                Circles.Add(circle);
                return;
            }

            if (IsLeaf)
            {
                SplitArea();
                DistributeCirclesAmongChildren();
                Circles.Clear();
            }

            if (circle.ContainsRectangle(Area))
            {
                Circles.Add(circle);
                return;
            }

            foreach (var child in Children)
            {
                if (circle.CenterInsideRectangle(child.Area))
                {
                    child.AddCircle(circle);
                    return;
                }
            }
        }

        private void SplitArea()
        {
            int childWidth = Area.Width / 2;
            int childHeight = Area.Height / 2;
            var tlArea = new Rectangle(Area.X, Area.Y, childWidth, childHeight);
            var blArea = new Rectangle(Area.X, Area.Y + childHeight, childWidth, childHeight);
            var trArea = new Rectangle(Area.X + childWidth, Area.Y, childWidth, childHeight);
            var brArea = new Rectangle(Area.X + childWidth, Area.Y + childHeight, childWidth, childHeight);

            Children.Add(new QuadNode(tlArea));
            Children.Add(new QuadNode(blArea));
            Children.Add(new QuadNode(trArea));
            Children.Add(new QuadNode(brArea));
        }

        private void DistributeCirclesAmongChildren()
        {
            foreach (var circle in Circles)
            {
                foreach (var child in Children)
                {
                    if (circle.CenterInsideRectangle(child.Area))
                    {
                        child.AddCircle(circle);
                        break;
                    }
                }
            }
        }
    }

    public class QuadTree
    {
        static public int K = 3;

        public QuadNode Root { get; set; }

        public QuadTree(Rectangle area) => Root = new(area);

        public void AddCircle(Circle circle) => Root.AddCircle(circle);

        public (QuadNode, Circle)? FindCircle(int x, int y) => FindCircle(Root, x, y);

        private (QuadNode, Circle)? FindCircle(QuadNode node, int x, int y)
        {
            foreach (var circle in node.Circles)
            {
                if (circle.ContainsPoint(x, y))
                    return (node, circle);
            }

            if (node.IsLeaf)
                return null;

            foreach (var child in node.Children)
            {
                var pair = FindCircle(child, x, y);
                if (pair != null)
                    return pair;
            }

            return null;
        }
    }
}
