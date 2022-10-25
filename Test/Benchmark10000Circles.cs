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
    public class Benchmark10000Circles
    {
        string Filepath = "Z:/circles.json";
        int Width = 100000;
        int Height = 100000;
        int CircleCount = 10000;
        int PointCount = 100000;
        List<Circle> Circles;
        List<(int x, int y)> points;
        Domain domain;
        public Benchmark10000Circles()
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


            domain = new Domain((int)Math.Sqrt(CircleCount), Width, Height);
            for (int i = 0; i < CircleCount; i++)
            {
                domain.AddCircleToList(Circles[i]);
                domain.AddCircleToQuadTree(Circles[i]);
            }

            points = Utils.GeneratePoints(Width, Height, PointCount);
        }

        [Benchmark]
        public void Linear10()
        {
            for (int i = 0; i < 10; i++)
                domain.FindInList(points[i].x, points[i].y);
        }

        [Benchmark]
        public void Linear100()
        {
            for (int i = 0; i < 100; i++)
                domain.FindInList(points[i].x, points[i].y);
        }

        [Benchmark]
        public void Linear1000()
        {
            for (int i = 0; i < 1000; i++)
                domain.FindInList(points[i].x, points[i].y);
        }

        [Benchmark]
        public void Linear10000()
        {
            for (int i = 0; i < 10000; i++)
                domain.FindInList(points[i].x, points[i].y);
        }

        [Benchmark]
        public void Quad10()
        {
            for (int i = 0; i < 10; i++)
                domain.FindInQuadTree(points[i].x, points[i].y);
        }

        [Benchmark]
        public void Quad100()
        {
            for (int i = 0; i < 100; i++)
                domain.FindInQuadTree(points[i].x, points[i].y);
        }

        [Benchmark]
        public void Quad1000()
        {
            for (int i = 0; i < 1000; i++)
                domain.FindInQuadTree(points[i].x, points[i].y);
        }

        [Benchmark]
        public void Quad10000()
        {
            for (int i = 0; i < 10000; i++)
                domain.FindInQuadTree(points[i].x, points[i].y);
        }
    }
}
