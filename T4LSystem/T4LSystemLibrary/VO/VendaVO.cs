using System;

namespace T4LSystemLibrary.VO
{
    public class VendaVO : StandardVO
    {
        public string ClienteDocumento { get; set; }
        public string ClienteNome { get; set; }
        public string Obs { get; set; }
        public double Total { get; set; }
        public DateTime DataHora { get; set; }
    }
}
