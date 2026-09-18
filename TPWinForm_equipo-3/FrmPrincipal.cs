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

        private void cargarListado()
        {

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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
