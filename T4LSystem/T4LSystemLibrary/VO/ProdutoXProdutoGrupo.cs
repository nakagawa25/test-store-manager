namespace T4LSystemLibrary.VO
{
    public class ProdutoXProdutoGrupo : StandardVO
    {
        public string Descricao { get; set; }
        public string CodBarra { get; set; }
        public string NomeGrupo { get; set; }
        public double PrecoCusto { get; set; }
        public double PrecoVenda { get; set; }
        public int Ativo { get; set; }
        public string UnidadeMedida { get; set; }
    }
}
