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
    public partial class FrmCategoria : Form
    {
        // Importante para conectarlo
        private System.Windows.Forms.DataGridView dgvCategorias;

        
        public FrmCategoria()
        {
            InitializeComponent();
        }

        private void FrmCategoria_Load(object sender, EventArgs e)
        {
            cargarGrilla();
        }

        private void cargarGrilla()
        {
            CategoriaNegocio negocio = new CategoriaNegocio();
            try
            {
                
                dgvCategorias.DataSource = negocio.listar();

                
                if (dgvCategorias.Columns["idCategoria"] != null)
                {
                    dgvCategorias.Columns["idCategoria"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.ToString());
            }
        }

       
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            
            if (dgvCategorias.CurrentRow == null)
            {
                MessageBox.Show("Por favor, seleccione una categoría de la lista.");
                return;
            }

            
            DialogResult respuesta = MessageBox.Show("¿Está seguro de que desea eliminar esta categoría? Esta acción no se puede deshacer.", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (respuesta == DialogResult.Yes)
            {
                Categoria seleccionada = (Categoria)dgvCategorias.CurrentRow.DataBoundItem;
                CategoriaNegocio negocio = new CategoriaNegocio();
                try
                {
                    
                    negocio.eliminar(seleccionada.idCategoria);
                    MessageBox.Show("Categoría eliminada correctamente.");
                    cargarGrilla(); 
                }
                catch (Exception ex)
                {
                
                    MessageBox.Show("Error al eliminar categoría. Verifique que no esté siendo usada por algún artículo.\n\nDetalles: " + ex.Message);
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            FrmAltaCategoria alta = new FrmAltaCategoria();


            alta.ShowDialog();

            cargarGrilla();

        }

        
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvCategorias.CurrentRow == null)
            {
                MessageBox.Show("Por favor, seleccione una categoría de la lista.");
                return;
            }

           
        }

      
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}