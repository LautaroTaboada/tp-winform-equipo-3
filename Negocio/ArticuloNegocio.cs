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
                datos.SetearConsulta("SELECT Id, Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio FROM ARTICULOS");
                datos.EjecutarLectura();

                while (datos.lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.Id = (int)datos.lector["Id"];
                    aux.Codigo = (string)datos.lector["Codigo"];
                    aux.Nombre = (string)datos.lector["Nombre"];
                    aux.Descripcion = (string)datos.lector["Descripcion"];

                    aux.Marca = new Marca();
                    aux.Marca.idMarca = (int)datos.lector["IdMarca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.idCategoria = (int)datos.lector["IdCategoria"];

                    aux.Precio = (decimal)datos.lector["Precio"];

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
        public void modificar(Articulo articulo)
        {
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;";
                comando.CommandType = System.Data.CommandType.Text;

                comando.CommandText = "UPDATE ARTICULOS SET Codigo = @codigo, Nombre = @nombre, " +
                                     "Descripcion = @descripcion, IdMarca = @idMarca, " +
                                     "IdCategoria = @idCategoria, Precio = @precio " +
                                     "WHERE Id = @id";

                comando.Parameters.AddWithValue("@codigo", articulo.Codigo);
                comando.Parameters.AddWithValue("@nombre", articulo.Nombre);
                comando.Parameters.AddWithValue("@descripcion", articulo.Descripcion);
                comando.Parameters.AddWithValue("@idMarca", articulo.Marca.idMarca);
                comando.Parameters.AddWithValue("@idCategoria", articulo.Categoria.idCategoria);
                comando.Parameters.AddWithValue("@precio", articulo.Precio);
                comando.Parameters.AddWithValue("@id", articulo.Id);

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
        public void eliminar(int id)
        {
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;";
                comando.CommandType = System.Data.CommandType.Text;

                comando.CommandText = "DELETE FROM ARTICULOS WHERE Id = @id";

                comando.Parameters.AddWithValue("@id", id);

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

        public List<Articulo> buscar(string criterio)
        {
            List<Articulo> lista = new List<Articulo>();

            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;";
                comando.CommandType = System.Data.CommandType.Text;

                comando.CommandText = "SELECT Id, Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio " +
                                     "FROM ARTICULOS " +
                                     "WHERE Codigo LIKE @criterio OR Nombre LIKE @criterio";

                comando.Parameters.AddWithValue("@criterio", "%" + criterio + "%");

                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.Id = (int)lector["Id"];
                    aux.Codigo = (string)lector["Codigo"];
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];

                    aux.Marca = new Marca();
                    aux.Marca.idMarca = (int)lector["IdMarca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.idCategoria = (int)lector["IdCategoria"];

                    aux.Precio = (decimal)lector["Precio"];

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
    }
}
