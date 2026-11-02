using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace TPWinForm_equipo_3
{
    public partial class FrmPrincipal : Form
    {
        private int indiceImagen = 0;
        private Articulo articuloActual;
        private List<Articulo> listaArticulos;      // lista completa traída de la base (como listaPokemon del profe)
        private bool cargandoFiltros = false;       // evita filtrar mientras se cargan los combos
        public FrmPrincipal()
        {
            InitializeComponent();
            
        }
        private void btnAdministrarMarcas_Click(object sender, EventArgs e)
        {
            FrmMarca formulario = new FrmMarca();
            formulario.ShowDialog();

            // Recargar los combos de filtro (pudieron cambiar las marcas) y el listado
            cargarFiltros();
            cargarListado();
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pictureBoxArticulo.Image = null;

                if (string.IsNullOrWhiteSpace(imagen))
                {
                    return;
                }

                using (var cliente = new System.Net.WebClient())
                {
                    byte[] datos = cliente.DownloadData(imagen);

                    using (var memoria = new System.IO.MemoryStream(datos))
                    using (var imagenTemporal = Image.FromStream(memoria))
                    {
                        pictureBoxArticulo.Image = new Bitmap(imagenTemporal);
                    }
                }
            }
            catch (Exception)
            {
                pictureBoxArticulo.Image = null;
                pictureBoxArticulo.Load("https://img.magnific.com/vector-premium/icono-marco-fotos-foto-vacia-blanco-vector-sobre-fondo-transparente-aislado-eps-10_399089-1290.jpg");
            }
        }


        private void cargarListado()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                listaArticulos = negocio.listar();

                // Muestra la lista aplicando el texto y los filtros que estén elegidos
                aplicarFiltros(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los artículos: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbFiltroMarca.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFiltroCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFiltroMarca.SelectedIndexChanged += cbFiltroMarca_SelectedIndexChanged;

            cargarFiltros();
            cargarListado();

        }
        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmArticulo formulario = new FrmArticulo();
            formulario.ShowDialog();
            cargarListado();
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dataGridView1.Rows[e.RowIndex].DataBoundItem == null)
                return;

            Articulo seleccionado =
                dataGridView1.Rows[e.RowIndex].DataBoundItem as Articulo;

            if (seleccionado == null)
                return;

            FrmArticulo formulario = new FrmArticulo(seleccionado);
            formulario.ShowDialog();

            cargarListado();
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void eliminar(bool logico = false)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            ImagenNegocio negocioImg = new ImagenNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿De verdad querés eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dataGridView1.CurrentRow.DataBoundItem;
                    int idArticulo = seleccionado.Id;
                    negocio.eliminar(idArticulo);

                    cargarListado();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            aplicarFiltros(true);
        }


        private void Filtros_Click(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            FrmArticulo formulario = new FrmArticulo();
            formulario.ShowDialog();
            cargarListado();
        }

        private void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            FrmArticulo formulario = new FrmArticulo();
            formulario.ShowDialog();
            cargarListado();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBoxArticulo_Click(object sender, EventArgs e)
        {

        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null ||
                dataGridView1.CurrentRow.DataBoundItem == null)
            {
                articuloActual = null;
                indiceImagen = 0;
                pictureBoxArticulo.Image = null;
                return;
            }

            articuloActual = dataGridView1.CurrentRow.DataBoundItem as Articulo;
            
            if (articuloActual == null)
                return;

            try
            {
                ImagenNegocio imagenNegocio = new ImagenNegocio();

                articuloActual.Imagenes =
                    imagenNegocio.listarPorArticulo(articuloActual.Id);
                

                indiceImagen = 0;

                if (articuloActual.Imagenes != null &&
                    articuloActual.Imagenes.Count > 0)
                {
                    cargarImagen(articuloActual.Imagenes[indiceImagen].ImagenUrl);
                }
                else
                {
                    pictureBoxArticulo.Image = null;
                }
            }
            catch (Exception ex)
            {
                pictureBoxArticulo.Image = null;
                MessageBox.Show("No se pudieron cargar las imágenes: " + ex.Message);
            }
        }


        private void btnAtras_Click(object sender, EventArgs e)
        {
            if (articuloActual == null)
                return;

            if (indiceImagen > 0)
            {
                indiceImagen--;
                cargarImagen(articuloActual.Imagenes[indiceImagen].ImagenUrl);
        }
       
        }

        private void btnAdelante_Click(object sender, EventArgs e)
        {
            if (articuloActual == null)
                return;

            if (indiceImagen < articuloActual.Imagenes.Count - 1)
            {
                indiceImagen++;
                cargarImagen(articuloActual.Imagenes[indiceImagen].ImagenUrl);
            }
        }

        private void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            FrmCategoria ventana = new FrmCategoria();

            ventana.ShowDialog();

            // Recargar los combos de filtro (pudieron cambiar las categorías) y el listado
            cargarFiltros();
            cargarListado();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Mismo comportamiento que btnAdministrarMarcas_Click (recarga filtros y listado)
            btnAdministrarMarcas_Click(sender, e);
        }

        private void cbFiltroCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cargandoFiltros)
                aplicarFiltros(false);
        }

        private void cbFiltroMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cargandoFiltros)
                aplicarFiltros(false);
        }

        // Llena los combos de filtro con las marcas y categorías de la base.
        // El primer ítem es "Todas" (Id 0 = sin filtro).
        private void cargarFiltros()
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            try
            {
                cargandoFiltros = true;

                List<Marca> marcas = marcaNegocio.listar();
                Marca todasLasMarcas = new Marca();
                todasLasMarcas.idMarca = 0;
                todasLasMarcas.Descripcion = "Todas";
                marcas.Insert(0, todasLasMarcas);

                cbFiltroMarca.DisplayMember = "Descripcion";
                cbFiltroMarca.ValueMember = "idMarca";
                cbFiltroMarca.DataSource = marcas;

                List<Categoria> categorias = categoriaNegocio.listar();
                Categoria todasLasCategorias = new Categoria();
                todasLasCategorias.idCategoria = 0;
                todasLasCategorias.Descripcion = "Todas";
                categorias.Insert(0, todasLasCategorias);

                cbFiltroCategoria.DisplayMember = "Descripcion";
                cbFiltroCategoria.ValueMember = "idCategoria";
                cbFiltroCategoria.DataSource = categorias;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los filtros: " + ex.Message);
            }
            finally
            {
                cargandoFiltros = false;
            }
        }

        // Filtra la lista que ya está cargada en memoria (misma idea que txtFiltro_TextChanged
        // del profe: FindAll con una expresión lambda). Se combinan texto, marca y categoría.
        // avisarSinResultados: solo se pasa true desde el botón Buscar.
        private void aplicarFiltros(bool avisarSinResultados)
        {
            if (listaArticulos == null)
                return;

            List<Articulo> listaFiltrada = listaArticulos;

            string filtro = txtBuscar.Text.Trim().ToUpper();
            if (filtro != "")
            {
                listaFiltrada = listaFiltrada.FindAll(x => x.Codigo.ToUpper().Contains(filtro) || x.Nombre.ToUpper().Contains(filtro) || x.Descripcion.ToUpper().Contains(filtro));
            }

            if (cbFiltroMarca.SelectedValue is int && (int)cbFiltroMarca.SelectedValue != 0)
            {
                int idMarca = (int)cbFiltroMarca.SelectedValue;
                listaFiltrada = listaFiltrada.FindAll(x => x.Marca.idMarca == idMarca);
            }

            if (cbFiltroCategoria.SelectedValue is int && (int)cbFiltroCategoria.SelectedValue != 0)
            {
                int idCategoria = (int)cbFiltroCategoria.SelectedValue;
                listaFiltrada = listaFiltrada.FindAll(x => x.Categoria.idCategoria == idCategoria);
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaFiltrada;

            if (listaFiltrada.Count == 0)
            {
                articuloActual = null;
                pictureBoxArticulo.Image = null;

                if (avisarSinResultados)
                {
                    MessageBox.Show(
                        "No se encontraron artículos con ese criterio.",
                        "Búsqueda",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }
    }
}
