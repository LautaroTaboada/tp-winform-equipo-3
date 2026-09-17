using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPWinForm_equipo_3
{
    internal class Articulo
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Marca Marca { get; set; } // guarda la marca y su id
        public Categoria Categoria { get; set; } // guarda la categoria y su id
        public decimal Precio { get; set; }
        public List<Imagen> Imagenes { get; set; } // Lista porque un articulo puede tener varias img
    }
}
