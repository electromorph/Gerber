using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;

namespace Visualization
{
    public class SimpleCubeGenerator
    {
        // MeshGeometry property generates MeshGeometry3D
        public MeshGeometry3D GenerateMeshGeometry(double x, double y, double z)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            Point3DCollection points = new Point3DCollection(20);
            points.Add(new Point3D(-x, y, z));
            points.Add(new Point3D(x, y, z));
            points.Add(new Point3D(x, y, -z));
            points.Add(new Point3D(-x, y, -z));
            points.Add(new Point3D(-x, -y, z));
            points.Add(new Point3D(x, -y, z));
            points.Add(new Point3D(x, -y, -z));
            points.Add(new Point3D(-x, -y, -z));
            mesh.Positions = points;
            mesh.TriangleIndices = ((Int32Collection)new Int32CollectionConverter().ConvertFromString("0,1,2, 0,2,3, 0,4,1, 4,5,1, 1,5,2, 5,6,2, 2,6,3, 6,7,3, 3,7,0, 7,4,0, 6,5,4, 7,6,4"));
            mesh.TextureCoordinates = ((PointCollection)new PointCollectionConverter().ConvertFromString("0,0, 0,1, 1,1, 1,0"));
            mesh.Freeze();
            return mesh;
        }
        
        public GeometryModel3D Model
        {
            get
            {
                GeometryModel3D gm3d = new GeometryModel3D();
                gm3d.Geometry = GenerateMeshGeometry(6,0.25,6);

                //Define materials
                MaterialGroup mg1 = new MaterialGroup();

                //Linear Gradient Brush
                LinearGradientBrush lgb = new LinearGradientBrush(Colors.Blue, Colors.Orange, new Point(0, 0), new Point(1, 1));
                lgb.Opacity = 0.2;

                //Solid Color Brush
                SolidColorBrush scb = new SolidColorBrush(Colors.Red);
                lgb.Opacity = 0.2;
                
                //Image Brush
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(@"c:\pcb.png");
                bi.EndInit();
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = bi;
                ib.Stretch = Stretch.Fill;
                ib.Opacity = 0.6;

                DiffuseMaterial diffuseMaterial = new DiffuseMaterial(ib);
                SpecularMaterial specularMaterial = new SpecularMaterial(ib, 24);
                mg1.Children.Add(diffuseMaterial);
                mg1.Children.Add(specularMaterial);
                gm3d.BackMaterial = mg1;
                gm3d.Material = mg1;
                return gm3d;
            }
        }
    }
}
