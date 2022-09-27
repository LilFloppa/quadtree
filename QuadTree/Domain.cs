using System.Drawing;

namespace QuadTree
{
    public class Circle
    {
        public int Radius { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Color Color { get; set; }

        public Circle(int radius, int x, int y, Color color)
        {
            Radius = radius;
            X = x;
            Y = y;
            Color = color;
        }

        public bool Intersect(Circle other)
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
    }

    public class Domain
    {
        public QuadTree QuadTree { get; }
        public List<Circle> Circles { get; } = new();
        public int Height { get; set; }
        public int Width { get; set; }

        Random random = new();

        int generateCircleAttempts = 1;

        public Domain(int width, int height)
        {
            Width = width;
            Height = height;
            QuadTree = new(new Rectangle { Height = height, Width = width, X = 0, Y = 0 });
        }

        public Circle? TryAddCircle(Color color)
        {
            int attempts = 0;
            while (attempts < generateCircleAttempts)
            {
                int radius = random.Next(5, 70);
                int x = random.Next(radius, Width - radius);
                int y = random.Next(radius, Height - radius);
                Circle circle = new Circle(radius, x, y, color);
                bool valid = true;
                foreach (var c in Circles)
                    if (c.Intersect(circle))
                        valid = false;

                if (valid)
                {
                    Circles.Add(circle);
                    QuadTree.AddCircle(circle);
                    return circle;
                }
            }

            return null;
        }

        public Circle? TryAddCircle(int x, int y, Color color)
        {
            int attempts = 0;
            while (attempts < generateCircleAttempts)
            {
                int radius = random.Next(10, 30);
                Circle circle = new Circle(radius, x, y, color);
                bool valid = true;
                foreach (var c in Circles)
                    if (c.Intersect(circle))
                        valid = false;

                if (valid)
                {
                    Circles.Add(circle);
                    QuadTree.AddCircle(circle);
                    return circle;
                }
            }

            return null;
        }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Circle? Find(int x, int y)
        {
            foreach (var circle in Circles)
                if (circle.ContainsPoint(x, y))
                    return circle;

            return null;
        }

        public (QuadNode, Circle)? FindInQuadTree(int x, int y) => QuadTree.FindCircle(x, y);
    }
}