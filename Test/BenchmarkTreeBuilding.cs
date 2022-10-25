using BenchmarkDotNet.Attributes;
using QuadTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Test
{
    public class BenchmarkTreeBuilding
    {
        string Filepath = "Z:/circles.json";
        int Width = 100000;
        int Height = 100000;
        List<Circle> Circles;

        public BenchmarkTreeBuilding()
        {
            if (!File.Exists(Filepath))
            {
                Circles = Utils.GenerateNonOverlappingCircles(20000, 80, Width, Height);
                string json = JsonSerializer.Serialize(Circles);
                File.WriteAllText(Filepath, json);
            }
            else
            {
                Circles = Utils.CirclesFromJsonFile(Filepath);
            }
        }

        [Benchmark]
        public void Circles10()
        {
            int n = 10;
            int i = 0;
            Domain domain = new Domain((int)Math.Sqrt(n), Width, Height);

            foreach (var circle in Circles)
            {
                domain.AddCircleToQuadTree(circle);
                i++;

                if (i > n)
                    break;
            }
        }

        [Benchmark]
        public void Circles100()
        {
            int n = 100;
            int i = 0;
            Domain domain = new Domain((int)Math.Sqrt(n), Width, Height);
            foreach (var circle in Circles)
            {
                domain.AddCircleToQuadTree(circle);
                i++;

                if (i > n)
                    break;
            }
        }

        [Benchmark]
        public void Circles1000()
        {
            int n = 1000;
            int i = 0;
            Domain domain = new Domain((int)Math.Sqrt(n), Width, Height);
            foreach (var circle in Circles)
            {
                domain.AddCircleToQuadTree(circle);
                i++;

                if (i > n)
                    break;
            }
        }

        [Benchmark]
        public void Circles10000()
        {
            int n = 10000;
            int i = 0;
            Domain domain = new Domain((int)Math.Sqrt(n), Width, Height);
            foreach (var circle in Circles)
            {
                domain.AddCircleToQuadTree(circle);
                i++;

                if (i > n)
                    break;
            }
        }
    }
}
