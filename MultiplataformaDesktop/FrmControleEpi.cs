using MultiplataformaDesktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FrmControleEpi : Form
    {
        public Controle controle;

        public FrmControleEpi()
        {
            InitializeComponent();
            FrmSplashScreen splash = new FrmSplashScreen();
            splash.Show();
            Application.DoEvents();
            Thread.Sleep(3000);
            splash.Close();

            controle = new Controle();
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            try
            {
                int matricula = Convert.ToInt32(txtMatricula.Text);
                DateTime dataEntrega = DateTime.Today;
                DateTime dataVencimento = Convert.ToDateTime(mtbVencimento.Text);

                string[] arrayEpis = new string[lvEpi.Items.Count];

                // Preenche o array com os EPIs da ListView
                for (int i = 0; i < lvEpi.Items.Count; i++)
                {
                    arrayEpis[i] = lvEpi.Items[i].Text;
                }

                // Inserir os EPIs no banco de dados
                controle.Inserir(matricula, arrayEpis, dataEntrega, dataVencimento);

                MessageBox.Show("Dados inseridos com sucesso!", "Inserção", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimparCampos();
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, insira um formato de data válido.", "Erro de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao inserir dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(txtId.Text.Trim());
                int matricula = Convert.ToInt32(txtMatricula.Text.Trim());
                string epi = cbxEpi.Text;
                DateTime dataEntrega = DateTime.Today;
                DateTime dataVencimento = Convert.ToDateTime(mtbVencimento.Text);

                controle.Editar(id, matricula, epi, dataEntrega, dataVencimento);

                MessageBox.Show("Matrícula atualizada com sucesso!", "Atualização", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                LimparCampos();
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, insira um formato de data válido.", "Erro de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar matrícula: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(txtId.Text.Trim());
                controle.Excluir(id);

                MessageBox.Show("Matrícula excluída com sucesso!", "Exclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                LimparCampos();
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, insira um ID válido.", "Erro de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir matrícula: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(txtId.Text);
                List<Controle> controles = controle.Localizar(id);

                if (controles != null && controles.Count > 0)
                {
                    txtMatricula.Text = controles[0].matricula.ToString();
                    mtbVencimento.Text = controles[0].data_vencimento.ToString("dd/MM/yyyy");

                    lvEpi.Items.Clear();
                    foreach (Controle ctrl in controles)
                    {
                        ListViewItem item = new ListViewItem(ctrl.epi);
                        lvEpi.Items.Add(item);
                    }
                }
                else
                {
                    MessageBox.Show("Registro não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, insira um ID válido.", "Erro de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao localizar registro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCampos()
        {
            txtMatricula.Clear();
            cbxEpi.SelectedIndex = -1;
            mtbVencimento.Clear();
            lvEpi.Items.Clear();
        }

        private void lvEpi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvEpi.SelectedItems.Count > 0)
            {
                string nomeEpiSelecionado = lvEpi.SelectedItems[0].Text;

                lvEpi.Items.Clear();

                foreach (Controle resultado in controle.Listar())
                {
                    foreach (Controle.Epi epi in resultado.epis)
                    {
                        if (epi.nome.Equals(nomeEpiSelecionado))
                        {
                            ListViewItem item = new ListViewItem(epi.nome);
                            item.SubItems.Add(epi.data_vencimento.ToString("dd/MM/yyyy"));
                            lvEpi.Items.Add(item);
                        }
                    }
                }
            }
        }

        private void cbxEpi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxEpi.SelectedItem != null)
            {
                string epiSelecionado = cbxEpi.SelectedItem.ToString();
                bool itemExiste = false;
                foreach (ListViewItem item in lvEpi.Items)
                {
                    if (item.Text == epiSelecionado)
                    {
                        itemExiste = true;
                        break;
                    }
                }

                if (itemExiste)
                {
                    MessageBox.Show("Este item já foi adicionado. Escolha outro item.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //ListViewItem newItem = new ListViewItem(epiSelecionado);
                    //lvEpi.Items.Add(newItem);
                }
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deseja realmente sair do Multiplataforma?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit(); 
            }
            else
            {
                return;
            }
        }

        private void dgvEpi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                lvEpi.Items.Clear();
                DataGridViewRow row = this.dgvEpi.Rows[e.RowIndex];
                this.dgvEpi.Rows[e.RowIndex].Selected = true;
                txtId.Text = row.Cells[0].Value.ToString();
                txtMatricula.Text = row.Cells[1].Value.ToString();
                mtbVencimento.Text = row.Cells[3].Value.ToString();

                string epiString = row.Cells[2].Value.ToString();
                string[] epis = epiString.Split(',');

                foreach (string epi in epis)
                {
                    ListViewItem newItem = new ListViewItem(epi.Trim());
                    lvEpi.Items.Add(newItem);
                }
            }
        }

        private void FrmControleEpi_Load(object sender, EventArgs e)
        {
            Controle controle = new Controle();
            List<Controle> controles = controle.Listar();
            dgvEpi.DataSource = controles;
            cbxEpi.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (cbxEpi.SelectedItem != null)
            {
                string epiSelecionado = cbxEpi.SelectedItem.ToString();
                bool itemExiste = false;
                foreach (ListViewItem item in lvEpi.Items)
                {
                    if (item.Text == epiSelecionado)
                    {
                        itemExiste = true;
                        break;
                    }
                }

                if (itemExiste)
                {
                    MessageBox.Show("Este item já foi adicionado. Escolha outro item.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    ListViewItem newItem = new ListViewItem(epiSelecionado);
                    lvEpi.Items.Add(newItem);
                }
            }
        }

        private void btnLinpar_Click(object sender, EventArgs e)
        {
            lvEpi.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Controle controle = new Controle();
            List<Controle> controles = controle.Listar();
            dgvEpi.DataSource = controles;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmFotoEpi frmFotoEpi = new FrmFotoEpi();
            frmFotoEpi.ShowDialog();
        }
    }
}
