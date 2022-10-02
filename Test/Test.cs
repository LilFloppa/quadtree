using QuadTree;
using System.Diagnostics;

namespace Test
{
    static class TestCases
    {
        static public double TestLinearSearch(Domain domain, int pointCount)
        {
            var points = Utils.GeneratePoints(domain.Width, domain.Height, pointCount);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Circle> result = new();
            foreach (var point in points)
            {
                var circle = domain.FindInList(point.x, point.y);
                if (circle != null)
                    result.Add(circle);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds;
        }

        static public double TestQuadTreeSearch(Domain domain, int pointCount)
        {
            var points = Utils.GeneratePoints(domain.Width, domain.Height, pointCount);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Circle> result = new();
            foreach (var point in points)
            {
                var circle = domain.FindInQuadTree(point.x, point.y);
                if (circle != null)
                    result.Add(circle.Value.Item2);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds;
        }

        static private List<Circle> LinearSearchCircles(Domain domain, List<(int x, int y)> points)
        {
            List<Circle> found = new();
            foreach (var point in points)
            {
                var circle = domain.FindInList(point.x, point.y);
                if (circle != null)
                    found.Add(circle);
            }
            return found;
        }

        static private List<Circle> QuadTreeSearchCircles(Domain domain, List<(int x, int y)> points)
        {
            List<Circle> found = new();
            foreach (var point in points)
            {
                var circle = domain.FindInQuadTree(point.x, point.y);
                if (circle != null)
                    found.Add(circle.Value.Item2);
            }

            return found;
        }

        static public void TestFoundCirclesAreTheSame()
        {
            int circleCount = 10000;
            int pointCount = 10000;
            int radius = 50;
            int width = 100000;
            int height = 100000;
            List<Circle> circles = Utils.GenerateNonOverlappingCircles(circleCount, radius, width, height);
            Console.WriteLine($"Circles Count: {circles.Count}");

            Domain domain = new Domain(width, height);

            foreach (var circle in circles)
                domain.AddCircle(circle);

            var points = Utils.GeneratePoints(width, height, pointCount);
            List<Circle> quad = QuadTreeSearchCircles(domain, points);
            List<Circle> linear = LinearSearchCircles(domain, points);

            if (quad.Count != linear.Count)
            {
                Console.WriteLine("FAIL");
                return;
            }

            for (int i = 0; i < linear.Count; i++)
                if (quad[i].X != linear[i].X || quad[i].Y != linear[i].Y)
                {
                    Console.WriteLine("FAIL");
                    return;
                }
        }
    }
}
