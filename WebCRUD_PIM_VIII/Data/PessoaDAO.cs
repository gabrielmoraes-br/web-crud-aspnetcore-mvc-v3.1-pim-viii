using MySql.Data.MySqlClient;
using System;
using WebCRUD_PIM_VIII.Models;

namespace WebCRUD_PIM_VIII.Data
{
    public class PessoaDAO
    {
        //Criação da conexão MySQL para acesso ao banco de dados.
        private static string src = "server=localhost;userid=developer;password=1234567;database=webcrud";
        private static MySqlConnection Connection;

        /// ========================= MÉTODO DE INSERIR =========================
        public static bool Insira(Pessoa pessoa)
        {
            try
            {
                GravarEndereco(pessoa); //Grava o endereço da pessoa no banco de dados e garante o EnderecoId correto no método ObterId.

                MySqlCommand cmd = PrepararComando("INSERT INTO Pessoa (Nome, CPF, EnderecoId) VALUES (@Nome,@CPF,@EnderecoId)");

                cmd.Parameters.AddWithValue("@Nome", pessoa.Nome);
                cmd.Parameters.AddWithValue("@CPF", pessoa.CPF);
                cmd.Parameters.AddWithValue("@EnderecoId", ObterId("Endereco"));

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                GravarTelefones(pessoa); //Grava os telefones da pessoa no banco de dados.

                Connection.Close();
            }
            return true;
        }

