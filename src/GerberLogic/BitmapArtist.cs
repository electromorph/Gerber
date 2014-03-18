using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using GerberLogic.Apertures;
using GerberLogic.Apertures.Primitives;
using GerberLogic.Apertures.Primitives.PrimitiveParameters;
using GerberLogic.DrawingElements;
using System.IO;
using GerberLogic.Helper;

namespace GerberLogic
{
    /// <summary>
    /// Outputs a bitmap whose dimensions are the largest size required by the Gerber plot.
    /// </summary>
    public static class BitmapArtist
    {
        public static GerberImage GeneratePathsForComponents(GerberImage gerberImage)
        {
            bool lastComponentPolygonFill = false;
            //Build up component paths layer-by-layer.
            foreach (Layer layer in gerberImage.Layers)
            {
                lastComponentPolygonFill = false;
                foreach (DrawingElement cmp in layer.Components)
                {
                    GraphicsPath componentPath = cmp.Draw();
                    gerberImage.CheckIfPathIncreasesImageSize(componentPath);                    
                    
                    if (cmp.PolygonAreaFill && lastComponentPolygonFill)
                    {
                        //Add path to existing path.
                        gerberImage.ImageGraphicsPaths[gerberImage.ImageGraphicsPaths.Count - 1].GraphicsPath.AddPath(componentPath, true);
                    }
                    else if (!cmp.PolygonAreaFill && lastComponentPolygonFill)
                    {
                        //Close the existing path and create a new one.
                        gerberImage.ImageGraphicsPaths[gerberImage.ImageGraphicsPaths.Count - 1].GraphicsPath.CloseFigure();
                        gerberImage.ImageGraphicsPaths.Add(new GerberGraphicsPath(componentPath, cmp.ApertureUsed.Width, layer.Polarity == LayerPolarity.Clear));
                        lastComponentPolygonFill = false;
                    }
                    else
                    {
                        //A new path.
                        gerberImage.ImageGraphicsPaths.Add(new GerberGraphicsPath(componentPath, cmp.ApertureUsed.Width, layer.Polarity == LayerPolarity.Clear));
                        lastComponentPolygonFill = cmp.PolygonAreaFill;
                    }
                }
            }
            return gerberImage;
        }

        public static Bitmap GenerateBitmap(GerberImage gerberImage)
        {
            const int DPI = 1000;
            //NOW DRAW THE ACTUAL BITMAP
            float widthAddAmount = 0;
            float heightAddAmount = 0;
            //Translate everything down to below 0,0
            if (gerberImage.TopLeftCorner.X < 0)
            {
                widthAddAmount = -gerberImage.TopLeftCorner.X;
            }
            if (gerberImage.TopLeftCorner.Y < 0)
            {
                heightAddAmount = -gerberImage.TopLeftCorner.Y;
            }
            int width = Convert.ToInt32(DPI * (gerberImage.BottomRightCorner.X - gerberImage.TopLeftCorner.X + widthAddAmount));
            int height = Convert.ToInt32(DPI * (gerberImage.BottomRightCorner.Y - gerberImage.TopLeftCorner.Y + heightAddAmount));
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            GraphicsPath whiteBackground = new GraphicsPath();
            whiteBackground.AddRectangle(new Rectangle(0, 0, width, height));
            //BAZOOKA - THIS WAS CHANGED TO ENABLE ME TO SEE THE WHITE SIDE OF THE BOARD WHEN NOTHING IS DRAWN ON IT.
            //g.FillPath(new SolidBrush(System.Drawing.Color.White), whiteBackground);
            g.FillPath(new SolidBrush(System.Drawing.Color.LightGreen), whiteBackground);
            Matrix translateMatrix = new Matrix();
            //Locate image at top-left corner.
            translateMatrix.Translate(-gerberImage.TopLeftCorner.X, -gerberImage.TopLeftCorner.Y);
            translateMatrix.Scale(1, -1);
            translateMatrix.Translate(0, -height);
            translateMatrix.Scale(DPI, DPI);
            foreach (GerberGraphicsPath gpath in gerberImage.ImageGraphicsPaths)
            {
                GraphicsPath path = (GraphicsPath)gpath.GraphicsPath.Clone();
                path.Transform(translateMatrix);
                g.FillPath(new SolidBrush(gpath.FillColor), path);
                float pathWidth = gpath.Width / (gerberImage.MinimumFeatureSize);
                //float pathWidth = gpath.Width;
                System.Drawing.Pen pen = new System.Drawing.Pen(gpath.LineColor, pathWidth);
                g.DrawPath(pen, path);
                pen.Dispose();
            }
            g.Dispose();
            return bmp;
        }
    }
}
