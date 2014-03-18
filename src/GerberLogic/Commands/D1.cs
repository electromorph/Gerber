﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    public class D1 : iCommand
    {
        Dictionary<string, float> _commandArgs;
        Globals _globals;
        GerberImage _image;

        public D1 (Dictionary<string, float> CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        } 

        public void Process()
        {
            _globals.Exposure = ExposureMode.On;
        }
    }
}
