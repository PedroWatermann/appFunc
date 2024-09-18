using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appFunc
{
    public partial class frmAniversario : Form
    {
        public frmAniversario()
        {
            InitializeComponent();
        }

        private void frmAniversario_Load(object sender, EventArgs e)
        {
            Funcionario funcionario = new Funcionario();
            // List<Funcionario> func = funcionario.ListaAniversariantes();
            List<Funcionario> func = funcionario.AniversariantesMes();
            dgvAnversariantes.DataSource = func;
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
