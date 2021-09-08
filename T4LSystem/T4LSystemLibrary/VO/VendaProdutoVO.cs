namespace T4LSystemLibrary.VO
{
    public class VendaProdutoVO : StandardVO
    {
        public long CodVenda { get; set; }
        public long CodProduto { get; set; }
        public double PrecoVenda { get; set; }
        public double Quantidade { get; set; }
    }
}
