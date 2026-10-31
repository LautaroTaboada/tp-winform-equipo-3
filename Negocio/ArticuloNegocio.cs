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
                datos.SetearConsulta(@"
            SELECT
                A.Id,
                A.Codigo,
                A.Nombre,
                A.Descripcion,
                M.Descripcion AS Marca,
                C.Descripcion AS Categoria,
                A.Precio,
                A.IdMarca,
                A.IdCategoria
            FROM ARTICULOS A
            LEFT JOIN MARCAS M ON A.IdMarca = M.Id
            INNER JOIN CATEGORIAS C ON C.Id = A.IdCategoria
        ");

                datos.EjecutarLectura();

                while (datos.lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.Id = (int)datos.lector["Id"];
                    aux.Codigo = (string)datos.lector["Codigo"];
                    aux.Nombre = (string)datos.lector["Nombre"];
                    aux.Descripcion = (string)datos.lector["Descripcion"];

                    aux.Marca = new Marca();

                    if (datos.lector["IdMarca"] != DBNull.Value)
                        aux.Marca.idMarca = (int)datos.lector["IdMarca"];

                    aux.Marca.Descripcion = datos.lector["Marca"] == DBNull.Value
                        ? "Sin marca"
                        : (string)datos.lector["Marca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.idCategoria = (int)datos.lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.lector["Categoria"];

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
            using (SqlConnection conexion = new SqlConnection())
            {
                conexion.ConnectionString =
                    "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;";

                conexion.Open();

                using (SqlTransaction transaccion = conexion.BeginTransaction(
                    System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        string codigo;

                        using (SqlCommand comandoCodigo = new SqlCommand(
                            "SELECT ISNULL(MAX(TRY_CONVERT(INT, " +
                            "SUBSTRING(Codigo, 4, 50))), 0) + 1 " +
                            "FROM ARTICULOS WITH (UPDLOCK, HOLDLOCK) " +
                            "WHERE Codigo LIKE 'ART%' " +
                            "AND TRY_CONVERT(INT, SUBSTRING(Codigo, 4, 50)) IS NOT NULL",
                            conexion, transaccion))
                        {
                            int siguiente = (int)comandoCodigo.ExecuteScalar();
                            codigo = "ART" + siguiente.ToString("D3");
                        }

                        using (SqlCommand comando = new SqlCommand(
                            "INSERT INTO ARTICULOS " +
                            "(Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) " +
                            "VALUES (@codigo, @nombre, @descripcion, " +
                            "@idMarca, @idCategoria, @precio)",
                            conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@codigo", codigo);
                            comando.Parameters.AddWithValue("@nombre", articulo.Nombre);
                            comando.Parameters.AddWithValue("@descripcion", articulo.Descripcion);
                            comando.Parameters.AddWithValue("@idMarca", articulo.Marca.idMarca);
                            comando.Parameters.AddWithValue("@idCategoria", articulo.Categoria.idCategoria);
                            comando.Parameters.AddWithValue("@precio", articulo.Precio);

                            comando.ExecuteNonQuery();
                        }

                        transaccion.Commit();

                        articulo.Codigo = codigo;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }



        public void modificar(Articulo articulo)
        {
            using (SqlConnection conexion = new SqlConnection())
            using (SqlCommand comando = new SqlCommand())
            {
                conexion.ConnectionString =
                    "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;";

                comando.Connection = conexion;
                comando.CommandType = System.Data.CommandType.Text;

                comando.CommandText =
                    "UPDATE ARTICULOS SET Nombre = @nombre, " +
                    "Descripcion = @descripcion, IdMarca = @idMarca, " +
                    "IdCategoria = @idCategoria, Precio = @precio " +
                    "WHERE Id = @id";

                comando.Parameters.AddWithValue("@nombre", articulo.Nombre);
                comando.Parameters.AddWithValue("@descripcion", articulo.Descripcion);
                comando.Parameters.AddWithValue("@idMarca", articulo.Marca.idMarca);
                comando.Parameters.AddWithValue("@idCategoria", articulo.Categoria.idCategoria);
                comando.Parameters.AddWithValue("@precio", articulo.Precio);
                comando.Parameters.AddWithValue("@id", articulo.Id);

                conexion.Open();
                comando.ExecuteNonQuery();
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

            using (SqlConnection conexion = new SqlConnection(
                "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;"))
            {
                string consulta = @"SELECT 
                                A.Id,
                                A.Codigo,
                                A.Nombre,
                                A.Descripcion,
                                A.IdMarca,
                                A.IdCategoria,
                                M.Descripcion AS Marca,
                                C.Descripcion AS Categoria,
                                A.Precio
                            FROM ARTICULOS A
                            INNER JOIN MARCAS M ON M.Id = A.IdMarca
                            INNER JOIN CATEGORIAS C ON C.Id = A.IdCategoria
                            WHERE A.Codigo LIKE @criterio
                               OR A.Nombre LIKE @criterio";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue(
                        "@criterio", "%" + (criterio ?? "").Trim() + "%");

                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Articulo aux = new Articulo();

                            aux.Id = (int)lector["Id"];
                            aux.Codigo = (string)lector["Codigo"];
                            aux.Nombre = (string)lector["Nombre"];
                            aux.Descripcion = (string)lector["Descripcion"];

                            aux.Marca = new Marca();
                            aux.Marca.idMarca = (int)lector["IdMarca"];
                            aux.Marca.Descripcion = (string)lector["Marca"];

                            aux.Categoria = new Categoria();
                            aux.Categoria.idCategoria = (int)lector["IdCategoria"];
                            aux.Categoria.Descripcion = (string)lector["Categoria"];

                            aux.Precio = (decimal)lector["Precio"];

                            lista.Add(aux);
                        }
                    }
                }
            }

            return lista;
        }

    }
}
