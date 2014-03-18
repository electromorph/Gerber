using System;
using System.Collections.Generic;
using System.Text;

namespace GerberLogic
{
    public class StepAndRepeat
    {
        private int _numberOfRepetitionsAlongX;
        public int NumberOfRepetitionsAlongX
        {
            get { return _numberOfRepetitionsAlongX; }
            set { _numberOfRepetitionsAlongX = value; }
        }

        private int _numberOfRepetitionsAlongY;
        public int NumberOfRepetitionsAlongY
        {
            get { return _numberOfRepetitionsAlongY; }
            set { _numberOfRepetitionsAlongY = value; }
        }

        private float _stepDistanceOnX;
        public float StepDistanceOnX
        {
            get { return _stepDistanceOnX; }
            set { _stepDistanceOnX = value; }
        }

        private float _stepDistanceOnY;
        public float StepDistanceOnY
        {
            get { return _stepDistanceOnY; }
            set { _stepDistanceOnY = value; }
        }
    }
}
