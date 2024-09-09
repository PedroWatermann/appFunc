using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace appFunc
{
	public class Funcionario
	{
		// Model =>
		public int Id { get; set; }
		public string nome { get; set; }
		public string turno { get; set; }
		public string data_nascimento { get; set; }
		public string matricula { get; set; }
        // Model =>

        // SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\Just Coding\\2 - Senai\\3 - Emerson\\Visual Studio\\appFunc\\dbFuncionario.mdf\";Integrated Security=True");
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"D:\\Just Coding\\2 - Senai\\3 - Emerson\\Visual Studio\\appFunc\\dbFuncionario.mdf\";Integrated Security=True");

        public List<Funcionario> ListaFuncionario()
		{
			List<Funcionario> li = new List<Funcionario>();

			string sql = "SELECT * FROM Funcionario";

			if (con.State == ConnectionState.Open)
			{
				con.Close();
			} 

			// Abrir conexão com o DB
			con.Open();

			// Preparando o comando SQL para acessar o DB
			SqlCommand cmd = new SqlCommand(sql, con);  // SqlCommand("query", "quem está fazendo a conexão")

			// Chegou ao DB (os registros estão em "dr" na forma de um array associativo)
			SqlDataReader dr = cmd.ExecuteReader();     // SqlDataReader => capta as informações no banco e armazena na forma de array associativo

			// Condição => enquanto for possível ler (tipo foreach)
			while (dr.Read())
			{
				Funcionario func = new Funcionario();

				func.Id = (int) dr["id"];
				func.nome = dr["nome"].ToString();
				func.turno = dr["turno"].ToString();
				func.data_nascimento = dr["data_nascimento"].ToString();
				func.matricula = dr["matricula"].ToString();

				li.Add(func);
			}

			dr.Close();
			con.Close();
			return li;
		}

		public void Inserir(string nome, string turno, string data_nascimento, string matricula)
		{
			try
			{
				string sql = $"INSERT INTO Funcionario(nome, turno, data_nascimento, matricula) VALUES ('{nome}', '{turno}', '{data_nascimento}', '{matricula}')";
				// string sql = $"INSERT INTO Funcionario(nome, turno, data_nascimento, matricula) VALUES ('"+nome+"', '"+turno+"', '"+data_nascimento+"', '"+matricula+"')";

				if (con.State == ConnectionState.Open)
				{
					con.Close();
				}
				
				con.Open();
				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.ExecuteNonQuery(); // Envia a query para o banco de dados
				con.Close();

			}
			catch (Exception er) // Genérico => assim que surgir qualquer exception ele já mostra
			{
				MessageBox.Show(er.Message);
			}
		}

		public void Localiza(int Id)
		{
			try
			{
				string sql = $"SELECT * FROM Funcionario WHERE Id = '{Id}'";

				if(con.State == ConnectionState.Open) 
				{ 
					con.Close(); 
				}

				con.Open();
				SqlCommand cmd = new SqlCommand(sql, con);
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					nome = dr["nome"].ToString();
					turno = dr["turno"].ToString();
					data_nascimento = dr["data_nascimento"].ToString();
					matricula = dr["matricula"].ToString();
				}
				dr.Close();
				con.Close();
			}
			catch (Exception er)
			{
				MessageBox.Show(er.Message);
			}
		}

		public void Atualizar(int Id, string nome, string turno, string data_nascimento, string matricula)
		{
			try
			{
				string sql = $"UPDATE Funcionario SET nome = '{nome}', turno = '{turno}', data_nascimento = '{data_nascimento}', matricula = '{matricula}' WHERE Id = {Id}";
				if (con.State == ConnectionState.Open)
				{
					con.Close();
				}
				con.Open();
				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.ExecuteNonQuery();
				con.Close();
			}
			catch (Exception er)
			{
				MessageBox.Show(er.Message);
			}
		}

		public void Excluir(int Id)
		{
			try
			{
				string sql = $"DELETE FROM Funcionario WHERE Id = '{Id}'";
				if (con.State == ConnectionState.Open)
				{
					con.Close();
				}
				con.Open();
				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.ExecuteNonQuery();
				con.Close();
			}
			catch (Exception er)
			{
				MessageBox.Show(er.Message);
			}
		}

		public bool RegistroRepetido(string matricula)
		{
			string sql = $"SELECT * FROM Funcionario WHERE matricula = '{matricula}'";
			if (con.State == ConnectionState.Open)
			{
				con.Close();
			}
			con.Open();
			SqlCommand cmd = new SqlCommand(sql, con);
			cmd.ExecuteNonQuery();

			var result = cmd.ExecuteScalar(); // Seleciona o valor da primeira linha da primeira coluna (neste caso é o ID)

			if (result != null)
			{
				return (int)result > 0;
			}

			return false;
		}

		public bool ExisteId (int Id)
		{
			string sql = $"SELECT * FROM Funcionario WHERE Id = '{Id}'";
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();

            var result = cmd.ExecuteScalar(); // Seleciona o valor da primeira linha da primeira coluna (neste caso é o ID)

            if (result != null)
            {
                return (int)result > 0;
            }

            return false;
        }

		public bool Aniversariantes(string data)
		{
			string sql = "SELECT data_nascimento FROM Funcionario";
			if (con.State == ConnectionState.Open)
			{
				con.Close();
			}
			con.Open();
			SqlCommand cmd = new SqlCommand(sql, con);
			SqlDataReader dr = cmd.ExecuteReader();

			var dataNiver = data;
			string data1 = dataNiver.Substring(0, 2); // Substring("posição", "qnts caracteres a partir da posição")
			string data2 = dataNiver.Substring(3, 2);
			
			while (dr.Read())
			{
				var dataNasc = dr["data_nascimento"].ToString();
				string dataN1 = dataNasc.Substring(0, 2); // dataN => Data de Nascimento
				string dataN2 = dataNasc.Substring(3, 2);
				if (data1.ToString() == dataN1.ToString() && data2.ToString() == dataN2.ToString())
				{
					return true;
				}
			}
			dr.Close();
			con.Close();
			return false;
		}

		public List<Funcionario> ListaAniversariantes()
		{
			List<Funcionario> li = new List<Funcionario>();

			string sql = "SELECT * FROM Funcionario";

			if (con.State == ConnectionState.Open)
			{
				con.Close();
			}
			con.Open();

			SqlCommand cmd = new SqlCommand(sql, con);
			SqlDataReader dr = cmd.ExecuteReader();
			
			var dataNiver = DateTime.Now.ToString("dd/MM/yyyy"); // Armazena em dataNiver a data de hoje
			string data1 = dataNiver.Substring(0, 2); // Armazena o dia de hoje
            string data2 = dataNiver.Substring(3, 2); // Armazena o mês em que estamos

			while (dr.Read()) // Este laço interage com o data reader, que armazenou todas as informações do DB em dr
			{
				Funcionario func = new Funcionario(); // Instancia o model (linhas iniciais)

				var dataNasc = dr["data_nascimento"].ToString(); // Armazena em dataNasc a data do registro do DB
				string dataN1 = dataNasc.Substring(0, 2); // Armazena o dia de nascimento do registro
				string dataN2 = dataNasc.Substring(3, 2); // Armazena o mês de nascimento do registro

				if (data1.ToString() == dataN1.ToString() && data2.ToString() == dataN2.ToString()) // Compara se o dia e o mês são iguais
				{
					func.Id = (int)dr["Id"]; // Armazena o id do registr
					func.nome = dr["nome"].ToString(); // Armazena o nome do registro
					func.turno = dr["turno"].ToString(); // Armazena o turno do registro
                    func.data_nascimento = dr["data_nascimento"].ToString(); // Armazena a data de nascimento do registro
                    func.matricula = dr["matricula"].ToString(); // Armazena a matrícula do registro
                    li.Add(func); // Adiciona todas as informações coletadas em func à lista (para ser mostrada para o usuário final)
				}
			}
			dr.Close();
			con.Close();
            return li;
        }
	}
}
