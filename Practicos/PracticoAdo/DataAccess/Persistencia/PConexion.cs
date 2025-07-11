using System.Data.SqlClient;

namespace DataAccess.Persistencia
{
    public class PConexion
    {
        private string connectionString = "Server=DESKTOP-QHKBMIA;Database=Almacen;Integrated Security=True;"; //Cambiar el connection string
        protected SqlConnection GetConnection() 
        {
            SqlConnection context = new SqlConnection(connectionString);
            return context;
        }
    }
}
