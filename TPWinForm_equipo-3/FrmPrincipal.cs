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
        private void cargarImagen(string imagen)
        {
            try
            {
                pictureBoxArticulo.Load(imagen);
            }
            catch (Exception)
            {
                pictureBoxArticulo.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
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
            Articulo seleccionado = (Articulo)dataGridView1.CurrentRow.DataBoundItem;

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
            ArticuloNegocio negocio = new ArticuloNegocio();

            dataGridView1.DataSource = negocio.buscar(txtBuscar.Text);
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

            if (dataGridView1.CurrentRow == null)
                return;

            articuloActual = (Articulo)dataGridView1.CurrentRow.DataBoundItem;

            ImagenNegocio imagenNegocio = new ImagenNegocio();

            articuloActual.Imagenes =
                imagenNegocio.listarPorArticulo(articuloActual.Id);

            indiceImagen = 0;

            if (articuloActual.Imagenes.Count > 0)
                cargarImagen(articuloActual.Imagenes[indiceImagen].ImagenUrl);
        
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
    }
}
