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

                if (articulo.Id != 0)
                {
                    articulo.Codigo = txtCodigo.Text;
                }
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

        // Método de validación basado en el estilo del proyecto de referencia[cite: 4]
        private bool Validar()
        {
            // Validar nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                txtNombre.Text.Trim().Length < 3)
            {
                MessageBox.Show("El nombre debe tener un mínimo de 3 caracteres.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            // Validar precio
            decimal precio;

            if (string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                !decimal.TryParse(
                    txtPrecio.Text.Trim(),
                    System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.CurrentCulture,
                    out precio))
            {
                MessageBox.Show("Ingrese un precio numérico válido.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return false;
            }

            if (precio <= 0)
            {
                MessageBox.Show("El precio debe ser mayor que cero.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return false;
            }

            // Validar descripción
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text) ||
                txtDescripcion.Text.Trim().Length < 5)
            {
                MessageBox.Show("La descripción debe tener un mínimo de 5 caracteres.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescripcion.Focus();
                return false;
            }

            // Validar marca
            if (cmbMarca.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una marca.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMarca.Focus();
                return false;
            }

            // Validar categoría
            if (cmbCategoria.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una categoría.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            Close();
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
        }

        private void cmbMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnVistaPrevia_Click(object sender, EventArgs e)
        {

        }

        private void txtCodigo_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}