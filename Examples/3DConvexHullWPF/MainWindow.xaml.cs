﻿#region
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MIConvexHull;
using Petzold.Media3D;
#endregion

namespace ExampleWithGraphics
{
    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int NumberOfVertices = 1000;
        private const double size = 50;
        private List<IVertexConvHull> convexHullVertices;
        private List<IFaceConvHull> faces;
        private ModelVisual3D modViz;
        private List<IVertexConvHull> vertices;

        public MainWindow()
        {
            InitializeComponent();
            btnMakeSquarePoints_Click(null, null);
        }

        private void ClearAndDrawAxes()
        {
            var init = viewport.Children[0];
            viewport.Children.Clear();
            viewport.Children.Add(init);
            var ax = new Axes { Extent = 60 };
            viewport.Children.Add(ax);
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Running...");
            var now = DateTime.Now;
            faces = new List<IFaceConvHull>();
            var convexHull = new ConvexHull(vertices);
            convexHullVertices = convexHull.FindConvexHull(out faces, typeof(face), 3);
            var interval = DateTime.Now - now;
            txtBlkTimer.Text = interval.Hours + ":" + interval.Minutes
                               + ":" + interval.Seconds + "." + interval.TotalMilliseconds;
            btnDisplay.IsEnabled = true;
            btnDisplay.IsDefault = true;
        }

        private void btnDisplay_Click(object sender, RoutedEventArgs e)
        {
            viewport.Children.Remove(modViz);

            var CVPoints = new Point3DCollection();
            foreach (var chV in convexHullVertices)
            {
                CVPoints.Add(((vertex)chV).Center);
                viewport.Children.Add(new Sphere
                                          {
                                              Center = ((vertex)chV).Center,
                                              BackMaterial = new DiffuseMaterial(Brushes.Orange),
                                              Radius = .5
                                          });
            }


            var faceTris = new Int32Collection();
            foreach (var f in faces)
            {
                var orderImpliedNormal = StarMath.crossProduct3(
                    StarMath.subtract(f.vertices[1].coordinates, f.vertices[0].coordinates, 3),
                    StarMath.subtract(f.vertices[2].coordinates, f.vertices[1].coordinates, 3)
                    );
                if (StarMath.dotProduct(f.normal, orderImpliedNormal, 3) < 0)
                    Array.Reverse(f.vertices);
                faceTris.Add(convexHullVertices.IndexOf(f.vertices[0]));
                faceTris.Add(convexHullVertices.IndexOf(f.vertices[1]));
                faceTris.Add(convexHullVertices.IndexOf(f.vertices[2]));
            }
            var mg3d = new MeshGeometry3D
                           {
                               Positions = CVPoints,
                               TriangleIndices = faceTris
                           };

            var material = new MaterialGroup
                            {
                                Children = new MaterialCollection
                                            {
                                                new DiffuseMaterial(Brushes.Red),
                                                new SpecularMaterial(Brushes.Beige, 2.0)
                                            }
                            };


            var geoMod = new GeometryModel3D
                             {
                                 Geometry = mg3d,
                                 Material = material
                             };

            modViz = new ModelVisual3D { Content = geoMod };
            viewport.Children.Add(modViz);
        }

        private void btnMakeSquarePoints_Click(object sender, RoutedEventArgs e)
        {
            ClearAndDrawAxes();
            vertices = new List<IVertexConvHull>();
            var r = new Random();

            /****** Random Vertices ******/
            for (var i = 0; i < NumberOfVertices; i++)
            {
                var vi = new vertex(size * r.NextDouble() - size / 2, size * r.NextDouble() - size / 2,
                                    size * r.NextDouble() - size / 2);
                vertices.Add(vi);

                viewport.Children.Add(vi);
            }

            //vertices.Add(new vertex(0, 0, 0));
            //vertices.Add(new vertex(10, 0, 0));
            //vertices.Add(new vertex(0, 10, 0));
            //vertices.Add(new vertex(0, 0, 10));
            //vertices.Add(new vertex(10, 10, 0));
            //vertices.Add(new vertex(10, 0, 10));
            //vertices.Add(new vertex(0, 10, 10));
            //vertices.Add(new vertex(10, 10, 10));

            //vertices.Add(new vertex(10, 10, 20));
            //vertices.Add(new vertex(0, 10, 20));
            //vertices.Add(new vertex(10, 0, 20));
            //vertices.Add(new vertex(0, 0, 20));

            //            for (int i = 0; i < 8; i++)
            //            {
            //                for (int j = 0; j < 8; j++)
            //                {
            //                    for (int k = 0; k < 8; k++)
            //                    {
            ////                        vertices.Add(new vertex(5 * i, 0.6 * (i * i + j * j), 5 * j));
            //                        vertices.Add(new vertex(5 * i, 5 * j, 5 * k));
            //                    }
            //                }
            //            }

            //foreach (var item in vertices)
            //{
            //    viewport.Children.Add((vertex)item);
            //}

            btnRun.IsDefault = true;
            btnDisplay.IsEnabled = false;
            txtBlkTimer.Text = "00:00:00.000";
        }

        private void btnMakeCirclePoints_Click(object sender, RoutedEventArgs e)
        {
            ClearAndDrawAxes();
            vertices = new List<IVertexConvHull>();
            var r = new Random();

            /****** Random Vertices ******/
            for (var i = 0; i < NumberOfVertices; i++)
            {
                var radius = size + r.NextDouble();
                // if (i < NumberOfVertices / 2) radius /= 2;
                var theta = 2 * Math.PI * r.NextDouble();
                var azimuth = Math.PI * r.NextDouble();
                var x = radius * Math.Cos(theta) * Math.Sin(azimuth);
                var y = radius * Math.Sin(theta) * Math.Sin(azimuth);
                var z = radius * Math.Cos(azimuth);
                var vi = new vertex(x, y, z);
                vertices.Add(vi);
                /*
                 *          do {
                 x1 = 2.0 * ranf() - 1.0;
                 x2 = 2.0 * ranf() - 1.0;
                 w = x1 * x1 + x2 * x2;
         } while ( w >= 1.0 );

         w = sqrt( (-2.0 * ln( w ) ) / w );
         y1 = x1 * w;
         y2 = x2 * w;

                 */

                viewport.Children.Add(vi);
            }
            btnRun.IsDefault = true;
            btnDisplay.IsEnabled = false;
            txtBlkTimer.Text = "00:00:00.000";
        }

        //private void btnDelau_Click(object sender, RoutedEventArgs e)
        //{
        //    Console.WriteLine("Running...");
        //    var now = DateTime.Now;
        //    //faces = new List<IFaceConvHull>();
        //    var convexHull = new ConvexHull(vertices);
        //    var faces = convexHull.FindDelaunayTriangulation();
        //    var interval = DateTime.Now - now;
        //    txtBlkTimer.Text = interval.Hours + ":" + interval.Minutes
        //                       + ":" + interval.Seconds + "." + interval.TotalMilliseconds;

        //    MessageBox.Show(faces.Count.ToString());
        //    btnDisplay.IsEnabled = true;
        //    btnDisplay.IsDefault = true;
        //}
    }
}