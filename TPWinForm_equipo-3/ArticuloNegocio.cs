using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace TPWinForm_equipo_3
{
    internal class ArticuloNegocio
    {
        public List<Articulo> lista()
        {
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true;";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT Codigo, Nombre, Descripcion, Precio FROM ARTICULOS";
                comando.Connection = conexion;

                conexion.Open();
            
                lector = comando.ExecuteReader();

                while (lector.Read()) 
                {
                    Articulo aux = new Articulo();
                    aux.Codigo = (string)lector["Codigo"]; 
                    aux.Nombre = (string) lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];
                    aux.Precio = (Decimal)lector["Precio"];

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
