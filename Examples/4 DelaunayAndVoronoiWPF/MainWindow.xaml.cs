﻿#region

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using MIConvexHull;

#endregion

namespace ExampleWithGraphics
{
    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int NumberOfVertices = 1000;
        private double size;
        private List<IFaceConvHull> faces;
        private List<IVertexConvHull> vertices;
        private List<Tuple<IVertexConvHull, IVertexConvHull>> edges;
        private ConvexHull convexHull;

        public MainWindow()
        {
            InitializeComponent();
            // btnMakePoints_Click(null, null);
        }


        private void btnMakePoints_Click(object sender, RoutedEventArgs e)
        {
            drawingCanvas.Children.Clear();
            size = Math.Min(drawingCanvas.Height, drawingCanvas.Width);
            vertices = new List<IVertexConvHull>();
            var r = new Random();

            /****** Random Vertices ******/
            for (var i = 0; i < NumberOfVertices; i++)
            {
                var vi = new vertex(size * r.NextDouble(), size * r.NextDouble());
                vertices.Add(vi);

                drawingCanvas.Children.Add(vi);
            }
            btnDisplayDelaunay.IsDefault = true;
            btnDisplayDelaunay.IsEnabled = false;
            txtBlkTimer.Text = "00:00:00.000";
            convexHull = new ConvexHull(vertices);
        }

        private void btnFindDelaunay_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Running...");
            var now = DateTime.Now;
            faces = convexHull.FindDelaunayTriangulation(typeof(face));
            var interval = DateTime.Now - now;
            txtBlkTimer.Text = interval.Hours + ":" + interval.Minutes
                               + ":" + interval.Seconds + "." + interval.TotalMilliseconds;
            btnDisplayDelaunay.IsEnabled = true;
            btnDisplayDelaunay.IsDefault = true;
        }

        private void btnDisplayDelaunay_Click(object sender, RoutedEventArgs e)
        {
            foreach (var f in faces)
                drawingCanvas.Children.Add((UIElement)f);
        }

        private void btnFindVoronoi_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Running...");
            var now = DateTime.Now;
            List<IVertexConvHull> nodes;
            convexHull.FindVoronoiGraph(out nodes, out edges);
            var interval = DateTime.Now - now;
            txtBlkTimer.Text = interval.Hours + ":" + interval.Minutes
                               + ":" + interval.Seconds + "." + interval.TotalMilliseconds;
            btnDisplayVoronoi.IsEnabled = true;
            btnDisplayVoronoi.IsDefault = true;

        }

        private void btnDisplayVoronoi_Click(object sender, RoutedEventArgs e)
        {
            foreach (var edge in edges)
                drawingCanvas.Children.Add(
                    new Line
                        {
                            X1 = edge.Item1.coordinates[0],
                            Y1 = edge.Item1.coordinates[1],
                            X2 = edge.Item2.coordinates[0],
                            Y2 = edge.Item2.coordinates[1],
                            Stroke = Brushes.Red
                        });
        }
    }
}