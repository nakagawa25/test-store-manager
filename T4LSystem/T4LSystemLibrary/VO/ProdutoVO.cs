using System;

namespace T4LSystemLibrary.VO
{
    public class ProdutoVO : StandardVO
    {
        public string Descricao { get; set; }
        public int CodGrupo { get; set; }
        public string CodBarra { get; set; }
        public double PrecoCusto{ get; set; }
        public double PrecoVenda { get; set; }
        public int Ativo { get; set; }
        public long CodMedida { get; set; }
        public DateTime DataHoraCadastro { get; set; }
    }
}
