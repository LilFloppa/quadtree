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
            UInt64 uint_x = (UInt64)x;
            UInt64 uint_y = (UInt64)y;

            UInt64 uint_X = (UInt64)X;
            UInt64 uint_Y = (UInt64)Y;

            UInt64 distance = (uint_X - uint_x) * (uint_X - uint_x) + (uint_Y - uint_y) * (uint_Y - uint_y);
            return distance < (UInt64)Radius * (UInt64)Radius;
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
            // https://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection#:~:text=There%20are%20only%20two%20cases%20when%20the%20circle%20intersects%20with%20the%20rectangle%3A

            if (ContainsRectangle(r))
                return true;

            bool intersectsEdges = IntersectsLine(r.Left, r.Top, r.Right, r.Top)
                || IntersectsLine(r.Left, r.Top, r.Left, r.Bottom)
                || IntersectsLine(r.Right, r.Top, r.Right, r.Bottom)
                || IntersectsLine(r.Left, r.Bottom, r.Right, r.Bottom);

            if (!intersectsEdges && CenterInsideRectangle(r))
                return true;

            return intersectsEdges;
        }

        public bool IntersectsLine(double Ax, double Ay, double Bx, double By)
        {
            if (ContainsPoint((int)Ax, (int)Ay) || ContainsPoint((int)Bx, (int)By))
            {
                bool a = ContainsPoint((int)Ax, (int)Ay) || ContainsPoint((int)Bx, (int)By);
                return true;
            }

            double Cx = (double)X;
            double Cy = (double)Y;
            double t = ((Cx - Ax) * (Bx - Ax) + (Cy - Ay) * (By - Ay)) / ((Bx - Ax) * (Bx - Ax) + (By - Ay) * (By - Ay));

            if (t >= 0 && t <= 1)
            {
                double Dx = Ax + t * (Bx - Ax);
                double Dy = Ay + t * (By - Ay);

                if ((Dx - Cx) * (Dx - Cx) + (Dy - Cy) * (Dy - Cy) <= Radius * Radius)
                    return true;
            }

            return false;
        }
    }

    public class Domain
    {
        public QuadTree QuadTree { get; }
        public List<Circle> Circles { get; } = new();
        public int Height { get; set; }
        public int Width { get; set; }

        Random random = new();

        public Domain(int K, int width, int height)
        {
            Width = width;
            Height = height;
            QuadTree = new(K, new Rectangle { Height = height, Width = width, X = 0, Y = 0 });
        }

        public Circle AddCircleWithRandomRadius(int x, int y)
        {
            int radius = random.Next(10, 30);
            Circle circle = new Circle(radius, x, y);

            Circles.Add(circle);
            QuadTree.AddCircle(circle);
            return circle;
        }

        public Circle AddCircleToList(Circle circle)
        {
            Circles.Add(circle);
            return circle;
        }

        public Circle AddCircleToQuadTree(Circle circle)
        {
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