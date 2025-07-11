namespace Models.DTO
{
    public class FamiliaProducto
    {
        private string codigoFamilia;
        private string descripcion;

        public FamiliaProducto() { }

        public FamiliaProducto(string codigoFamilia, string descripcion)
        {
            this.codigoFamilia = codigoFamilia;
            this.descripcion = descripcion;
        }

        public string CodigoFamilia { get => codigoFamilia; set => codigoFamilia = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
    }
}
