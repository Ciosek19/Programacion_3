using DataAccess.Persistencia;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Repository
{
    public class FamiliaProductoRepository : PConexion
    {
        public void AgregarFamilia(FamiliaProducto nuevaFamilia) 
        {
            SqlConnection context = null;
            SqlCommand cmd = null;

            using (context = this.GetConnection()) 
            {
                try
                {
                    context.Open();
                    string sqlAgregarFamilia = "insert into Familia_Producto (CodigoFamilia, Descripcion) values (@codigo, @descripcion)";
                    cmd = new SqlCommand(sqlAgregarFamilia, context);

                    cmd.Parameters.AddWithValue("codigo", nuevaFamilia.CodigoFamilia);
                    cmd.Parameters.AddWithValue("descripcion", nuevaFamilia.Descripcion);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally 
                {
                    if(cmd != null) 
                    { 
                        cmd.Dispose(); 
                    }
                    context.Close();
                }
            }
        }

        public List<FamiliaProducto> GetFamilias()
        {


            List<FamiliaProducto> colFamilias = new List<FamiliaProducto>();

            SqlConnection context = null;
            SqlDataReader sqlReader = null;
            SqlCommand cmd = null;

            using (context = this.GetConnection())
            {
                try
                {
                    context.Open();
                    string sqlSelectFamilias = "select CodigoFamilia, Descripcion from Familia_Producto";
                    cmd = new SqlCommand(sqlSelectFamilias, context);
                    sqlReader = cmd.ExecuteReader();

                    if (sqlReader.HasRows)
                    {
                        while (sqlReader.Read())
                        {
                            FamiliaProducto familia = new FamiliaProducto(sqlReader["codigoFamilia"].ToString(), sqlReader["descripcion"].ToString());
                            colFamilias.Add(familia);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
                finally
                {
                    if (sqlReader != null)
                    {
                        sqlReader.Dispose();
                    }

                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }

                }
            }
            return colFamilias;
        }

        public List<FamiliaProducto> GetFamiliasConDataSet()
        {
            List<FamiliaProducto> colFamilias = new List<FamiliaProducto>();

            using (SqlConnection context = this.GetConnection())
            {
                string sql = "SELECT CodigoFamilia, Descripcion FROM Familia_Producto";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, context);
                DataSet ds = new DataSet();

                try
                {
                    adapter.Fill(ds, "Familias");

                    foreach (DataRow row in ds.Tables["Familias"].Rows)
                    {
                        string cod = row["CodigoFamilia"].ToString();
                        string desc = row["Descripcion"].ToString();

                        colFamilias.Add(new FamiliaProducto(cod, desc));
                    }
                }
                catch (Exception ex)
                {
                    // Manejá el error si querés mostrar un mensaje
                }
            }

            return colFamilias;
        }
    }
}
