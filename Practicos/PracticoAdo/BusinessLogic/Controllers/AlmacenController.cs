using DataAccess.Persistencia;
using DataAccess.Repository;
using Models.DTO;
using System.Collections.Generic;

namespace BusinessLogic.Controllers
{
    public class AlmacenController
    {
        private Repository _Repository;

        public AlmacenController()
        {
            this._Repository = new Repository();
        }

        public void AgregarFamilia(FamiliaProducto nuevaFamilia) 
        {
            this._Repository.getFamiliaRepository().AgregarFamilia(nuevaFamilia);
        }

        public void AgregarProducto(Producto producto) 
        {
            this._Repository.getProductoRepository().AgregarProducto(producto);
        }

        public List<FamiliaProducto> GetFamilias()
        {
            return this._Repository.getFamiliaRepository().GetFamilias();
        }

        public int? CantidadProductos()
        {
            return this._Repository.getProductoRepository().GetCantidadProductos();
        }

        public bool ExisteProducto(string id)
        {
            return this._Repository.getProductoRepository().ExisteProducto(id);
        }
    }
}
