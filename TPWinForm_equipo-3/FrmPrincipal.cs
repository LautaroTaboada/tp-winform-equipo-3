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
        public FrmPrincipal()
        {
            InitializeComponent();
            
        }
        private void btnAdministrarMarcas_Click(object sender, EventArgs e)
        {
            FrmMarca formulario = new FrmMarca();
            formulario.ShowDialog();

            // Actualizar el listado al cerrar
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
            }
        }


        private void cargarListado()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            dataGridView1.DataSource = negocio.listar();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            try
            {
                string criterio = txtBuscar.Text.Trim();

                if (string.IsNullOrEmpty(criterio))
                {
                    cargarListado();
                    return;
                }

                ArticuloNegocio negocio = new ArticuloNegocio();

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = negocio.buscar(criterio);

                if (dataGridView1.Rows.Count == 0)
                {
                    articuloActual = null;
                    pictureBoxArticulo.Image = null;

                    MessageBox.Show(
                        "No se encontraron artículos con ese criterio.",
                        "Búsqueda",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrió un error al buscar el artículo: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmMarca formulario = new FrmMarca();
            formulario.ShowDialog();
        }

        private void cbFiltroCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
