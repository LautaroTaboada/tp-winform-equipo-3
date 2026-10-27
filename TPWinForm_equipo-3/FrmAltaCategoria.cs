using System;
using System.Windows.Forms;
using Dominio; // Para usar la clase Categoria
using Negocio; // Para usar CategoriaNegocio

namespace TPWinForm_equipo_3
{
    public partial class FrmAltaCategoria : Form
    {
        // Variable local para guardar el objeto si estamos modificando
        private Categoria categoria = null;

        // --- Constructor 1: Para AGREGAR (Crea una ventana vacía) ---
        public FrmAltaCategoria()
        {
            InitializeComponent();
            Text = "Agregar Categoría"; // Cambiamos el título de la ventana
        }

        // --- Constructor 2: Para MODIFICAR (Crea una ventana precargada) ---
        public FrmAltaCategoria(Categoria categoria)
        {
            InitializeComponent();
            this.categoria = categoria; // Guardamos la categoría que nos pasaron
            Text = "Modificar Categoría"; // Cambiamos el título
        }

        private void FrmAltaCategoria_Load(object sender, EventArgs e)
        {
            // Si la variable 'categoria' NO es nula, significa que el constructor 2 fue llamado
            // y estamos MODIFICANDO.
            if (categoria != null)
            {
                // Cargamos el TextBox con la descripción actual
                txtDescripcion.Text = categoria.Descripcion;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Simplemente cierra esta ventana
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // 1. Instanciamos la capa de Negocio
            CategoriaNegocio negocio = new CategoriaNegocio();

            try
            {
                // 2. Validación básica: que no esté vacío
                if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    MessageBox.Show("Por favor, ingrese un nombre para la categoría.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3. Si 'categoria' es nula, es un ALTA nueva. Creamos el objeto.
                if (categoria == null)
                {
                    categoria = new Categoria();
                }

                // 4. Cargamos el valor del TextBox en el objeto
                categoria.Descripcion = txtDescripcion.Text;

                // 5. Decidimos qué método llamar: agregar() o modificar()
                // Si cat.Id != 0, ya existe en la DB -> Modificar.
                if (categoria.idCategoria != 0) // Usa 'Id' o 'idCategoria', según tu clase Categoria
                {
                    negocio.modificar(categoria);
                    MessageBox.Show("Categoría modificada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else // Si es 0 (o nulo en DB) -> Agregar.
                {
                    negocio.agregar(categoria);
                    MessageBox.Show("Categoría agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // 6. Cerramos la ventana y volvemos a la principal
                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar guardar la categoría: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

