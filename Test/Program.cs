using QuadTree;
using System.Diagnostics;

int radius = 1;
int width = 100000;
int height = 100000;
int circlesCount = 100;

List<Circle> GenerateNonOverlappingCircles(int count, int width, int height)
{
    List<Circle> result = new List<Circle>();
    int maxAttempts = 50;

    for (int i = 0; i < count; i++)
    {
        int attempt = 0;
        while (attempt < maxAttempts)
        {
            int x = Random.Shared.Next(0, width);
            int y = Random.Shared.Next(0, height);
            Circle c = new(radius, x, y);

            bool valid = true;
            foreach (var circle in result)
                if (c.Intersects(circle))
                    valid = false;

            if (valid)
            {
                result.Add(c);
                break;
            }
            attempt++;
        }
    }

    return result;
}



List<Circle> circles = GenerateNonOverlappingCircles(circlesCount, width, height);
Domain domain = new Domain(width, height);

foreach (var circle in circles)
    domain.AddCircle(circle);

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();
domain.FindInList(100, 100);
stopwatch.Stop();
Console.WriteLine($"Linear Search Time: {stopwatch.Elapsed.TotalMilliseconds}ms");

stopwatch.Reset();
stopwatch.Start();
domain.FindInQuadTree(100, 100);
stopwatch.Stop();
Console.WriteLine($"QuadTree Search Time: {stopwatch.Elapsed.TotalMilliseconds}ms");
