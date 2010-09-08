﻿using System;
using MIConvexHullPluginNameSpace;

namespace TestEXE_for_MIConvexHull_No_Interfaces
{
    class Program
    {
        static void Main()
        {
            const int NumberOfVertices = 1000000;
            const double size = 1000;
            const int dimension = 3;

            var r = new Random();
            Console.WriteLine("Ready? Push Return/Enter to start.");
            Console.ReadLine();

            Console.WriteLine("Making " + NumberOfVertices + " random vertices.");
            /* our inputs are simply in the form of an array of double arrays */
            var vertices = new double[NumberOfVertices][];
            for (var i = 0; i < NumberOfVertices; i++)
            {
                var location = new double[dimension];
                for (var j = 0; j < dimension; j++)
                    location[j] = size * r.NextDouble();
                vertices[i] = location;
            }
            Console.WriteLine("Running...");
            var now = DateTime.Now;
            ConvexHull.InputVertices(vertices);
            double[][] CVpoints = ConvexHull.FindConvexHull_AsDoubleArray();
            var interval = DateTime.Now - now;
            Console.WriteLine("Out of the " + NumberOfVertices + " vertices, there are " +
                CVpoints.GetLength(0) + " in the convex hull.");
            Console.WriteLine("time = " + interval);
            Console.ReadLine();
        }
    }
}