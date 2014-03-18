using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using _3DTools;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for AssemblePCB.xaml
    /// </summary>
    public partial class AssemblePCB : Window
    {
        public AssemblePCB()
        {
            InitializeComponent();
            CreateScene();
        }
        
        private void CreateScene()
        {
            var boardMaker = new BoardMaker();
            
            //Hardcode board size for now!
            boardMaker.BoardOutline = new Square(new Point(0, 0), new Point(5, 5));
            
            //Hardcode a couple of vias for now!
            boardMaker.Vias.Add(new Via(new Point(2, 2), 2));
            boardMaker.Vias.Add(new Via(new Point(4, 2), 1));
            boardMaker.Vias.Add(new Via(new Point(2, 4), 1));
            
            //Generate the mesh, assign to a GeometryModel & attach material.
            MeshGeometry3D mesh = boardMaker.Generate(Side.Front);
            
            //These get us started, but will be replaced by ImageBrush once the Geometry is sorted out.
            //Material material = new DiffuseMaterial(new SolidColorBrush(Colors.LightGreen));
            //Image Brush - will use when we get everything else working.
            ImageBrush ib = new ImageBrush(new BitmapImage(new Uri(System.Environment.CurrentDirectory + @"\pcb.png")));
            ib.Stretch = Stretch.Uniform;
            ib.Opacity = 1;
            Material material = new DiffuseMaterial(ib);

            Material materialBack = new DiffuseMaterial(new SolidColorBrush(Colors.LightSalmon));
                        
            //Create the geometry.
            GeometryModel3D gm3d = new GeometryModel3D(mesh, materialBack);
            
            //Assign the back material (will go later; the back won't be visible).
            gm3d.BackMaterial = material;
            
            //Display the model.
            ModelBase pcb = new ModelBase(gm3d);
            pcb.Move(0, 0, 0, 0);
            this.Picture.Children.Add(pcb);
        }
        int CameraPosition = 0;
        private void viewport_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int delta = (e.Delta > 1) ? 1 : 0;
            //Point pos = e.GetPosition(this);
            //this.myPerspectiveCamera.Position = new Point3D(this.myPerspectiveCamera.Position.X, this.myPerspectiveCamera.Position.Y, this.myPerspectiveCamera.Position.Z + delta);
            this.CameraPosition += delta;
            lblMouseWheelPosition.Content = CameraPosition + "(" + delta + ")";
        }

        private void scrollBar1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.myPerspectiveCamera.Position = new Point3D(this.myPerspectiveCamera.Position.X, this.myPerspectiveCamera.Position.Y - (e.NewValue - e.OldValue), this.myPerspectiveCamera.Position.Z);
        }

        private void scrollBar2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.myPerspectiveCamera.Position = new Point3D(this.myPerspectiveCamera.Position.X + (e.NewValue - e.OldValue), this.myPerspectiveCamera.Position.Y, this.myPerspectiveCamera.Position.Z);
        }
    }
}
