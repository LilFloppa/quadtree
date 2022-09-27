using System.Drawing;

namespace QuadTree
{
    public class Circle
    {
        public int Radius { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Circle(int radius, int x, int y)
        {
            Radius = radius;
            X = x;
            Y = y;
        }

        public bool Intersects(Circle other)
        {
            int distance = (other.X - X) * (other.X - X) + (other.Y - Y) * (other.Y - Y);
            return distance < (Radius + other.Radius) * (Radius + other.Radius);
        }

        public bool ContainsPoint(int x, int y)
        {
            int distance = (X - x) * (X - x) + (Y - y) * (Y - y);
            return distance < Radius * Radius;
        }

        public bool CenterInsideRectangle(Rectangle r)
        {
            return X >= r.Left && X <= r.Right && Y >= r.Top && Y <= r.Bottom;
        }

        public bool ContainsRectangle(Rectangle r)
        {
            return
                ContainsPoint(r.Top, r.Left) &&
                ContainsPoint(r.Top, r.Right) &&
                ContainsPoint(r.Bottom, r.Left) &&
                ContainsPoint(r.Bottom, r.Left);
        }

        public bool IntersectsRectangle(Rectangle r)
        {
            // TODO: implement
            // https://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection#:~:text=There%20are%20only%20two%20cases%20when%20the%20circle%20intersects%20with%20the%20rectangle%3A
            throw new NotImplementedException();
        }
    }

    public class Domain
    {
        public QuadTree QuadTree { get; }
        public List<Circle> Circles { get; } = new();
        public int Height { get; set; }
        public int Width { get; set; }

        Random random = new();

        public Domain(int width, int height)
        {
            Width = width;
            Height = height;
            QuadTree = new(new Rectangle { Height = height, Width = width, X = 0, Y = 0 });
        }

        public Circle AddCircle(int x, int y)
        {
            int radius = random.Next(10, 30);
            Circle circle = new Circle(radius, x, y);

            Circles.Add(circle);
            QuadTree.AddCircle(circle);
            return circle;
        }

        public Circle? FindInList(int x, int y)
        {
            foreach (var circle in Circles)
                if (circle.ContainsPoint(x, y))
                    return circle;

            return null;
        }

        public (QuadNode, Circle)? FindInQuadTree(int x, int y) => QuadTree.FindCircle(x, y);
    }
}