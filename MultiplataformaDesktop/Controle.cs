using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Controle
    {
        public int id { get; set; }
        public int matricula { get; set; }
        public string epi { get; set; }
        public DateTime data_entrega { get; set; }
        public DateTime data_vencimento { get; set; }
        public List<Epi> epis { get; set; }

        public MySqlConnection con;

        public Controle()
        {
            con = new MySqlConnection("server=sql.freedb.tech; port=3306; database=freedb_MultiplataformaEPI; user=freedb_Multiplataforma; password=VxAbA!m4@RG3A?c");
            epis = new List<Epi>();
        }

        public class Epi
        {
            public string nome { get; set; }
            public DateTime data_entrega { get; set; }
            public DateTime data_vencimento { get; set; }
        }

        public void Inserir(int matricula, string[] epis, DateTime dataEntrega, DateTime dataVencimento)
        {
            try
            {
                con.Open();
                foreach (string epi in epis)
                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO epi (matricula, epi, data_entrega, data_vencimento) VALUES (@matricula, @epi, @dataEntrega, @dataVencimento)", con);
                    cmd.Parameters.AddWithValue("@matricula", matricula);
                    cmd.Parameters.AddWithValue("@epi", epi);
                    cmd.Parameters.AddWithValue("@dataEntrega", dataEntrega);
                    cmd.Parameters.AddWithValue("@dataVencimento", dataVencimento);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao inserir matrícula: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        public void Editar(int id, int matricula, string epi, DateTime dataEntrega, DateTime dataVencimento)
        {
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE epi SET matricula = @matricula, epi = @epi, data_entrega = @dataEntrega, data_vencimento = @dataVencimento WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@matricula", matricula);
                cmd.Parameters.AddWithValue("@epi", epi);
                cmd.Parameters.AddWithValue("@dataEntrega", dataEntrega);
                cmd.Parameters.AddWithValue("@dataVencimento", dataVencimento);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao editar matrícula: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        public void Excluir(int id)
        {
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM epi WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir matrícula: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        public List<Controle> Localizar(int id)
        {
            List<Controle> controles = new List<Controle>();

            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM epi WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Controle controle = new Controle();
                        controle.id = Convert.ToInt32(dr["id"]);
                        controle.matricula = Convert.ToInt32(dr["matricula"]);
                        controle.epi = dr["epi"].ToString();
                        controle.data_entrega = Convert.ToDateTime(dr["data_entrega"]).Date;
                        controle.data_vencimento = Convert.ToDateTime(dr["data_vencimento"]).Date;

                        controles.Add(controle);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao localizar matrículas: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

            return controles;
        }

        public List<Controle> Listar()
        {
            List<Controle> resultados = new List<Controle>();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM epi", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Controle controle = new Controle();
                        controle.id = Convert.ToInt32(dr["id"]);
                        controle.matricula = Convert.ToInt32(dr["matricula"]);
                        controle.epi = dr["epi"].ToString();
                        controle.data_entrega = Convert.ToDateTime(dr["data_entrega"]);
                        controle.data_vencimento = Convert.ToDateTime(dr["data_vencimento"]);
                        resultados.Add(controle);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar matrículas: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
            return resultados;
        }
    }
}