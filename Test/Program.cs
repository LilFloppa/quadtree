using QuadTree;
using System.Text.Json;
using Test;

System.Globalization.CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture.Clone() as System.Globalization.CultureInfo ?? throw new InvalidCastException();
culture.NumberFormat = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
System.Threading.Thread.CurrentThread.CurrentCulture = culture;
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;

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

List<int> counts = new(){ 1, 10, 100, 1000, 10000, 100000 };

Console.WriteLine("Linear Search");
Console.WriteLine("Point Count\tms");
foreach (var count in counts)
{
    double ms = TestCases.TestLinearSearch(domain, count);
    Console.WriteLine($"{count,11};\t{ms,2}");
}

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("Quad Tree Search");
Console.WriteLine("Point Count\tms");
foreach (var count in counts)
{
    double ms = TestCases.TestQuadTreeSearch(domain, count);
    Console.WriteLine($"{count,11};\t{ms,2}");

}
