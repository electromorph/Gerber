using System.Collections.Generic;

namespace GerberLogic.Commands
{
    /// <summary>
    /// Aperture change - this command does not add anything to Dxx, so it's just there to 
    /// stop a null from appearing in the command list.
    /// </summary>
    public class G54 : iCommand
    {
        public G54(Dictionary<string, float> CommandArgs, Globals Globals, GerberImage Image)
        { }
        
        public void Process()
        { }
    }
}