        /// ========================= MÉTODO DE CONSULTA =========================
        public static Pessoa Consulte(long cpf)
        {
            //Objetos instanciados para iniciar a recuperação de dados do banco.
            Pessoa pessoa = new Pessoa();
            Endereco endereco = new Endereco();
            Telefone[] vect = new Telefone[2]; //vetor provisório para instanciar os dois telefones do formulário.
            int count = 0; //Este contador será usado para instanciar o vetor de telefones definito no final.

            //========================= DADOS DA TABELA DE PESSOA =========================

            //Recupera os dados da tabela Pessoa baseado no número de CPF consultado.
            try
            {
                MySqlCommand cmd = PrepararComando($"SELECT * FROM Pessoa WHERE CPF = {cpf}");

                MySqlDataReader reader = cmd.ExecuteReader();

                //Faz a leitura dos dados da tabela e insere nos atributos da Pessoa.
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        pessoa.Id = reader.GetInt16(0);
                        pessoa.Nome = reader.GetString(1);
                        pessoa.CPF = reader.GetInt64(2);
                        endereco.Id = reader.GetInt16(3);
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Connection.Close();
            }

            ///========================= DADOS DA TABELA DE ENDEREÇO =========================

            //Recupera o Endereço vinculado a Pessoa consultada e instancia no novo objeto.
            try
            {
                MySqlCommand cmd = PrepararComando($"SELECT * FROM Endereco WHERE Id = {endereco.Id}");

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        endereco.Logradouro = reader.GetString(1);
                        endereco.Numero = reader.GetInt32(2);
                        endereco.Cep = reader.GetInt32(3);
                        endereco.Bairro = reader.GetString(4);
                        endereco.Cidade = reader.GetString(5);
                        endereco.Estado = reader.GetString(6);
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Connection.Close();
            }

            //========================= DADOS DAS TABELAS DE TELEFONE =========================

            //PARTE 1: Recupera os ids dos telefones cadastrados baseado no id da pessoa consultada.
            try
            {
                MySqlCommand cmd = PrepararComando($"SELECT * FROM Pessoa_Telefone WHERE Id_Pessoa = {pessoa.Id}");

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    for (int i = 0; reader.Read(); i++)
                    {
                        vect[i] = new Telefone { Id = reader.GetInt16(0) };

                        //O contador armazenará os valores válidos, sem considerar espaços nulos no index
                        //(caso cadastre apenas 1 telefone).
                        count++; //Isso fará com que não aconteçam comandos SQL para espaços nulos.
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Connection.Close();
            }

            //PARTE 2: Recupera os números de telefones, DDDs e os ids do TipoTelefone.
            for (int i = 0; i < count; i++)
            {
                try
                {
                    MySqlCommand cmd = PrepararComando($"SELECT * FROM Telefone WHERE Id = {vect[i].Id}");

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            vect[i].Numero = reader.GetInt32(1);
                            vect[i].DDD = reader.GetInt16(2);
                            vect[i].TipoTelefone = new TipoTelefone { Id = reader.GetInt16(3) };
                        }
                    }
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Connection.Close();
                }

                //PARTE 3: Recupera os tipos de telefones que foram cadastrados nestes TipoTelefoneIds.
                try
                {
                    MySqlCommand cmd = PrepararComando($"SELECT * FROM Tipo_Telefone WHERE Id = {vect[i].TipoTelefone.Id}");

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            vect[i].TipoTelefone.Tipo = reader.GetString(1);
                        }
                    }
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Connection.Close();
                }
            }

            //PARTE 4: Termina de instanciar os atributos que constituem uma Pessoa, retornando-a.

            Telefone[] telefones = new Telefone[count]; //Vetor instanciado sem espaços nulos.

            for (int i = 0; i < count; i++)
            {
                telefones[i] = vect[i];
            }

            //Atribui o endereço e os telefones recuperados para o novo objeto Pessoa.
            pessoa.Endereco = endereco;
            pessoa.Telefones = telefones;

            return pessoa;
        }

        /// ========================= MÉTODO DE ALTERAÇÃO =========================
        public static bool Altere(Pessoa pessoa)
        {
            //Altera as informações básicas da pessoa na tabela do banco de dados, Nome e CPF.
            try
            {
                MySqlCommand cmd = PrepararComando($"UPDATE Pessoa SET Nome=@Nome, CPF=@CPF WHERE Id = {pessoa.Id}");

                cmd.Parameters.AddWithValue("@Nome", pessoa.Nome);
                cmd.Parameters.AddWithValue("@CPF", pessoa.CPF);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                Connection.Close();
            }

            //Altera o endereço completo da pessoa no banco de dados.
            try
            {
                MySqlCommand cmd = PrepararComando($"UPDATE Endereco SET Logradouro=@Logradouro, Numero=@Numero, Cep=@Cep, Bairro=@Bairro, Cidade=@Cidade, Estado=@Estado WHERE Id = {pessoa.Endereco.Id}");

                cmd.Parameters.AddWithValue("@Logradouro", pessoa.Endereco.Logradouro);
                cmd.Parameters.AddWithValue("@Numero", pessoa.Endereco.Numero);
                cmd.Parameters.AddWithValue("@Cep", pessoa.Endereco.Cep);
                cmd.Parameters.AddWithValue("@Bairro", pessoa.Endereco.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", pessoa.Endereco.Cidade);
                cmd.Parameters.AddWithValue("@Estado", pessoa.Endereco.Estado);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                Connection.Close();
            }

            //Altera os telefones da pessoa no banco de dados.
            for (int i = 0; i < pessoa.Telefones.Length; i++)
            {
                try
                {
                    MySqlCommand cmd = PrepararComando($"UPDATE Telefone SET Numero=@Numero, DDD=@DDD WHERE Id = {pessoa.Telefones[i].Id}");

                    cmd.Parameters.AddWithValue("@Numero", pessoa.Telefones[i].Numero);
                    cmd.Parameters.AddWithValue("@DDD", pessoa.Telefones[i].DDD);

                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                finally
                {
                    Connection.Close();
                }

                //Altera os Tipos de telefones no banco de dados.
                try
                {
                    MySqlCommand cmd = PrepararComando($"UPDATE Tipo_Telefone SET Tipo=@Tipo WHERE Id = {pessoa.Telefones[i].TipoTelefone.Id}");

                    cmd.Parameters.AddWithValue("@Tipo", pessoa.Telefones[i].TipoTelefone.Tipo);

                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                finally
                {
                    Connection.Close();
                }
            }
            return true;
        }

        /// ========================= MÉTODO DE EXCLUSÃO =========================
        public static bool Exclua(Pessoa pessoa)
        {
            //Exclusão dos números de telefone de acordo com o Id da Pessoa.
            //Esta exclusão precisa acontecer antes da exclusão da Pessoa, devido associação na tabela.
            try
            {
                MySqlCommand cmd = PrepararComando("DELETE FROM Telefone WHERE PessoaId = @PessoaId");

                cmd.Parameters.AddWithValue("@PessoaId", pessoa.Id);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                Connection.Close();
            }

            //Exclusão da Pessoa de acordo com seu Id.
            try
            {
                MySqlCommand cmd = PrepararComando("DELETE FROM Pessoa WHERE Id = @PessoaId");

                cmd.Parameters.AddWithValue("@PessoaId", pessoa.Id);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                Connection.Close();
            }

            //Exclusão do endereço da Pessoa de acordo com seu Id.
            try
            {
                MySqlCommand cmd = PrepararComando("DELETE FROM Endereco WHERE Id = @EnderecoId");

                cmd.Parameters.AddWithValue("@EnderecoId", pessoa.Endereco.Id);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                Connection.Close();
            }

            //Exclusão de telefones e Pessoa da tabela associativa.
            try
            {
                MySqlCommand cmd = PrepararComando("DELETE FROM Pessoa_Telefone WHERE Id_Pessoa = @PessoaId");

                cmd.Parameters.AddWithValue("@PessoaId", pessoa.Id);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                Connection.Close();
            }

            //Exclusão dos tipos de telefones que eram vinculados aos ids de telefones deletados.
            for (int i = 0; i < pessoa.Telefones.Length; i++)
            {
                try
                {
                    MySqlCommand cmd = PrepararComando("DELETE FROM Tipo_Telefone WHERE Id = @TipoTelefoneId");

                    cmd.Parameters.AddWithValue("@TipoTelefoneId", pessoa.Telefones[i].TipoTelefone.Id);

                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                finally
                {
                    Connection.Close();
                }
            }

            return true;
        }

        ///========================= GRAVAÇÃO DE ENDEREÇO =========================
        protected static bool GravarEndereco(Pessoa pessoa)
        {
            try
            {
                MySqlCommand cmd = PrepararComando("INSERT INTO Endereco (Logradouro, Numero, Cep, Bairro, Cidade, Estado) " +
                    "VALUES (@Logradouro,@Numero,@Cep,@Bairro,@Cidade,@Estado)");

                cmd.Parameters.AddWithValue("@Logradouro", pessoa.Endereco.Logradouro);
                cmd.Parameters.AddWithValue("@Numero", pessoa.Endereco.Numero);
                cmd.Parameters.AddWithValue("@Cep", pessoa.Endereco.Cep);
                cmd.Parameters.AddWithValue("@Bairro", pessoa.Endereco.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", pessoa.Endereco.Cidade);
                cmd.Parameters.AddWithValue("@Estado", pessoa.Endereco.Estado);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                Connection.Close();
            }
            return true;
        }

        ///========================= GRAVAÇÃO DE TELEFONES =========================
        protected static bool GravarTelefones(Pessoa pessoa)
        {
            for (int i = 0; i < pessoa.Telefones.Length; i++)
            {
                //Adiciona os tipos dos telefones, gerando os ids de tipo.
                //Precisa acontecer antes para que o telefone não fique com o campo tipo nulo.
                try
                {
                    MySqlCommand cmd = PrepararComando("INSERT INTO Tipo_Telefone (Tipo) VALUES (@Tipo)");

                    cmd.Parameters.AddWithValue("@Tipo", pessoa.Telefones[i].TipoTelefone.Tipo); ;

                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                finally
                {
                    Connection.Close();
                }

                //Adiciona os telefones com os ids de tipo previamente gerados.
                try
                {
                    MySqlCommand cmd = PrepararComando("INSERT INTO Telefone (Numero, DDD, TipoTelefoneId, PessoaId) VALUES (@Numero,@DDD,@TipoTelefoneId,@PessoaId)");

                    cmd.Parameters.AddWithValue("@Numero", pessoa.Telefones[i].Numero);
                    cmd.Parameters.AddWithValue("@DDD", pessoa.Telefones[i].DDD);
                    cmd.Parameters.AddWithValue("@TipoTelefoneId", ObterId("Tipo_Telefone"));
                    cmd.Parameters.AddWithValue("@PessoaId", ObterId("Pessoa"));

                    cmd.ExecuteNonQuery();

                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                finally
                {
                    Connection.Close();
                }

                //Adiciona os valores de ids na tabela associativa Pessoa_Telefone.
                try
                {
                    MySqlCommand cmd = PrepararComando("INSERT INTO Pessoa_Telefone (Id_Pessoa, Id_Telefone) VALUES (@Id_Pessoa, @Id_Telefone)");

                    cmd.Parameters.AddWithValue("@Id_Pessoa", ObterId("Pessoa"));
                    cmd.Parameters.AddWithValue("@Id_Telefone", ObterId("Telefone"));

                    cmd.ExecuteNonQuery();

                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                finally
                {
                    Connection.Close();
                }
            }

            return true;
        }

        //Método que recupera o último id adicionado em uma tabela.
        protected static int ObterId(string table)
        {
            int result = 0;

            try
            {
                MySqlCommand cmd = PrepararComando($"SELECT MAX(Id) FROM {table}");

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result = reader.GetInt16(0);
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Connection.Close();
            }
            return result;
        }

        //Método que instancia a conexão MySQL e prepara o comando de texto.
        private static MySqlCommand PrepararComando(string sqlText)
        {
            Connection = new MySqlConnection(src);
            Connection.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = Connection;

            cmd.CommandText = sqlText;
            cmd.Prepare();

            return cmd;
        }
    }
}
