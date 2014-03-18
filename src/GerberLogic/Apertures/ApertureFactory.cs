using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Apertures
{
    public enum ApertureType
    {
        Circle,
        Rectangle,
        Obround,
        Polygon,
        Custom
    }

    public class ApertureFactory
    {
        
        /// <summary>
        /// Find a previously defined aperture
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="definedApertures"></param>
        /// <returns></returns>
        public static StandardAperture GetApertureFromId(int Id, Dictionary<int, StandardAperture> definedApertures)
        {
            if (definedApertures.ContainsKey(Id))
            {
                return definedApertures[Id];
            }
            return null;
        }

        /// <summary>
        /// Return a new aperture given its number and type.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static StandardAperture NewAperture(int Id, ApertureType type)
        {
            switch (type)
            {
                case ApertureType.Circle:
                    return new CircleAperture();
                case ApertureType.Obround:
                    return new ObroundAperture();
                case ApertureType.Polygon:
                    return new PolygonAperture();
                case ApertureType.Rectangle:
                    return new RectangleAperture();
                default:
                    return new CustomAperture();
            }
        }
    }
}
