﻿/*************************************************************************
 *     This file & class is part of the MIConvexHull Library Project. 
 *     Copyright 2010 Matthew Ira Campbell, PhD.
 *
 *     MIConvexHull is free software: you can redistribute it and/or modify
 *     it under the terms of the GNU General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 *  
 *     MIConvexHull is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU General Public License for more details.
 *  
 *     You should have received a copy of the GNU General Public License
 *     along with MIConvexHull.  If not, see <http://www.gnu.org/licenses/>.
 *     
 *     Please find further details and contact information on GraphSynth
 *     at http://miconvexhull.codeplex.com
 *************************************************************************/

/*
 * Note: this will take 55-100 minutes to complete!
 * It will create around 14 thousand voronoi points, and 26 thousand voronoi
 * edges.
 * Why is this even included?! The idea was in path-planning where you have 
 * to maneuver in full 3-D space, and you have 6 variables: x, y, z, and rotations
 * about x, y, and z. The resulting graph could be used by a path-planning search process.
 */
using System.Threading;
using System.Windows.Threading;

namespace TestEXE_for_MIConvexHull_Voronoi
{
    using System;
    using System.Collections.Generic;
    using MIConvexHull;


    static class Program
    {
        static void Main()
        {
            statusTimer.Tick += statusTimerTimer_Tick;
            const int NumberOfVertices = 500;
            const double size = 1000;
            const int dimension = 6;

            var r = new Random();
            Console.WriteLine("Ready? Push Return/Enter to start.");
            Console.ReadLine();

            Console.WriteLine("Making " + NumberOfVertices + " random vertices.");
            var vertices = new List<vertex>();
            for (var i = 0; i < NumberOfVertices; i++)
            {
                var location = new double[dimension];
                for (var j = 0; j < dimension; j++)
                    location[j] = size * r.NextDouble();
                vertices.Add(new vertex(location));
            }
            Console.WriteLine("Running...");
            var now = DateTime.Now;
            convexHull = new ConvexHull(vertices);
            List<IVertexConvHull> vnodes;
            List<Tuple<IVertexConvHull, IVertexConvHull>> vedges;
            statusTimer.Start();
            convexHull.FindVoronoiGraph(out vnodes, out vedges, typeof (vertex));
            //var interval = DateTime.Now - now;
            //statusTimer.Stop();
            //Console.WriteLine("Out of the " + NumberOfVertices + " vertices, there are " +
            //    vnodes.Count + " voronoi points and " + vedges.Count + " voronoi edges.");
            //Console.WriteLine("time = " + interval);
            //Console.ReadLine();
        }

        private static ConvexHull convexHull;
        static readonly DispatcherTimer statusTimer = new DispatcherTimer
              {
                  Interval = new TimeSpan(50000000),
                  IsEnabled = true
              };

        static void statusTimerTimer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(convexHull.Status);
        }
    }
}

