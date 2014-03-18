using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visualization
{
    public class SimpleCylinderGenerator
    {
        // Private field and public property
        int slices = 20;

        public int Slices
        {
            set { slices = value; }
            get { return slices; }
        }

        // MeshGeometry property generates MeshGeometry3D
        public MeshGeometry3D MeshGeometry
        {
            get
            {
                MeshGeometry3D mesh = new MeshGeometry3D();

                for (int i = 0; i < Slices; i++)
                {
                    // Points around top of cylinder
                    double x = Math.Sin(2 * i * Math.PI / Slices);
                    double z = Math.Cos(2 * i * Math.PI / Slices);
                    mesh.Positions.Add(new Point3D(x, 1, z));

                    // Points around bottom of cylinder
                    x = Math.Sin((2 * i + 1) * Math.PI / Slices);
                    z = Math.Cos((2 * i + 1) * Math.PI / Slices);
                    mesh.Positions.Add(new Point3D(x, 0, z));
                }
                // Points at center of top and bottom
                mesh.Positions.Add(new Point3D(0, 1, 0));
                mesh.Positions.Add(new Point3D(0, 0, 0));

                for (int i = 0; i < Slices; i++)
                {
                    // Triangles along length of cylinder
                    mesh.TriangleIndices.Add(2 * i + 0);
                    mesh.TriangleIndices.Add(2 * i + 1);
                    mesh.TriangleIndices.Add((2 * i + 2) % (2 * Slices));

                    mesh.TriangleIndices.Add((2 * i + 2) % (2 * Slices));
                    mesh.TriangleIndices.Add(2 * i + 1);
                    mesh.TriangleIndices.Add((2 * i + 3) % (2 * Slices));

                    // Triangles on top
                    mesh.TriangleIndices.Add(2 * Slices);
                    mesh.TriangleIndices.Add(2 * i + 0);
                    mesh.TriangleIndices.Add((2 * i + 2) % (2 * Slices));

                    // Triangles on bottom
                    mesh.TriangleIndices.Add(2 * Slices + 1);
                    mesh.TriangleIndices.Add((2 * i + 3) % (2 * Slices));
                    mesh.TriangleIndices.Add(2 * i + 1);
                }
                // We know this object won't be changed, so freeze it
                mesh.Freeze();
                return mesh;
            }
        }

        public GeometryModel3D Model
        {
            get
            {
                GeometryModel3D gm3d = new GeometryModel3D();
                gm3d.Geometry = this.MeshGeometry;
                
                //Define materials
                MaterialGroup mg1 = new MaterialGroup();
                
                //Linear Gradient Brush
                LinearGradientBrush lgb = new LinearGradientBrush();
                lgb.GradientStops.Add(new GradientStop(Colors.Blue, 0));
                lgb.GradientStops.Add(new GradientStop(Colors.Orange, 0.2));
                lgb.GradientStops.Add(new GradientStop(Colors.Orange, 0.8));
                lgb.GradientStops.Add(new GradientStop(Colors.Blue, 1));
                lgb.Opacity = 0.6;
                
                //Solid Color Brush
                SolidColorBrush scb = new SolidColorBrush(Colors.Red);
                
                //Image Brush
                ImageBrush ib = new ImageBrush(new BitmapImage(new Uri(@"c:\blob.jpg")));
                ib.Stretch = Stretch.Fill;
                ib.Opacity = 0.6;

                DiffuseMaterial diffuseMaterial = new DiffuseMaterial(scb);
                SpecularMaterial specularMaterial = new SpecularMaterial(scb, 283.733);
                mg1.Children.Add(diffuseMaterial);
                mg1.Children.Add(specularMaterial);
                gm3d.BackMaterial = mg1;
                gm3d.Material = mg1;
                return gm3d;
            }
        }
    }
}


    
        

