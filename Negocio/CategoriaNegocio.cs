using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> listar()
        {
            List<Categoria> lista = new List<Categoria>();

            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT Id, Descripcion FROM CATEGORIAS";
                comando.Connection = conexion;

                conexion.Open();

                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Categoria aux = new Categoria();

                    aux.idCategoria= (int)lector["Id"];
                    aux.Descripcion = (string)lector["Descripcion"];

                    lista.Add(aux);
                }

                conexion.Close();

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void agregar(Categoria nueva)
        {
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();

            try
            {
                
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;";
                comando.CommandType = System.Data.CommandType.Text;
                
                comando.CommandText = "INSERT INTO CATEGORIAS (Descripcion) VALUES (@descripcion)";
                
                comando.Parameters.AddWithValue("@descripcion", nueva.Descripcion);
                comando.Connection = conexion;

                conexion.Open();
                
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
                conexion.Close();
            }
        }

        
        public void modificar(Categoria seleccionada)
        {
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;";
                comando.CommandType = System.Data.CommandType.Text;
                
                comando.CommandText = "UPDATE CATEGORIAS SET Descripcion = @descripcion WHERE Id = @id";
                comando.Parameters.AddWithValue("@descripcion", seleccionada.Descripcion);
                
                comando.Parameters.AddWithValue("@id", seleccionada.idCategoria);
                comando.Connection = conexion;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
        }

        
        public void eliminar(int id)
        {
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;";
                comando.CommandType = System.Data.CommandType.Text;
                
                comando.CommandText = "DELETE FROM CATEGORIAS WHERE Id = @id";
                comando.Parameters.AddWithValue("@id", id);
                comando.Connection = conexion;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
        }
    }
}
