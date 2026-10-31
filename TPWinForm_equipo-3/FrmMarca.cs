
using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TPWinForm_equipo_3
{
    public partial class FrmMarca : Form
    {
        private Label lblTitulo;
        private Label lblSeleccionar;
        private Label lblDescripcion;
        private ComboBox cmbMarca;
        private TextBox txtDescripcion;
        private DataGridView dgvMarcas;
        private Button btnAgregar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnCancelar;

        public FrmMarca()
        {
            InitializeComponent();
            CrearFormulario();
        }

        private void CrearFormulario()
        {
            Text = "Administración de marcas";
            Size = new Size(750, 550);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.White;

            lblTitulo = new Label();
            lblTitulo.Text = "ADMINISTRACIÓN DE MARCAS";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(30, 20);

            lblSeleccionar = new Label();
            lblSeleccionar.Text = "Seleccione una marca";
            lblSeleccionar.Location = new Point(30, 85);
            lblSeleccionar.AutoSize = true;

            cmbMarca = new ComboBox();
            cmbMarca.Name = "cmbMarca";
            cmbMarca.Location = new Point(30, 110);
            cmbMarca.Size = new Size(320, 30);
            cmbMarca.DropDownStyle = ComboBoxStyle.DropDownList;

            lblDescripcion = new Label();
            lblDescripcion.Text = "Nombre de la marca";
            lblDescripcion.Location = new Point(30, 165);
            lblDescripcion.AutoSize = true;

            txtDescripcion = new TextBox();
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Location = new Point(30, 190);
            txtDescripcion.Size = new Size(320, 30);

            btnAgregar = CrearBoton("Agregar", 30, 245);
            btnModificar = CrearBoton("Modificar", 190, 245);
            btnEliminar = CrearBoton("Eliminar", 30, 300);
            btnCancelar = CrearBoton("Cancelar", 190, 300);

            dgvMarcas = new DataGridView();
            dgvMarcas.Name = "dgvMarcas";
            dgvMarcas.Location = new Point(380, 85);
            dgvMarcas.Size = new Size(330, 350);
            dgvMarcas.ReadOnly = true;
            dgvMarcas.AllowUserToAddRows = false;
            dgvMarcas.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvMarcas.MultiSelect = false;
            dgvMarcas.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            btnAgregar.Click += btnAgregar_Click;
            btnModificar.Click += btnModificar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnCancelar.Click += btnCancelar_Click;

            Controls.AddRange(new Control[]
            {
                lblTitulo, lblSeleccionar, cmbMarca,
                lblDescripcion, txtDescripcion,
                btnAgregar, btnModificar, btnEliminar,
                btnCancelar, dgvMarcas
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

        private void cargarMarcas()
        {
            MarcaNegocio negocio = new MarcaNegocio();

            try
            {
                List<Marca> lista = negocio.listar();

                // Cargar ComboBox
                cmbMarca.DataSource = null;
                cmbMarca.DataSource = lista;
                cmbMarca.DisplayMember = "Descripcion";
                cmbMarca.ValueMember = "idMarca";

                // Cargar DataGridView
                dgvMarcas.DataSource = null;
                dgvMarcas.DataSource = lista;

                dgvMarcas.Columns["idMarca"].HeaderText = "ID";
                dgvMarcas.Columns["Descripcion"].HeaderText = "Descripción";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las marcas: " + ex.Message);
            }
        }
        private void FrmMarca_Load(object sender, EventArgs e)
        {
            cargarMarcas();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Ingrese el nombre de la marca.");
                return;
            }

            Marca nueva = new Marca();
            nueva.Descripcion = txtDescripcion.Text.Trim();

            MarcaNegocio negocio = new MarcaNegocio();

            try
            {
                negocio.agregar(nueva);

                MessageBox.Show("Marca agregada correctamente.");

                txtDescripcion.Clear();
                cargarMarcas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar la marca: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (cmbMarca.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una marca.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Ingrese la nueva descripción.");
                return;
            }

            Marca seleccionada = (Marca)cmbMarca.SelectedItem;
            seleccionada.Descripcion = txtDescripcion.Text.Trim();

            MarcaNegocio negocio = new MarcaNegocio();

            try
            {
                negocio.modificar(seleccionada);

                MessageBox.Show("Marca modificada correctamente.");

                cargarMarcas();
                txtDescripcion.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (cmbMarca.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una marca.");
                return;
            }

            Marca seleccionada = (Marca)cmbMarca.SelectedItem;

            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de que desea eliminar la marca " + seleccionada.Descripcion + "?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (resultado == DialogResult.Yes)
            {
                MarcaNegocio negocio = new MarcaNegocio();

                try
                {
                    negocio.eliminar(seleccionada.idMarca);

                    MessageBox.Show("Marca eliminada correctamente.");

                    cargarMarcas();
                    txtDescripcion.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo eliminar la marca. Puede estar asociada a uno o más artículos.\n" + ex.Message);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

