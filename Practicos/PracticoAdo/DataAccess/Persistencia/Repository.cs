using DataAccess.Repository;

namespace DataAccess.Persistencia
{
    public class Repository
    {
        private ProductoRepository _ProductoRepository;
        private FamiliaProductoRepository _FamiliaRepository;

        public Repository()
        {
            this._ProductoRepository = new ProductoRepository();
            this._FamiliaRepository = new FamiliaProductoRepository();
        }

        public ProductoRepository getProductoRepository()
        {
            return this._ProductoRepository;
        }

        public FamiliaProductoRepository getFamiliaRepository()
        {
            return this._FamiliaRepository;
        }
    }
}
