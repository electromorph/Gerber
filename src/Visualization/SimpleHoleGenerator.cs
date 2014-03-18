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
    public class SimpleHoleGenerator
    {
        // Private field and public property
        int slices = 32;
        double _diameter = 1;
        Point _center = new Point(0, 0);

        public double Radius { get { return _diameter / 2; } set { _diameter = value * 2; } }
        
        public int Slices
        {
            set { slices = value; }
            get { return slices; }
        }

        // Get the mesh for the hole
        public MeshGeometry3D GetMeshGeometry(Point Center, double Diameter, Side side)
        {
            _center = Center;
            _diameter = Diameter;

            int heightSurfaceIsAt = 0;
            if (side == Side.Back)
            {
                heightSurfaceIsAt = -1;
            }
            else
            {
                heightSurfaceIsAt = 1;
            }

            MeshGeometry3D mesh = new MeshGeometry3D();
            double minx = Center.X, maxx = Center.X, minz = Center.Y, maxz = Center.Y;
            for (int i = 0; i < Slices; i++)
            {
                // Generate Points around top of cylinder
                double x = Center.X + this.Radius * Math.Sin(2 * i * Math.PI / Slices);
                double z = Center.Y + this.Radius * Math.Cos(2 * i * Math.PI / Slices);
                mesh.Positions.Add(new Point3D(x, heightSurfaceIsAt, z));
                // And generate the points around bottom of cylinder
                mesh.Positions.Add(new Point3D(x, 0, z));
                minx = (x < minx) ? x : minx;
                maxx = (x > maxx) ? x : maxx;
                minz = (z < minz) ? z : minz;
                maxz = (z > maxz) ? z : maxz;
            }
            
            for (int i = 0; i < Slices; i++)
            {
                // Triangles along length of cylinder
                mesh.TriangleIndices.Add((2 * i + 2) % (2 * Slices)); 
                mesh.TriangleIndices.Add(2 * i + 1);
                mesh.TriangleIndices.Add(2 * i + 0);

                mesh.TriangleIndices.Add((2 * i + 3) % (2 * Slices)); 
                mesh.TriangleIndices.Add(2 * i + 1);
                mesh.TriangleIndices.Add((2 * i + 2) % (2 * Slices));
            }
            
            double maxExtremity;
            
            //Points at right-angle of triangle from each side.
            for (int i = 0; i < Slices; i++)
            {
                int firstPointIndex = 2 * i;
                int secondPointIndex = (2 * i + 2) % (2 * Slices);
                int newPoint1Index = mesh.Positions.Count;  //This point doesn't yet exist, but will in a moment.
                double x1 = mesh.Positions[firstPointIndex].X;
                double x2 = mesh.Positions[secondPointIndex].X;
                double z1 = mesh.Positions[firstPointIndex].Z;
                double z2 = mesh.Positions[secondPointIndex].Z;
                x1 = Convert.ToDouble(Convert.ToSingle(x1));
                x2 = Convert.ToDouble(Convert.ToSingle(x2));
                z1 = Convert.ToDouble(Convert.ToSingle(z1));
                z2 = Convert.ToDouble(Convert.ToSingle(z2));

                if (((x1 < x2) && (z1 < z2)) || ((x1 > x2) && (z1 > z2)))
                {
                    //Top-left / Bottom-Right
                    mesh.Positions.Add(new Point3D(x1, heightSurfaceIsAt, z2));
                    mesh.TriangleIndices.Add(newPoint1Index);
                    mesh.TriangleIndices.Add(secondPointIndex);
                    mesh.TriangleIndices.Add(firstPointIndex); 
                }
                if (((x1 < x2) && (z1 > z2)) || ((x1 > x2) && (z1 < z2)))
                {
                    //Top-right / bottom-left.
                    mesh.Positions.Add(new Point3D(x2, heightSurfaceIsAt, z1));
                    mesh.TriangleIndices.Add(newPoint1Index);
                    mesh.TriangleIndices.Add(secondPointIndex);
                    mesh.TriangleIndices.Add(firstPointIndex);
                }
                if (x1 == x2)
                {
                    mesh.Positions.Add(new Point3D(x1, heightSurfaceIsAt, (z2 + z1) / 2));
                    mesh.TriangleIndices.Add(newPoint1Index);
                    mesh.TriangleIndices.Add(secondPointIndex);
                    mesh.TriangleIndices.Add(firstPointIndex); 
                }
                if (z1 == z2)
                {
                    mesh.Positions.Add(new Point3D((x2 + x1) / 2, heightSurfaceIsAt, z1));
                    mesh.TriangleIndices.Add(newPoint1Index);
                    mesh.TriangleIndices.Add(secondPointIndex);
                    mesh.TriangleIndices.Add(firstPointIndex);
                }
                
                //Add square to left/right of screen
                //if RHS, extend square to maxx, or if LHS, extend square to minx.
                maxExtremity = (z1 > z2) ? maxx : minx;
                mesh.Positions.Add(new Point3D(maxExtremity, heightSurfaceIsAt, z1));
                mesh.Positions.Add(new Point3D(maxExtremity, heightSurfaceIsAt, z2));
                int newPoint2Index = newPoint1Index + 1;
                int newPoint3Index = newPoint1Index + 2;
                //Top-Right
                if ((x1 > x2) && (z1 < z2))
                {
                    mesh.TriangleIndices.Add(secondPointIndex);
                    mesh.TriangleIndices.Add(newPoint1Index);
                    mesh.TriangleIndices.Add(newPoint3Index);
                    
                    mesh.TriangleIndices.Add(newPoint1Index);
                    mesh.TriangleIndices.Add(newPoint2Index);
                    mesh.TriangleIndices.Add(newPoint3Index);
                }
                //Bottom-Left
                if ((x1 < x2) && (z1 > z2))
                {
                    mesh.TriangleIndices.Add(secondPointIndex);
                    mesh.TriangleIndices.Add(newPoint1Index);
                    mesh.TriangleIndices.Add(newPoint3Index);
                    
                    mesh.TriangleIndices.Add(newPoint1Index);
                    mesh.TriangleIndices.Add(newPoint2Index);
                    mesh.TriangleIndices.Add(newPoint3Index);
                }
                //Bottom-Right
                if ((x1 > x2) && (z1 > z2))
                {
                    mesh.TriangleIndices.Add(newPoint2Index);
                    mesh.TriangleIndices.Add(newPoint1Index);
                    mesh.TriangleIndices.Add(firstPointIndex);
                    
                    mesh.TriangleIndices.Add(newPoint1Index);
                    mesh.TriangleIndices.Add(newPoint2Index);
                    mesh.TriangleIndices.Add(newPoint3Index);
                }
                //Top-Left
                if ((x1 < x2) && (z1 < z2))
                {
                    mesh.TriangleIndices.Add(newPoint2Index);
                    mesh.TriangleIndices.Add(newPoint1Index);
                    mesh.TriangleIndices.Add(firstPointIndex);
                    
                    mesh.TriangleIndices.Add(newPoint1Index);
                    mesh.TriangleIndices.Add(newPoint2Index);
                    mesh.TriangleIndices.Add(newPoint3Index);
                }
            }
            var pointCollection = new PointCollection();
            
            //Make texture
            foreach (Point3D point3d in mesh.Positions)
            {
                pointCollection.Add(new Point(point3d.X, point3d.Z));
            }

            mesh.TextureCoordinates = pointCollection;
                
            // We know this object won't be changed, so freeze it
            mesh.Freeze();
            return mesh;
        }
    }
}