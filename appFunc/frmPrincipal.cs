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
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
            
            Funcionario funcionario = new Funcionario();
            string data = DateTime.Now.ToString("dd/MM/yyyy");

            if (funcionario.Aniversariantes(data))
            {
                pbxAniversario.Visible = true;
            }
            else
            {
                pbxAniversario.Visible = false;
            }
        }

        public void limpaCampos(bool controle)
        {
            txtId.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtMatricula.Text = string.Empty;
            cbxTurno.Text = string.Empty;
            mskDataNascimento.Text = string.Empty;
            if (controle)
            {
                this.ActiveControl = txtNome;
            }
        }

        public void editar(bool controle)
        {
            btnEditar.Enabled = controle;
            btnExcluir.Enabled = controle;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Funcionario funcionario = new Funcionario();
            List<Funcionario> func = funcionario.ListaFuncionario();
            dgvFuncionario.DataSource = func;
            editar(false);
            // btnEditar.Enabled = false;
            // btnExcluir.Enabled = false;
            this.ActiveControl = txtNome;
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            if (txtNome.Text == "" || txtMatricula.Text == "" || cbxTurno.Text == "" || mskDataNascimento.Text == "")
            {
                MessageBox.Show("Por favor, preencha todos os campos!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ActiveControl = txtNome;
                return;
            }

            try
            {
                Funcionario funcionario = new Funcionario();

                if (funcionario.RegistroRepetido(txtMatricula.Text) == true)
                {
                    MessageBox.Show("Este registro já existe em nossa base de dados!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    limpaCampos(true);
                    // txtNome.Text = string.Empty;
                    // txtMatricula.Text = string.Empty;
                    // cbxTurno.Text = string.Empty;
                    // mskDataNascimento.Text = string.Empty;
                    // this.ActiveControl = txtNome;
                }
                else
                {
                    funcionario.Inserir(txtNome.Text, cbxTurno.Text, mskDataNascimento.Text, txtMatricula.Text);
                    MessageBox.Show("Registro cadastrado com sucesso!", "Cadastrar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    List<Funcionario> func = funcionario.ListaFuncionario();
                    dgvFuncionario.DataSource = func;

                    limpaCampos(true);
                    // txtNome.Text = string.Empty;
                    // txtMatricula.Text = string.Empty;
                    // cbxTurno.Text = string.Empty;
                    // mskDataNascimento.Text = string.Empty;
                    // this.ActiveControl = txtNome;

                    string data = DateTime.Now.ToString("dd/MM/yyyy");
                    if (funcionario.Aniversariantes(data) == false)
                    {
                        pbxAniversario.Visible = false;
                    }
                    else
                    {
                        pbxAniversario.Visible = true;
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void btnLocalizar_Click(object sender, EventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("Por favor, digite um ID válido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                limpaCampos(false);
                txtId.Text = string.Empty;
                this.ActiveControl = txtId;
                return;
            }

            try
            {
                Funcionario funcionario = new Funcionario();

                int Id = Convert.ToInt32(txtId.Text.Trim());

                if (funcionario.ExisteId(Id) == false)
                {
                    MessageBox.Show("Registro inexistente!\nPor favor, tente novamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    limpaCampos(false);
                    this.ActiveControl = txtId;
                    return;
                }
                else
                {
                    funcionario.Localiza(Id);
                    txtNome.Text = funcionario.nome;
                    cbxTurno.Text = funcionario.turno;
                    mskDataNascimento.Text = funcionario.data_nascimento;
                    txtMatricula.Text = funcionario.matricula;

                    if (txtNome.Text != null)
                    {
                        editar(true);
                        // btnEditar.Enabled = true;
                        // btnExcluir.Enabled = true;
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Deseja mesmo editar este registro?", "Editar", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                try
                {
                    // int Id = Convert.ToInt32(txtId.Text.Trim());
                    Funcionario funcionario = new Funcionario();

                    funcionario.Atualizar(Convert.ToInt32(txtId.Text.Trim()), txtNome.Text.Trim(), cbxTurno.Text.Trim(), mskDataNascimento.Text, txtMatricula.Text.Trim());
                    MessageBox.Show("Registro atualizado com sucesso!", "Atualizar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    List<Funcionario> func = funcionario.ListaFuncionario();
                    dgvFuncionario.DataSource = func;
                    limpaCampos(false);
                    editar(false);
                    // btnEditar.Enabled = false;
                    // btnExcluir.Enabled = false;

                    string data = DateTime.Now.ToString("dd/MM/yyyy");

                    if (funcionario.Aniversariantes(data) == false)
                    {
                        pbxAniversario.Visible = false;
                    }
                    else
                    {
                        pbxAniversario.Visible = true;
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int Id = Convert.ToInt32(txtId.Text.Trim());
            Funcionario funcionario = new Funcionario();

            funcionario.Excluir(Id);

            // Se resultado fosse declarado com var, ele assumiria o tipo DialogResult
            /*ou var*/ DialogResult resultado = MessageBox.Show("Deseja realmente excluir este registro?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (resultado == DialogResult.Yes)
            {
                MessageBox.Show("Registro excluído com sucesso!", "Excluir", MessageBoxButtons.OK, MessageBoxIcon.Information);
                List<Funcionario> func = funcionario.ListaFuncionario();
                dgvFuncionario.DataSource = func;

                limpaCampos(true);
                editar(false);
                // btnEditar.Enabled = false;
                // btnExcluir.Enabled = false;

                string data = DateTime.Now.ToString("dd/MM/yyyy");
                if (funcionario.Aniversariantes(data) == false)
                {
                    pbxAniversario.Visible = false;
                }
                else
                {
                    pbxAniversario.Visible = true;
                }
            }
            
        }

        private void dgvFuncionario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Recebe do datagrid a linha que o usuário clicou
                DataGridViewRow row = this.dgvFuncionario.Rows[e.RowIndex];
                // Seleciona a linha clicada
                this.dgvFuncionario.Rows[e.RowIndex].Selected = true;

                // Mostra os valores ao usuário
                txtId.Text = row.Cells[0].Value.ToString();
                txtNome.Text = row.Cells[1].Value.ToString();
                cbxTurno.Text = row.Cells[2].Value.ToString();
                mskDataNascimento.Text = row.Cells[3].Value.ToString();
                txtMatricula.Text = row.Cells[4].Value.ToString();
            }
            editar(true);
            // btnEditar.Enabled = true;
            // btnExcluir.Enabled = true;
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limpaCampos(true);
            editar(false);
        }
    }
}
