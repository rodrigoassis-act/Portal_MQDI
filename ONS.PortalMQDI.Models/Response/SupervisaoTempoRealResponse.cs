using ONS.PortalMQDI.Models.ViewModel;
using System.Collections.Generic;

namespace ONS.PortalMQDI.Models.Response
{
    public class SupervisaoTempoRealResponse
    {
        public int Status { get; set; }
        public string Msg { get; set; }
        public List<SupervisaoTempoRealViewModel> Data { get; set; }
    }
}
