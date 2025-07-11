using DataAccess.Persistencia;
using Models.DTO;
using System;
using System.Data.SqlClient;

namespace DataAccess.Repository
{
    public class ProductoRepository : PConexion
    {
        public void AgregarProducto(Producto producto)
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;

            using (conexion = this.GetConnection())
            {
                try
                {
                    conexion.Open();
                    string sqlAgregarFamilia = "INSERT INTO Producto (CodigoProducto, Descripcion, PrecioVenta, CodigoFamilia) values (@codigo, @descripcion, @precioVenta, @codigoFamilia)";
                    cmd = new SqlCommand(sqlAgregarFamilia, conexion);

                    cmd.Parameters.AddWithValue("codigo", producto.CodigoProducto);
                    cmd.Parameters.AddWithValue("descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("precioVenta", producto.PrecioVenta);
                    cmd.Parameters.AddWithValue("codigoFamilia", producto.CodigoFamilia);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                    conexion.Close();
                }
            }
        }

        public int? GetCantidadProductos()
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            int? cantidad = null;

            using (conexion = this.GetConnection())
            {
                try
                {
                    conexion.Open();
                    string sql = "SELECT count(CodigoProducto) as Cantidad FROM Producto";
                    cmd = new SqlCommand(sql, conexion);
                    cantidad = (int)cmd.ExecuteScalar();

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                }
            }
            return cantidad;
        }

        public bool ExisteProducto(string id)
        {
            bool existe = false;
    
            using (SqlConnection conexion = this.GetConnection())
            {
                try
                {
                    conexion.Open();
                    string sql = "SELECT CodigoProducto FROM Producto WHERE CodigoProducto = @id";
            
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        existe = cmd.ExecuteScalar() != null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }
            return existe;
        }

    }
}
