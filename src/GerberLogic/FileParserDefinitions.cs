using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using GerberLogic.Apertures;
using GerberLogic.DrawingElements;

namespace GerberLogic
{
    /// <summary>
    /// The axes in the Gerber file are X and Y.  The corresponding axes on the physical
    /// plotter are A and B. This parameter changes the mapping.
    /// </summary>
    public enum AxisSelect
    {
        /// <summary>
        /// X maps to plotter axis A.  Y maps to plotter axis B.
        /// </summary>
        AxBy,
        /// <summary>
        /// Y maps to plotter axis A.  X maps to plotter axis B.
        /// </summary>
        AyBx
    }

    /// <summary>
    /// The units of length used on the plot.
    /// </summary>
    public enum Units
    {
        Inches,
        mm
    }

    /// <summary>
    /// Are the coordinates specified absolute or relative to the last point.
    /// </summary>
    public enum Coordinates
    {
        Absolute,
        Incremental
    }

    /// <summary>
    /// Exposure mode
    /// </summary>
    public enum ExposureMode
    {
        Off,
        On
    }

    /// <summary>
    /// This class keeps track of all the Gerber 'global' variables such as macros, whether polarity
    /// is positive or negative etc.
    /// </summary>
    public class Globals
    {
        
        private bool _polygonAreaFill = false;
        public bool PolygonAreaFill
        {
            get { return _polygonAreaFill; }
            set { _polygonAreaFill = value; }
        }

        private float _linearScaleFactor = 1;
        public float LinearScaleFactor
        {
            get { return _linearScaleFactor; }
            set { _linearScaleFactor = value; }
        }

        private bool _coordinatesAreAbsolute = true;
        public bool CoordinatesAreAbsolute
        {
            get { return _coordinatesAreAbsolute; }
            set { _coordinatesAreAbsolute = value; }
        }

        private AxisSelect _axes = AxisSelect.AxBy;
        public AxisSelect Axes
        {
            get { return _axes; }
            set { _axes = value; }
        }

        private AxisMirrorImage _mirrorImage = new AxisMirrorImage();
        public AxisMirrorImage MirrorImage
        {
            get { return _mirrorImage; }
            set { _mirrorImage = value; }
        }

        private Units _units = Units.mm;
        public Units Units
        {
            get { return _units; }
            set { _units = value; }
        }

        private Offset _offset = new Offset();
        public Offset Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        private Offset _scaleFactor = new Offset();
        public Offset ScaleFactor
        {
            get { return _scaleFactor; }
            set { _scaleFactor = value; }
        }

        private bool _omitLeadingZeros = false;
        public bool OmitLeadingZeros
        {
            get { return _omitLeadingZeros; }
            set { _omitLeadingZeros = value; }
        }

        private CommandNumberFormat _formatN = new CommandNumberFormat();
        public CommandNumberFormat FormatN
        {
            get { return _formatN; }
            set { _formatN = value; }
        }

        private CommandNumberFormat _formatG = new CommandNumberFormat();
        public CommandNumberFormat FormatG
        {
            get { return _formatG; }
            set { _formatG = value; }
        }

        private CommandNumberFormat _formatD = new CommandNumberFormat();
        public CommandNumberFormat FormatD
        {
            get { return _formatD; }
            set { _formatD = value; }
        }

        private CommandNumberFormat _formatM = new CommandNumberFormat();
        public CommandNumberFormat FormatM
        {
            get { return _formatM; }
            set { _formatM = value; }
        }

        private string _plotterFilm = "STANDARD";
        public string PlotterFilm
        {
            get { return _plotterFilm; }
            set { _plotterFilm = value; }
        }

        private StepAndRepeat _stepAndRepeat = new StepAndRepeat();
        public StepAndRepeat StepAndRepeat
        {
            get { return _stepAndRepeat; }
            set { _stepAndRepeat = value; }
        }

        /// <summary>
        /// Are circles being drawn single or multiple quadrants?
        /// </summary>
        private bool _circlesAreSingleQuadrant;
        public bool CirclesAreSingleQuadrant
        {
            get { return _circlesAreSingleQuadrant; }
            set { _circlesAreSingleQuadrant = value; }
        }

        /// <summary>
        /// Are we drawing lines or circles?
        /// </summary>
        private DrawingElement _currentComponent;
        public DrawingElement CurrentComponent
        {
            get { return _currentComponent; }
            set { _currentComponent = value; }
        }

        private Coordinates _Coordinates;
        public Coordinates Coordinates
        {
            get { return _Coordinates; }
            set { _Coordinates = value; }
        }

        public StandardAperture CurrentAperture { get; set; }

        /// <summary>
        /// LastStandardAperture
        /// </summary>
        public StandardAperture LastStandardAperture { get; set; }

        private Dictionary<int, StandardAperture> _apertures = new Dictionary<int, StandardAperture>();
        public Dictionary<int, StandardAperture> Apertures
        {
            get { return _apertures; }
            set { _apertures = value; }
        }

        private Dictionary<string, StandardAperture> _apertureMacros = new Dictionary<string, StandardAperture>();
        public Dictionary<string, StandardAperture> ApertureMacros
        {
            get { return _apertureMacros; }
            set { _apertureMacros = value; }
        }

        private PointF _lastPoint = new PointF(0, 0);
        public PointF LastPoint
        {
            get { return _lastPoint; }
            set { _lastPoint = value; }
        }

        private PointF _lastIJValue = new PointF(0, 0);
        public PointF LastIJValue
        {
            get { return _lastIJValue; }
            set { _lastIJValue = value; }
        }

        private string _lastCommand = "";
        public string LastCommand
        {
            get { return _lastCommand; }
            set { _lastCommand = value; }
        }
                
        private ExposureMode _exposure = ExposureMode.Off;
        public ExposureMode Exposure
        {
            get { return _exposure; }
            set { _exposure = value; }
        }
    }
}