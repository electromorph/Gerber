using System;
using System.IO;
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
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using GerberLogic;
using System.Drawing;
using System.Windows.Media.Media3D;
using _3DTools;
using Visualization;
using ExcellonLogic;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ExcellonFile excellon = new ExcellonFile();
        BitmapImage FrontArtwork;
        BitmapImage BackArtwork;
        List<DrillBit> Tools = new List<DrillBit>();
        Square BoardOutline = null;

        public MainWindow()
        {
            InitializeComponent();
        }
        
        public void ProcessFile(string Filename, Side side)
        {
            FileParser fp = new FileParser();
            Bitmap bmp = fp.ProcessFile(Filename);
            string bitmapFilename = SaveBmp(bmp);
            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            bmpImage.CacheOption = BitmapCacheOption.OnLoad;
            bmpImage.UriSource = new Uri(bitmapFilename, UriKind.Absolute);
            bmpImage.EndInit();
            File.Delete(bitmapFilename);
            if (side == Side.Front)
            {
                this.FrontArtwork = bmpImage;
            }
            else
            {
                this.BackArtwork = bmpImage;
            }
            if (this.BoardOutline != null)
            {
                double biggestWidth = this.BoardOutline.Width > bmpImage.Width ? this.BoardOutline.Width : bmpImage.Width;
                double biggestHeight = this.BoardOutline.Height > bmpImage.Height ? this.BoardOutline.Height : bmpImage.Height;
                this.BoardOutline = new Square(new System.Windows.Point(0, 0), new System.Windows.Point(biggestWidth, biggestHeight));
            }
            else
            {
                this.BoardOutline = new Square(new System.Windows.Point(0, 0), new System.Windows.Point(bmpImage.Width, bmpImage.Height));
            }
            //c(this.FrontArtwork, this.BackArtwork, excellon.ThroughHoles, this.BoardOutline);
        }

        private string SaveBmp(Bitmap bmp)
        {
            string filename = System.Environment.CurrentDirectory + "Output" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".png";
            bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
            return filename;
        }

        private void CreateScene(BitmapImage FrontArtwork, BitmapImage BackArtwork, List<Hole> ListOfVias, Square BoardProfile)
        {
            //if ((FrontArtwork != null) && (BackArtwork != null))
            //{
            //    if ( (FrontArtwork.Width != BackArtwork.Width) || (FrontArtwork.Height != BackArtwork.Height) )
            //    {
            //        throw new Exception("Bitmap sizes do not match (" + FrontArtwork.Width + "x" + FrontArtwork.Height + ") vs (" + BackArtwork.Width + "x" + BackArtwork.Height + ") Check that bitmaps are correctly aligned.");
            //    }
            //}

            var boardMaker = new BoardMaker();

            if (BoardProfile != null)
            {
                boardMaker.BoardOutline = this.BoardOutline;
            }

            if (ListOfVias != null)
            {
                foreach (Hole hole in ListOfVias)
                {
                    if (hole.Tool != null)
                    {
                        boardMaker.Vias.Add(new Via(new System.Windows.Point(hole.Position.X, hole.Position.Y), hole.Tool.Diameter));
                    }
                }
            }

            //this.Picture.ClearChildren();

            if (FrontArtwork != null)
            {
                //FRONT ARTWORK:  Generate the mesh, assign to a GeometryModel & attach material.
                MeshGeometry3D frontSide = boardMaker.Generate(Side.Front);
                //Image Brush - this is the PCB Image.
                ImageBrush ibFront = new ImageBrush(FrontArtwork);
                //ibFront.Stretch = Stretch.Fill;
                
                ////Almighty Test
                //RotateTransform aRotateTransform = new RotateTransform();
                //aRotateTransform.CenterX = 0.5;
                //aRotateTransform.CenterY = 0.5;
                //aRotateTransform.Angle = 45;
                //ibFront.RelativeTransform = aRotateTransform;

                ibFront.Opacity = 1;
                Material frontMaterial = new DiffuseMaterial(ibFront);
                Material frontMaterialBack = new DiffuseMaterial(new SolidColorBrush(Colors.LightSalmon));
                //Create the geometry.
                GeometryModel3D gm3dFront = new GeometryModel3D(frontSide, frontMaterialBack);
                //Assign the back material (will go later; the back won't be visible).
                gm3dFront.BackMaterial = frontMaterial;
                //Display the model.
                ModelBase front = new ModelBase(gm3dFront);
                this.Picture.Children.Add(front);
            }

            if (BackArtwork != null)
            {
                //BACK ARTWORK:  Generate the mesh, assign to a GeometryModel & attach material.
                MeshGeometry3D backSide = boardMaker.Generate(Side.Back);
                //Image Brush - this is the PCB Image.
                ImageBrush ibBack = new ImageBrush(BackArtwork);
                ibBack.Stretch = Stretch.Fill;
                ibBack.Opacity = 1;
                Material backMaterial = new DiffuseMaterial(ibBack);
                Material backMaterialBack = new DiffuseMaterial(new SolidColorBrush(Colors.LightSalmon));
                //Create the geometry.
                GeometryModel3D gm3dBack = new GeometryModel3D(backSide, backMaterial);
                //Assign the back material (will go later; the back won't be visible).
                gm3dBack.BackMaterial = backMaterialBack;
                //Display the model.
                ModelBase back = new ModelBase(gm3dBack);
                back.Move(0, 0, 0, 0);
                this.Picture.Children.Add(back);
            }

            this.Picture.ZoomExtents();
        }

        private string SelectFileName()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Gerber documents (.txt)|*.txt"; // Filter files by extension

            // Show Dialog & process
            return (dlg.ShowDialog() ?? false) ? dlg.FileName : string.Empty;
            //AddFileEntryToPCB(dlg.FileName);
        }

        private void btnSelectFile1_Click(object sender, RoutedEventArgs e)
        {
            string fileName = SelectFileName();
            lblFile1.Content = fileName;
            excellon.ProcessFile(fileName);
            //CreateScene(this.FrontArtwork, this.BackArtwork, excellon.ThroughHoles, this.BoardOutline);
        }

        private void btnSelectFile2_Click(object sender, RoutedEventArgs e)
        {
            
            string fileName = SelectFileName();
            lblFile2.Content = fileName;
            ProcessFile(fileName, Side.Front);
        }

        private void btnSelectFile3_Click(object sender, RoutedEventArgs e)
        {
            string fileName = SelectFileName();
            if (FrontArtwork == null)
            {
                lblFile2.Content = fileName;
                ProcessFile(fileName, Side.Front);
            }
            else
            {
                lblFile3.Content = fileName;
                ProcessFile(fileName, Side.Back);
            }
        }

        private void ShowPCB_Click(object sender, RoutedEventArgs e)
        {
            CreateScene(this.FrontArtwork, this.BackArtwork, excellon.ThroughHoles, this.BoardOutline);
        }

    }
    
    public sealed class GerberFile
    {
        public string Filename { get; set; }
        public string Status { get; set; }
        public string Contents { get; set; }
        public Bitmap Visualization { get; set; }

        public GerberFile(string Filename, string Status, string Contents)
        {
            this.Filename = Filename;
            this.Status = Status;
            this.Contents = Contents;
            Visualization = new Bitmap(@"pcb.png");
        }
    }
}
