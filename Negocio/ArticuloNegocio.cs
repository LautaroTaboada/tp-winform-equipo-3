using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT Codigo, Nombre, Descripcion, Precio FROM ARTICULOS");
                datos.EjecutarLectura();

                while (datos.lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Codigo = (string)datos.lector["Codigo"];
                    aux.Nombre = (string)datos.lector["Nombre"];
                    aux.Descripcion = (string)datos.lector["Descripcion"];
                    aux.Precio = (Decimal)datos.lector["Precio"];
                    lista.Add(aux);
                }


                return lista;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public void agregar(Articulo articulo)
        {
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;";
                comando.CommandType = System.Data.CommandType.Text;

                comando.CommandText = "INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) " +
                                     "VALUES (@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @precio)";

                comando.Parameters.AddWithValue("@codigo", articulo.Codigo);
                comando.Parameters.AddWithValue("@nombre", articulo.Nombre);
                comando.Parameters.AddWithValue("@descripcion", articulo.Descripcion);
                comando.Parameters.AddWithValue("@idMarca", articulo.Marca.idMarca);
                comando.Parameters.AddWithValue("@idCategoria", articulo.Categoria.idCategoria);
                comando.Parameters.AddWithValue("@precio", articulo.Precio);

                comando.Connection = conexion;

                conexion.Open();
                comando.ExecuteNonQuery();

                conexion.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
