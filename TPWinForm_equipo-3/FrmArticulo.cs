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
    public partial class FrmArticulo : Form
    {
        private Articulo articulo = null;

        public FrmArticulo()
        {
            InitializeComponent();
            cargarCombos();
        }

        public FrmArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            cargarCombos();
            cargarDatos();
        }

        private void cargarCombos()
        {
            MarcaNegocio negocioMarca = new MarcaNegocio();
            CategoriaNegocio negocioCategoria = new CategoriaNegocio();

            cmbMarca.DataSource = negocioMarca.listar();
            cmbMarca.DisplayMember = "Descripcion";
            cmbMarca.ValueMember = "IdMarca";

            cmbCategoria.DataSource = negocioCategoria.listar();
            cmbCategoria.DisplayMember = "Descripcion";
            cmbCategoria.ValueMember = "IdCategoria";
        }

        private void cargarDatos()
        {
            txtCodigo.Text = articulo.Codigo;
            txtNombre.Text = articulo.Nombre;
            txtDescripcion.Text = articulo.Descripcion;
            txtPrecio.Text = articulo.Precio.ToString();

            cmbMarca.SelectedValue = articulo.Marca.idMarca;
            cmbCategoria.SelectedValue = articulo.Categoria.idCategoria;
        }

        private void FrmArticulo_Load(object sender, EventArgs e)
        {
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validamos antes de asignar o tocar la base de datos
            if (!Validar())
                return;

            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (articulo == null)
                    articulo = new Articulo();

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;

                articulo.Marca = (Marca)cmbMarca.SelectedItem;
                articulo.Categoria = (Categoria)cmbCategoria.SelectedItem;

                articulo.Precio = decimal.Parse(txtPrecio.Text);

                if (articulo.Id == 0)
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }
                else
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Método de validación basado en el estilo del proyecto de referencia
        private bool Validar()
        {
            if (txtCodigo.Text.Length == 0)
            {
                MessageBox.Show("El código no puede estar vacío.");
                txtCodigo.Focus();
                return false;
            }

            if (txtNombre.Text.Length < 3)
            {
                MessageBox.Show("El nombre debe tener un mínimo de 3 caracteres.");
                txtNombre.Focus();
                return false;
            }

            if (!soloNumeros(txtPrecio.Text))
            {
                MessageBox.Show("Debe ingresar sólo valores numéricos en el precio.");
                txtPrecio.Focus();
                return false;
            }

            if (!separaPorComa(txtPrecio.Text))
            {
                MessageBox.Show("Use la coma ',' para separar los decimales.");
                txtPrecio.Focus();
                return false;
            }

            if (txtDescripcion.Text.Length < 5)
            {
                MessageBox.Show("La descripción debe tener un mínimo de 5 caracteres.");
                txtDescripcion.Focus();
                return false;
            }

            if (cmbMarca.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una marca.");
                cmbMarca.Focus();
                return false;
            }

            if (cmbCategoria.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una categoría.");
                cmbCategoria.Focus();
                return false;
            }

            return true;
        }

        private bool soloNumeros(string cadena)
        {
            try
            {
                decimal.Parse(cadena);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool separaPorComa(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (caracter == '.')
                    return false;
            }
            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
        }

        private void cmbMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}