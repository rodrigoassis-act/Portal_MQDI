using System.Collections.Generic;
using System.Linq;

namespace ONS.PortalMQDI.Models.Model
{
    public class FiltroBase
    {
        public List<string> ErroMessage { get; set; } = new List<string>();

        public bool IsValido()
        {
            if (ErroMessage.Count() > 0)
                return false;
            else
                return true;
        }
    }
}
