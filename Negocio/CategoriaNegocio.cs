using Dominio;
using System;
using System.Collections.Generic;

namespace Negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> listar()
        {
            List<Categoria> lista = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT Id, Descripcion FROM CATEGORIAS");
                datos.EjecutarLectura();

                while (datos.lector.Read())
                {
                    Categoria aux = new Categoria();

                    aux.idCategoria = (int)datos.lector["Id"];
                    if (!(datos.lector["Descripcion"] is DBNull))
                        aux.Descripcion = (string)datos.lector["Descripcion"];

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

        public void agregar(Categoria nueva)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO CATEGORIAS (Descripcion) VALUES (@descripcion)");
                datos.SetearParametro("@descripcion", nueva.Descripcion);
                datos.EjecutarAccion();
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

        public void modificar(Categoria seleccionada)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE CATEGORIAS SET Descripcion = @descripcion WHERE Id = @id");
                datos.SetearParametro("@descripcion", seleccionada.Descripcion);
                datos.SetearParametro("@id", seleccionada.idCategoria);
                datos.EjecutarAccion();
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

        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // La base no tiene clave foránea: si no lo controlamos acá,
                // se borraría una categoría que todavía usan los artículos.
                if (estaEnUso(id))
                    throw new Exception("No se puede eliminar la categoría porque hay artículos que la usan.");

                datos.SetearConsulta("DELETE FROM CATEGORIAS WHERE Id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarAccion();
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

        // true si hay al menos un artículo con esa categoría
        public bool estaEnUso(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT COUNT(*) FROM ARTICULOS WHERE IdCategoria = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarLectura();

                datos.lector.Read();
                return (int)datos.lector[0] > 0;
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

        // true si ya existe OTRA categoría con ese nombre.
        // idExcluir: Id de la categoría que se está modificando (0 si es un alta).
        public bool existe(string descripcion, int idExcluir)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT COUNT(*) FROM CATEGORIAS WHERE LOWER(LTRIM(RTRIM(Descripcion))) = LOWER(@descripcion) AND Id <> @id");
                datos.SetearParametro("@descripcion", descripcion.Trim());
                datos.SetearParametro("@id", idExcluir);
                datos.EjecutarLectura();

                datos.lector.Read();
                return (int)datos.lector[0] > 0;
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
    }
}
