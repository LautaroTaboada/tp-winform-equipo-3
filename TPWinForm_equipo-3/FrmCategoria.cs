using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TPWinForm_equipo_3
{
    public partial class FrmCategoria : Form
    {
        private Label lblTitulo;
        private Label lblSeleccionar;
        private Label lblDescripcion;
        private ComboBox cmbCategoria;
        private TextBox txtDescripcion;
        private DataGridView dgvCategorias;
        private Button btnAgregar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnCancelar;

        // Evita que se disparen los eventos de selección mientras se recarga la lista
        private bool cargando = false;

        public FrmCategoria()
        {
            InitializeComponent();
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            Text = "Administración de categorías";
            Size = new Size(750, 550);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.White;

            lblTitulo = new Label();
            lblTitulo.Text = "ADMINISTRACIÓN DE CATEGORÍAS";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(30, 20);

            lblSeleccionar = new Label();
            lblSeleccionar.Text = "Seleccione una categoría";
            lblSeleccionar.Location = new Point(30, 85);
            lblSeleccionar.AutoSize = true;

            cmbCategoria = new ComboBox();
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.Location = new Point(30, 110);
            cmbCategoria.Size = new Size(320, 30);
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;

            lblDescripcion = new Label();
            lblDescripcion.Text = "Nombre de la categoría";
            lblDescripcion.Location = new Point(30, 165);
            lblDescripcion.AutoSize = true;

            txtDescripcion = new TextBox();
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Location = new Point(30, 190);
            txtDescripcion.Size = new Size(320, 30);
            txtDescripcion.MaxLength = 50;   // largo de la columna Descripcion en la base

            btnAgregar = CrearBoton("Agregar", 30, 245);
            btnModificar = CrearBoton("Modificar", 190, 245);
            btnEliminar = CrearBoton("Eliminar", 30, 300);
            btnCancelar = CrearBoton("Cancelar", 190, 300);

            dgvCategorias = new DataGridView();
            dgvCategorias.Name = "dgvCategorias";
            dgvCategorias.Location = new Point(380, 85);
            dgvCategorias.Size = new Size(330, 350);
            dgvCategorias.ReadOnly = true;
            dgvCategorias.AllowUserToAddRows = false;
            dgvCategorias.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvCategorias.MultiSelect = false;
            dgvCategorias.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            btnAgregar.Click += btnAgregar_Click;
            btnModificar.Click += btnModificar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnCancelar.Click += btnCancelar_Click;
            cmbCategoria.SelectedIndexChanged += cmbCategoria_SelectedIndexChanged;
            dgvCategorias.SelectionChanged += dgvCategorias_SelectionChanged;

            Controls.AddRange(new Control[]
            {
                lblTitulo, lblSeleccionar, cmbCategoria,
                lblDescripcion, txtDescripcion,
                btnAgregar, btnModificar, btnEliminar,
                btnCancelar, dgvCategorias
            });
        }

        private Button CrearBoton(string texto, int x, int y)
        {
            Button boton = new Button();
            boton.Text = texto;
            boton.Location = new Point(x, y);
            boton.Size = new Size(145, 40);
            boton.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            return boton;
        }

        private void cargarCategorias()
        {
            CategoriaNegocio negocio = new CategoriaNegocio();

            try
            {
                cargando = true;

                List<Categoria> lista = negocio.listar();

                // Cargar ComboBox
                cmbCategoria.DataSource = null;
                cmbCategoria.DataSource = lista;
                cmbCategoria.DisplayMember = "Descripcion";
                cmbCategoria.ValueMember = "idCategoria";

                // Cargar DataGridView
                dgvCategorias.DataSource = null;
                dgvCategorias.DataSource = lista;

                dgvCategorias.Columns["idCategoria"].HeaderText = "ID";
                dgvCategorias.Columns["Descripcion"].HeaderText = "Descripción";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las categorías: " + ex.Message);
            }
            finally
            {
                cargando = false;
            }
        }

        private void FrmCategoria_Load(object sender, EventArgs e)
        {
            cargarCategorias();
        }

        // Al hacer clic en una fila, queda elegida la misma categoría en el combo
        private void dgvCategorias_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando || dgvCategorias.CurrentRow == null)
                return;

            Categoria seleccionada = dgvCategorias.CurrentRow.DataBoundItem as Categoria;

            if (seleccionada != null)
                cmbCategoria.SelectedValue = seleccionada.idCategoria;
        }

        // Al elegir una categoría, su nombre se carga en el cuadro de texto para poder modificarlo
        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargando)
                return;

            Categoria seleccionada = cmbCategoria.SelectedItem as Categoria;

            if (seleccionada != null)
                txtDescripcion.Text = seleccionada.Descripcion;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            CategoriaNegocio negocio = new CategoriaNegocio();

            try
            {
                if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    MessageBox.Show("Ingrese el nombre de la categoría.");
                    return;
                }

                if (negocio.existe(txtDescripcion.Text, 0))
                {
                    MessageBox.Show("Ya existe una categoría con ese nombre.");
                    return;
                }

                Categoria nueva = new Categoria();
                nueva.Descripcion = txtDescripcion.Text.Trim();

                negocio.agregar(nueva);

                MessageBox.Show("Categoría agregada correctamente.");

                cargarCategorias();
                txtDescripcion.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar la categoría: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (cmbCategoria.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una categoría.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Ingrese la nueva descripción.");
                return;
            }

            Categoria seleccionada = (Categoria)cmbCategoria.SelectedItem;
            CategoriaNegocio negocio = new CategoriaNegocio();

            try
            {
                if (negocio.existe(txtDescripcion.Text, seleccionada.idCategoria))
                {
                    MessageBox.Show("Ya existe una categoría con ese nombre.");
                    return;
                }

                // Objeto nuevo: si falla el guardado, la lista no queda con datos falsos
                Categoria modificada = new Categoria();
                modificada.idCategoria = seleccionada.idCategoria;
                modificada.Descripcion = txtDescripcion.Text.Trim();

                negocio.modificar(modificada);

                MessageBox.Show("Categoría modificada correctamente.");

                cargarCategorias();
                txtDescripcion.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (cmbCategoria.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una categoría.");
                return;
            }

            Categoria seleccionada = (Categoria)cmbCategoria.SelectedItem;

            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de que desea eliminar la categoría " + seleccionada.Descripcion + "?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (resultado == DialogResult.Yes)
            {
                CategoriaNegocio negocio = new CategoriaNegocio();

                try
                {
                    negocio.eliminar(seleccionada.idCategoria);

                    MessageBox.Show("Categoría eliminada correctamente.");

                    cargarCategorias();
                    txtDescripcion.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
