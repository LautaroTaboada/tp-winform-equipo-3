using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class MarcaNegocio
    {
        public List<Marca> listar()
        {
            List<Marca> lista = new List<Marca>();

            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT Id, Descripcion FROM MARCAS";
                comando.Connection = conexion;

                conexion.Open();

                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Marca aux = new Marca();

                    aux.idMarca = (int)lector["Id"];
                    aux.Descripcion = (string)lector["Descripcion"];

                    lista.Add(aux);
                }

                conexion.Close();

                return lista;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public void agregar(Marca nueva)
        {
            using (SqlConnection conexion = new SqlConnection(
                "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;"))
            {
                try
                {
                    string consulta = "INSERT INTO MARCAS (Descripcion) VALUES (@descripcion)";

                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@descripcion", nueva.Descripcion);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void modificar(Marca seleccionada)
        {
            using (SqlConnection conexion = new SqlConnection(
                "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;"))
            {
                try
                {
                    string consulta = "UPDATE MARCAS SET Descripcion = @descripcion WHERE Id = @id";

                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@descripcion", seleccionada.Descripcion);
                    comando.Parameters.AddWithValue("@id", seleccionada.idMarca);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public void eliminar(int id)
        {
            using (SqlConnection conexion = new SqlConnection(
                "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;"))
            {
                try
                {
                    string consulta = "DELETE FROM MARCAS WHERE Id = @id";

                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@id", id);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }



    }
}
