using QuadTree;
using System.Text.Json;
using Test;

int width = 100000;
int height = 100000;
string filepath = "C:/repos/circles.json";


List<Circle> circles = null;

if (!File.Exists(filepath))
{
    circles = Utils.GenerateNonOverlappingCircles(20000, 80, width, height);
    string json = JsonSerializer.Serialize(circles);
    File.WriteAllText(filepath, json);
}
else
{
    circles = Utils.CirclesFromJsonFile(filepath);
}

Console.WriteLine($"Circles Count: {circles.Count}");
Domain domain = new Domain(width, height);

foreach (var circle in circles)
    domain.AddCircle(circle);

double ms = TestCases.TestLinearSearch(domain, 100);
ms = TestCases.TestQuadTreeSearch(domain, 100);
