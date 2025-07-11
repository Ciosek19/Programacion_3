namespace Models.DTO
{
    public class Producto
    {
        private string codigoProducto;
        private float precioVenta;
        private string descripcion;
        private string codigoFamilia;
        private FamiliaProducto familiaProducto;

        public Producto() { }

        public string CodigoProducto { get => codigoProducto; set => codigoProducto = value; }
        public float PrecioVenta { get => precioVenta; set => precioVenta = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string CodigoFamilia { get => codigoFamilia; set => codigoFamilia = value; }
        public FamiliaProducto FamiliaProducto { get => familiaProducto; set => familiaProducto = value; }
    }
}
