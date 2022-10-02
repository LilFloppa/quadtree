using QuadTree;
using System.Text.Json;

namespace Test
{
    static class Utils
    {
        public static List<Circle>? CirclesFromJsonFile(string path)
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Circle>>(json);
        }

        public static List<Circle> GenerateNonOverlappingCircles(int count, int radius, int width, int height)
        {
            List<Circle> result = new List<Circle>();
            Random random = new Random();
            int maxAttempts = 50;

            for (int i = 0; i < count; i++)
            {
                int attempt = 0;
                while (attempt < maxAttempts)
                {
                    int x = random.Next(0, width);
                    int y = random.Next(0, height);
                    Circle c = new(radius, x, y);

                    bool valid = true;
                    foreach (var circle in result)
                        if (c.Intersects(circle))
                        {
                            valid = false;
                            break;
                        }

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

        static public List<(int x, int y)> GeneratePoints(int width, int height, int pointCount)
        {
            Random random = new Random();
            List<(int x, int y)> points = new List<(int x, int y)>();

            for (int i = 0; i < pointCount; i++)
            {
                double x = random.NextDouble();
                double y = random.NextDouble();

                x = width * x;
                y = height * y;

                points.Add(((int)x, (int)y));
            }

            return points;
        }
    }
}
