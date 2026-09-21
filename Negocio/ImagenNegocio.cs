using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ImagenNegocio
    {

        public List<Imagen> listarPorArticulo(int idArticulo)
        {
            List<Imagen> lista = new List<Imagen>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    "SELECT Id, IdArticulo, ImagenUrl " +
                    "FROM IMAGENES " +
                    "WHERE IdArticulo = @IdArticulo");

                datos.SetearParametro("@IdArticulo", idArticulo);

                datos.EjecutarLectura();

                while (datos.lector.Read())
                {
                    Imagen aux = new Imagen();

                    aux.Id = (int)datos.lector["Id"];
                    aux.IdArticulo = (int)datos.lector["IdArticulo"];
                    aux.ImagenUrl = (string)datos.lector["ImagenUrl"];

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

    }
}
