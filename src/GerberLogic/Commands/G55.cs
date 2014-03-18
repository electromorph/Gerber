using System.Collections.Generic;

namespace GerberLogic.Commands
{
    /// <summary>
    /// Prepare for Flash - this command does not add anything to D03, so it's just there to 
    /// stop a null from appearing in the command list.
    /// </summary>
    public class G55 : iCommand
    {
        public G55(Dictionary<string, float> CommandArgs, Globals Globals, GerberImage Image)
        { }
        
        public void Process()
        {
            
        }
    }
}
