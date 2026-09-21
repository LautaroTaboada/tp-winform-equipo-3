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
            Articulo seleccionado = (Articulo)dataGridView1.CurrentRow.DataBoundItem;
            ArticuloNegocio negocio = new ArticuloNegocio();

            negocio.eliminar(seleccionado.Id);

            cargarListado();
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

            Articulo seleccionado = (Articulo)dataGridView1.CurrentRow.DataBoundItem;

            ImagenNegocio imagenNegocio = new ImagenNegocio();
            seleccionado.Imagenes =
                imagenNegocio.listarPorArticulo(seleccionado.Id);
            if (seleccionado.Imagenes.Count > 0)
                cargarImagen(seleccionado.Imagenes[0].ImagenUrl);
        
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {

        }

        private void btnAdelante_Click(object sender, EventArgs e)
        {

        }
    }
}
